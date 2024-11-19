using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using Effanville.Common.Structure.DataStructures;

using NUnit.Framework;

namespace Effanville.Common.Structure.Tests.DataStructures.Money
{
    /// <summary>
    /// Tests to ensure that a TimeList as a field or property of another class
    /// is serialized correctly.
    /// </summary>
    [TestFixture]
    public sealed class TimeListFieldTests
    {
        private static string enl = TestConstants.EnvNewLine;

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

        private static (string name, TestClass testList, int count, bool any, string XmlString)[] TestLists()
        {
            return new[] {
                ("empty",
                new TestClass(
                new TimeList(new List<DailyValuation>())),
                0,
                false,
                $"<?xml version=\"1.0\" encoding=\"utf-8\"?>{enl}<TestClass xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">{enl}  <Vals>{enl}    <Values />{enl}  </Vals>{enl}</TestClass>"),
                ("single",
                new TestClass(new TimeList(
                    new List<DailyValuation>()
                    {
                    new DailyValuation(new DateTime(2018, 1, 1), 0.0m)
                    })),
                1,
                true,
                $"<?xml version=\"1.0\" encoding=\"utf-8\"?>{enl}<TestClass xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">{enl}  <Vals>{enl}    <Values>{enl}      <DV D=\"2018-01-01T00:00:00\" V=\"0.0\" />{enl}    </Values>{enl}  </Vals>{enl}</TestClass>"),
                ("repeatedValue",
                new TestClass(new TimeList(
                new List<DailyValuation>()
                {
                    new DailyValuation(new DateTime(2018, 1, 1), 0.0m),
                    new DailyValuation(new DateTime(2018, 1, 1), 0.0m)
                })),
            1,
            true,
            $"<?xml version=\"1.0\" encoding=\"utf-8\"?>{enl}<TestClass xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">{enl}  <Vals>{enl}    <Values>{enl}      <DV D=\"2018-01-01T00:00:00\" V=\"0.0\" />{enl}      <DV D=\"2018-01-01T00:00:00\" V=\"0.0\" />{enl}    </Values>{enl}  </Vals>{enl}</TestClass>"),
            (
            "standardList",
                new TestClass(new TimeList(
                    new List<DailyValuation>()
                    {
                        new DailyValuation(new DateTime(2018, 1, 1), 0.0m),
                        new DailyValuation(new DateTime(2019, 1, 1), 1.0m),
                        new DailyValuation(new DateTime(2019, 5, 1), 2.0m),
                        new DailyValuation(new DateTime(2019, 5, 5), 0.0m)
                    })),
                4,
                true,
                $"<?xml version=\"1.0\" encoding=\"utf-8\"?>{enl}<TestClass xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">{enl}  <Vals>{enl}    <Values>{enl}      <DV D=\"2018-01-01T00:00:00\" V=\"0.0\" />{enl}      <DV D=\"2019-01-01T00:00:00\" V=\"1.0\" />{enl}      <DV D=\"2019-05-01T00:00:00\" V=\"2.0\" />{enl}      <DV D=\"2019-05-05T00:00:00\" V=\"0.0\" />{enl}    </Values>{enl}  </Vals>{enl}</TestClass>")
            };
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
            var xmlWriterSettings = new XmlWriterSettings()
            {
                Indent = true
            };
            using (MemoryStream stream = new MemoryStream())
            using (XmlWriter writer = XmlWriter.Create(new StreamWriter(stream), xmlWriterSettings))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(TestClass));
                serializer.Serialize(writer, times);

                string output = Encoding.UTF8.GetString(stream.ToArray());

                Assert.That(output, Is.EqualTo(expectedXml));
            }
        }

        private static IEnumerable<TestCaseData> ReadSerializationData(string testName)
        {
            var tests = TestLists();
            foreach (var test in tests)
            {
                yield return new TestCaseData(test.XmlString, test.testList).SetName($"{testName}-{test.name}");
            }

            yield return new TestCaseData(
                $"<?xml version=\"1.0\" encoding=\"utf-8\"?>{enl}<TestClass xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">{enl}  <Vals>{enl}    <Values>{enl}<DailyValuation>{enl}<Day>2018-01-01T00:00:00</Day>{enl}<Value>0</Value>{enl}</DailyValuation>{enl}<DailyValuation>{enl}<Day>2019-01-01T00:00:00</Day>{enl}<Value>1.0</Value>{enl}</DailyValuation>{enl}<DailyValuation>{enl}<Day>2019-05-01T00:00:00</Day>{enl}<Value>2.0</Value>{enl}</DailyValuation>{enl}<DailyValuation>{enl}<Day>2019-05-05T00:00:00</Day>{enl}<Value>0</Value>{enl}</DailyValuation>{enl}</Values>{enl}</Vals>{enl}</TestClass>", 
                new TestClass(new TimeList(new List<DailyValuation>() { new DailyValuation(new DateTime(2018, 1, 1), 0.0m), new DailyValuation(new DateTime(2019, 1, 1), 1.0m), new DailyValuation(new DateTime(2019, 5, 1), 2.0m), new DailyValuation(new DateTime(2019, 5, 5), 0.0m) })))
                .SetName($"{testName}-old1");
            yield return new TestCaseData($"<?xml version=\"1.0\" encoding=\"utf-8\"?>{enl}<TestClass xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">{enl}  <Vals>{enl}<Values>{enl}    <DailyValuation>{enl}<Day>2018-01-01T00:00:00</Day>{enl}<Value>0</Value>{enl}</DailyValuation>{enl}</Values>{enl}  </Vals>{enl}</TestClass>", new TestClass(new TimeList(new List<DailyValuation>() { new DailyValuation(new DateTime(2018, 1, 1), 0.0m) })))
                .SetName($"{testName}-old2");
            yield return new TestCaseData($"<?xml version=\"1.0\" encoding=\"utf-8\"?>{enl}<TestClass xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">{enl}  <Vals>{enl}<Values />{enl}  </Vals></TestClass>", new TestClass(new TimeList(new List<DailyValuation>())))
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

                Assert.That(valuation, Is.EqualTo(timelist));
            }
        }

        [TestCaseSource(nameof(WriteSerializationData), new object[] { nameof(XmlRoundTripTests) })]
        public void XmlRoundTripTests(string expectedXml, TestClass timelist)
        {
            string output;
            var xmlWriterSettings = new XmlWriterSettings()
            {
                Indent = true
            };
            using (var stream = new MemoryStream())
            using (XmlWriter writer = XmlWriter.Create(new StreamWriter(stream), xmlWriterSettings))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(TestClass));
                serializer.Serialize(writer, timelist);

                output = Encoding.UTF8.GetString(stream.ToArray());
            }

            Assert.That(output, Is.EqualTo(expectedXml));

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

                Assert.That(valuation, Is.EqualTo(timelist));
            }
        }
    }
}
