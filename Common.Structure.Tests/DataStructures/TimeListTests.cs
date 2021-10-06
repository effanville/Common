using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using NUnit.Framework;
using Common.Structure.DataStructures;

namespace Common.Structure.Tests.DataStructures
{
    [TestFixture]
    public sealed class TimeListTests
    {
        private static IEnumerable<(string name, TimeList testList, int count, bool any, string XmlString)> TestLists()
        {
            yield return ("empty", TimeListTestData.GetTestTimeList(TimeListTestData.EmptyListKey), 0, false, "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<Values />");
            yield return ("single", TimeListTestData.GetTestTimeList(TimeListTestData.SingleEntryKey), 1, true, "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<Values>\r\n  <DV D=\"2018-01-01T00:00:00\" V=\"1000\" />\r\n</Values>");
            yield return ("repeatedValue", new TimeList(new List<DailyValuation>() { new DailyValuation(new DateTime(2018, 1, 1), 0.0), new DailyValuation(new DateTime(2018, 1, 1), 0.0) }), 1, true, "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<Values>\r\n  <DV D=\"2018-01-01T00:00:00\" V=\"0\" />\r\n  <DV D=\"2018-01-01T00:00:00\" V=\"0\" />\r\n</Values>");
            yield return ("standardList", new TimeList(new List<DailyValuation>() { new DailyValuation(new DateTime(2018, 1, 1), 0.0), new DailyValuation(new DateTime(2019, 1, 1), 1.0), new DailyValuation(new DateTime(2019, 5, 1), 2.0), new DailyValuation(new DateTime(2019, 5, 5), 0.0) }), 4, true, "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<Values>\r\n  <DV D=\"2018-01-01T00:00:00\" V=\"0\" />\r\n  <DV D=\"2019-01-01T00:00:00\" V=\"1\" />\r\n  <DV D=\"2019-05-01T00:00:00\" V=\"2\" />\r\n  <DV D=\"2019-05-05T00:00:00\" V=\"0\" />\r\n</Values>");
        }

        private static IEnumerable<TestCaseData> TryAddValueTestSource()
        {
            yield return new TestCaseData(0, Array.Empty<(DateTime, double)>());
            yield return new TestCaseData(1, new[] { (new DateTime(2018, 1, 1), 0.0), (new DateTime(2018, 1, 1), 0.0) });
            yield return new TestCaseData(2, new[] { (new DateTime(2018, 1, 1), 0.0), (new DateTime(2019, 1, 1), 0.0) });
        }

        [TestCaseSource(nameof(TryAddValueTestSource))]
        public void SetDataTests(int count, params (DateTime date, double value)[] first)
        {
            TimeList newList = new TimeList();
            foreach (var value in first)
            {
                newList.SetData(value.date, value.value);
            }
            Assert.AreEqual(count, newList.Count());
        }
        private static IEnumerable<TestCaseData> AnyTestSource()
        {
            var tests = TestLists();
            foreach (var test in tests)
            {
                yield return new TestCaseData(test.any, test.testList).SetName($"Any-{test.name}");
            }
        }

        [TestCaseSource(nameof(AnyTestSource))]
        public void AnyTests(bool result, TimeList listToTest)
        {
            Assert.AreEqual(result, listToTest.Any());
        }

        private static IEnumerable<TestCaseData> CleanValuesTestSource()
        {
            yield return new TestCaseData(new TimeList(), new TimeList());
            yield return new TestCaseData(
                new TimeList(new List<DailyValuation>() { new DailyValuation(new DateTime(2018, 1, 1), 0.0) }),
                new TimeList(new List<DailyValuation>() { new DailyValuation(new DateTime(2018, 1, 1), 0.0), new DailyValuation(new DateTime(2019, 1, 1), 0.0) }));
            yield return new TestCaseData(
                new TimeList(new List<DailyValuation>() { new DailyValuation(new DateTime(2018, 1, 1), 0.0) }),
                new TimeList(new List<DailyValuation>() { new DailyValuation(new DateTime(2018, 1, 1), 0.0), new DailyValuation(new DateTime(2019, 1, 1), 0.0), new DailyValuation(new DateTime(2019, 5, 1), 0.0), new DailyValuation(new DateTime(2019, 5, 5), 0.0) }));
            yield return new TestCaseData(
                new TimeList(new List<DailyValuation>() { new DailyValuation(new DateTime(2018, 1, 1), 0.0), new DailyValuation(new DateTime(2019, 5, 1), 2.0) }),
                new TimeList(new List<DailyValuation>() { new DailyValuation(new DateTime(2018, 1, 1), 0.0), new DailyValuation(new DateTime(2019, 1, 1), 0.0), new DailyValuation(new DateTime(2019, 5, 1), 2.0), new DailyValuation(new DateTime(2019, 5, 5), 2.0) }));
            yield return new TestCaseData(
                new TimeList(new List<DailyValuation>() { new DailyValuation(new DateTime(2018, 1, 1), 0.0), new DailyValuation(new DateTime(2019, 1, 1), 1.0), new DailyValuation(new DateTime(2019, 5, 1), 2.0), new DailyValuation(new DateTime(2019, 5, 5), 0.0) }),
                new TimeList(new List<DailyValuation>() { new DailyValuation(new DateTime(2018, 1, 1), 0.0), new DailyValuation(new DateTime(2019, 1, 1), 1.0), new DailyValuation(new DateTime(2019, 5, 1), 2.0), new DailyValuation(new DateTime(2019, 5, 5), 0.0) }));
        }

        [TestCaseSource(nameof(CleanValuesTestSource))]
        public void CleanValuesTests(TimeList expectedCleaned, TimeList timelist)
        {
            timelist.CleanValues();
            int count = expectedCleaned.Count();
            Assert.AreEqual(count, timelist.Count());
            Assert.AreEqual(timelist, expectedCleaned);
        }

        [TestCaseSource(nameof(ValuesTestSource))]
        public void ValuesTests(double? expectedResult, DateTime expectedDate, DateTime date, params (DateTime date, double value)[] first)
        {
            var timelist = new TimeList();
            foreach (var value in first)
            {
                timelist.SetData(value.date, value.value);
            }

            var actualValue = timelist.Value(date);
            if (expectedResult == null)
            {
                Assert.IsNull(actualValue);
            }
            else
            {
                Assert.AreEqual(expectedDate, actualValue.Day, $" date not correct");
                Assert.AreEqual(expectedResult, actualValue.Value, $" value not correct");
            }
        }

        [TestCaseSource(nameof(ValuesTestSource))]
        public void ValuesSpecialFuncTests(double? expectedResult, DateTime expectedDate, DateTime date, (DateTime date, double value)[] first)
        {
            var timelist = new TimeList();
            foreach (var value in first)
            {
                timelist.SetData(value.date, value.value);
            }

            double interpolator(DailyValuation earlier, DailyValuation later, DateTime chosenDate) => earlier.Value + (later.Value - earlier.Value) / (later.Day - earlier.Day).Days * (chosenDate - earlier.Day).Days;
            var actualValue = timelist.Value(date, interpolator);
            if (expectedResult == null)
            {
                Assert.IsNull(actualValue);
            }
            else
            {
                Assert.AreEqual(expectedDate, actualValue.Day, $" date not correct");
                Assert.AreEqual(expectedResult, actualValue.Value, $" value not correct");
            }
        }

        private static IEnumerable<TestCaseData> ValuesTestSource()
        {
            yield return new TestCaseData(null, new DateTime(2018, 1, 1), new DateTime(2018, 1, 1), Array.Empty<(DateTime, double)>());
            yield return new TestCaseData(0.0, new DateTime(2018, 1, 1), new DateTime(2018, 1, 1), new[] { (new DateTime(2018, 1, 1), 0.0), (new DateTime(2019, 1, 1), 0.0) });
            yield return new TestCaseData(0.0, new DateTime(2018, 1, 1), new DateTime(2017, 1, 1), new[] { (new DateTime(2018, 1, 1), 0.0), (new DateTime(2019, 1, 1), 0.0), (new DateTime(2019, 5, 1), 0.0), (new DateTime(2019, 5, 5), 0.0) });
            yield return new TestCaseData(0.0, new DateTime(2018, 1, 1), new DateTime(2018, 1, 1), new[] { (new DateTime(2018, 1, 1), 0.0), (new DateTime(2019, 1, 1), 0.0), (new DateTime(2019, 5, 1), 0.0), (new DateTime(2019, 5, 5), 0.0) });
            yield return new TestCaseData(0.0, new DateTime(2019, 5, 5), new DateTime(2020, 1, 1), new[] { (new DateTime(2018, 1, 1), 0.0), (new DateTime(2019, 1, 1), 0.0), (new DateTime(2019, 5, 1), 0.0), (new DateTime(2019, 5, 5), 0.0) });
            yield return new TestCaseData(0.0, new DateTime(2018, 1, 1), new DateTime(2018, 1, 1), new[] { (new DateTime(2018, 1, 1), 0.0), (new DateTime(2019, 1, 1), 0.0), (new DateTime(2019, 5, 1), 2.0), (new DateTime(2019, 5, 5), 2.0) });
            yield return new TestCaseData(1.05, new DateTime(2019, 3, 5), new DateTime(2019, 3, 5), new[] { (new DateTime(2018, 1, 1), 0.0), (new DateTime(2019, 1, 1), 0.0), (new DateTime(2019, 5, 1), 2.0), (new DateTime(2019, 5, 5), 2.0) });
            yield return new TestCaseData(2.0, new DateTime(2019, 5, 1), new DateTime(2019, 5, 1), new[] { (new DateTime(2018, 1, 1), 0.0), (new DateTime(2019, 1, 1), 0.0), (new DateTime(2019, 5, 1), 2.0), (new DateTime(2019, 5, 5), 2.0) });
            yield return new TestCaseData(0.0, new DateTime(2018, 1, 1), new DateTime(2018, 1, 1), new[] { (new DateTime(2018, 1, 1), 0.0), (new DateTime(2019, 1, 1), 1.0), (new DateTime(2019, 5, 1), 2.0), (new DateTime(2019, 5, 5), 0.0) });
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
        public void WriteXmlTests(string expectedXml, TimeList times)
        {
            using (StringWriter fs = new StringWriter())
            {
                XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
                xmlWriterSettings.NewLineOnAttributes = false;
                xmlWriterSettings.Encoding = Encoding.UTF8;
                xmlWriterSettings.Indent = true;
                using (XmlWriter writer = XmlWriter.Create(fs, xmlWriterSettings))
                {
                    times.WriteXml(writer);
                }

                string output = fs.ToString();

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

            yield return new TestCaseData("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<Values>\r\n<DailyValuation>\r\n<Day>2018-01-01T00:00:00</Day>\r\n<Value>0</Value>\r\n</DailyValuation>\r\n<DailyValuation>\r\n<Day>2019-01-01T00:00:00</Day>\r\n<Value>1.0</Value>\r\n</DailyValuation>\r\n<DailyValuation>\r\n<Day>2019-05-01T00:00:00</Day>\r\n<Value>2.0</Value>\r\n</DailyValuation>\r\n<DailyValuation>\r\n<Day>2019-05-05T00:00:00</Day>\r\n<Value>0</Value>\r\n</DailyValuation>\r\n</Values>", new TimeList(new List<DailyValuation>() { new DailyValuation(new DateTime(2018, 1, 1), 0.0), new DailyValuation(new DateTime(2019, 1, 1), 1.0), new DailyValuation(new DateTime(2019, 5, 1), 2.0), new DailyValuation(new DateTime(2019, 5, 5), 0.0) }))
                .SetName($"{testName}-old1");
            yield return new TestCaseData("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<Values>\r\n<DailyValuation>\r\n<Day>2018-01-01T00:00:00</Day>\r\n<Value>0</Value>\r\n</DailyValuation>\r\n</Values>", new TimeList(new List<DailyValuation>() { new DailyValuation(new DateTime(2018, 1, 1), 0.0) }))
                .SetName($"{testName}-old2");
            yield return new TestCaseData("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<Values />", new TimeList(new List<DailyValuation>()))
.SetName($"{testName}-old3");
        }

        [TestCaseSource(nameof(ReadSerializationData), new object[] { nameof(ReadXmlTests) })]
        public void ReadXmlTests(string expectedXml, TimeList timelist)
        {
            var valuation = new TimeList();
            using (StringReader fs = new StringReader(expectedXml))
            {
                XmlReaderSettings xmlWriterSettings = new XmlReaderSettings();
                using (XmlReader reader = XmlReader.Create(fs, xmlWriterSettings))
                {
                    valuation.ReadXml(reader);
                }

                Assert.AreEqual(timelist, valuation);
            }
        }

        [TestCaseSource(nameof(WriteSerializationData), new object[] { nameof(XmlRoundTripTests) })]
        public void XmlRoundTripTests(string expectedXml, TimeList timelist)
        {
            string output;
            using (StringWriter fs = new StringWriter())
            {
                XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
                xmlWriterSettings.NewLineOnAttributes = false;
                xmlWriterSettings.Encoding = Encoding.UTF8;
                xmlWriterSettings.Indent = true;
                using (XmlWriter writer = XmlWriter.Create(fs, xmlWriterSettings))
                {
                    timelist.WriteXml(writer);
                }

                output = fs.ToString();
            }

            Assert.AreEqual(expectedXml, output);

            var valuation = new TimeList();
            using (StringReader fs = new StringReader(output))
            {
                XmlReaderSettings xmlWriterSettings = new XmlReaderSettings();
                using (XmlReader reader = XmlReader.Create(fs, xmlWriterSettings))
                {
                    valuation.ReadXml(reader);
                }

                Assert.AreEqual(valuation, timelist);
            }
        }
    }
}
