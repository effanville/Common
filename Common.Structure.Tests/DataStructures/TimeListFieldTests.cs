﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using NUnit.Framework;
using Common.Structure.DataStructures;

namespace Common.Structure.Tests.DataStructures
{
    /// <summary>
    /// Tests to ensure that a TimeList as a field or property of another class
    /// is serialized correctly.
    /// </summary>
    public sealed class TimeListFieldTests
    {
        public sealed class TestClass : IEquatable<TestClass>
        {
            public TimeList Vals { get; set; } = new TimeList();
            public TestClass()
            {
            }

            public TestClass(TimeList values)
            {
                Vals = values;
            }

            public bool Equals(TestClass other)
            {
                if (other == null)
                {
                    return false;
                }

                return Vals.Equals(other.Vals);
            }
        }

        private static IEnumerable<(string name, TestClass testList, int count, bool any, string XmlString)> TestLists()
        {
            yield return ("empty", new TestClass(new TimeList(new List<DailyValuation>())), 0, false, "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<TestClass xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <Vals>\r\n    <Values />\r\n  </Vals>\r\n</TestClass>");
            yield return ("single", new TestClass(new TimeList(new List<DailyValuation>() { new DailyValuation(new DateTime(2018, 1, 1), 0.0) })), 1, true, "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<TestClass xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <Vals>\r\n    <Values>\r\n      <DV D=\"2018-01-01T00:00:00\" V=\"0\" />\r\n    </Values>\r\n  </Vals>\r\n</TestClass>");
            yield return ("repeatedValue", new TestClass(new TimeList(new List<DailyValuation>() { new DailyValuation(new DateTime(2018, 1, 1), 0.0), new DailyValuation(new DateTime(2018, 1, 1), 0.0) })), 1, true, "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<TestClass xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <Vals>\r\n    <Values>\r\n      <DV D=\"2018-01-01T00:00:00\" V=\"0\" />\r\n      <DV D=\"2018-01-01T00:00:00\" V=\"0\" />\r\n    </Values>\r\n  </Vals>\r\n</TestClass>");
            yield return ("standardList", new TestClass(new TimeList(new List<DailyValuation>() { new DailyValuation(new DateTime(2018, 1, 1), 0.0), new DailyValuation(new DateTime(2019, 1, 1), 1.0), new DailyValuation(new DateTime(2019, 5, 1), 2.0), new DailyValuation(new DateTime(2019, 5, 5), 0.0) })), 4, true, "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<TestClass xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <Vals>\r\n    <Values>\r\n      <DV D=\"2018-01-01T00:00:00\" V=\"0\" />\r\n      <DV D=\"2019-01-01T00:00:00\" V=\"1\" />\r\n      <DV D=\"2019-05-01T00:00:00\" V=\"2\" />\r\n      <DV D=\"2019-05-05T00:00:00\" V=\"0\" />\r\n    </Values>\r\n  </Vals>\r\n</TestClass>");
        }

        private static IEnumerable<TestCaseData> WriteSerializationData(string testName)
        {
            var tests = TestLists();
            foreach (var test in tests)
            {
                yield return new TestCaseData(test.XmlString, test.testList).SetName($"{testName}-{test.name}");
            }
        }

        [TestCaseSource(nameof(WriteSerializationData), new object[] { nameof(WriteXmlTests) })]
        public void WriteXmlTests(string expectedXml, TestClass times)
        {
            using (var stream = new MemoryStream())
            using (TextWriter writer = new StreamWriter(stream))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(TestClass));
                serializer.Serialize(writer, times);

                string output = Encoding.UTF8.GetString(stream.ToArray());

                Assert.AreEqual(expectedXml, output);
            }
        }

        private static IEnumerable<TestCaseData> ReadSerializationData(string testName)
        {
            var tests = TestLists();
            foreach (var test in tests)
            {
                yield return new TestCaseData(test.XmlString, test.testList).SetName($"{testName}-{test.name}");
            }

            yield return new TestCaseData("<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<TestClass xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <Vals>\r\n    <Values>\r\n<DailyValuation>\r\n<Day>2018-01-01T00:00:00</Day>\r\n<Value>0</Value>\r\n</DailyValuation>\r\n<DailyValuation>\r\n<Day>2019-01-01T00:00:00</Day>\r\n<Value>1.0</Value>\r\n</DailyValuation>\r\n<DailyValuation>\r\n<Day>2019-05-01T00:00:00</Day>\r\n<Value>2.0</Value>\r\n</DailyValuation>\r\n<DailyValuation>\r\n<Day>2019-05-05T00:00:00</Day>\r\n<Value>0</Value>\r\n</DailyValuation>\r\n</Values>\r\n</Vals>\r\n</TestClass>", new TestClass(new TimeList(new List<DailyValuation>() { new DailyValuation(new DateTime(2018, 1, 1), 0.0), new DailyValuation(new DateTime(2019, 1, 1), 1.0), new DailyValuation(new DateTime(2019, 5, 1), 2.0), new DailyValuation(new DateTime(2019, 5, 5), 0.0) })))
                .SetName($"{testName}-old1");
            yield return new TestCaseData("<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<TestClass xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <Vals>\r\n<Values>\r\n    <DailyValuation>\r\n<Day>2018-01-01T00:00:00</Day>\r\n<Value>0</Value>\r\n</DailyValuation>\r\n</Values>\r\n  </Vals>\r\n</TestClass>", new TestClass(new TimeList(new List<DailyValuation>() { new DailyValuation(new DateTime(2018, 1, 1), 0.0) })))
                .SetName($"{testName}-old2");
            yield return new TestCaseData("<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<TestClass xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <Vals>\r\n<Values />\r\n  </Vals></TestClass>", new TestClass(new TimeList(new List<DailyValuation>())))
                .SetName($"{testName}-old3");
        }

        [TestCaseSource(nameof(ReadSerializationData), new object[] { nameof(ReadXmlTests) })]
        public void ReadXmlTests(string expectedXml, TestClass timelist)
        {
            TestClass valuation = null;
            using (Stream fs = new MemoryStream(Encoding.UTF8.GetBytes(expectedXml)))
            {
                using (TextReader reader = new StreamReader(fs))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(TestClass));
                    var valuationAsObject = serializer.Deserialize(reader);
                    if (valuationAsObject is TestClass val)
                    {
                        valuation = val;
                    }
                }

                Assert.AreEqual(timelist, valuation);
            }
        }

        [TestCaseSource(nameof(WriteSerializationData), new object[] { nameof(XmlRoundTripTests) })]
        public void XmlRoundTripTests(string expectedXml, TestClass timelist)
        {
            string output;
            using (var stream = new MemoryStream())
            using (TextWriter writer = new StreamWriter(stream))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(TestClass));
                serializer.Serialize(writer, timelist);

                output = Encoding.UTF8.GetString(stream.ToArray());
            }

            Assert.AreEqual(expectedXml, output);

            TestClass valuation = null;
            using (Stream fs = new MemoryStream(Encoding.UTF8.GetBytes(expectedXml)))
            {
                using (TextReader reader = new StreamReader(fs))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(TestClass));
                    var valuationAsObject = serializer.Deserialize(reader);
                    if (valuationAsObject is TestClass val)
                    {
                        valuation = val;
                    }
                }

                Assert.AreEqual(timelist, valuation);
            }
        }

    }
}