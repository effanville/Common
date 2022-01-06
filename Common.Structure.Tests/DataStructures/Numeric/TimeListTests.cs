using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using NUnit.Framework;
using Common.Structure.DataStructures.Numeric;

namespace Common.Structure.Tests.DataStructures.Numeric
{
    [TestFixture]
    public sealed class TimeListTests
    {
        private static IEnumerable<(string name, TimeNumberList testList, int count, bool any, string XmlString)> TestLists()
        {
            yield return ("empty", TimeListTestData.GetTestTimeList(TimeListTestData.EmptyListKey), 0, false, "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<Values />");
            yield return ("single", TimeListTestData.GetTestTimeList(TimeListTestData.SingleEntryKey), 1, true, "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<Values>\r\n  <DV D=\"2018-01-01T00:00:00\" V=\"1000\" />\r\n</Values>");
            yield return ("repeatedValue", new TimeNumberList(new List<DailyNumeric>() { new DailyNumeric(new DateTime(2018, 1, 1), 0.0), new DailyNumeric(new DateTime(2018, 1, 1), 0.0) }), 1, true, "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<Values>\r\n  <DV D=\"2018-01-01T00:00:00\" V=\"0\" />\r\n  <DV D=\"2018-01-01T00:00:00\" V=\"0\" />\r\n</Values>");
            yield return ("standardList", new TimeNumberList(new List<DailyNumeric>() { new DailyNumeric(new DateTime(2018, 1, 1), 0.0), new DailyNumeric(new DateTime(2019, 1, 1), 1.0), new DailyNumeric(new DateTime(2019, 5, 1), 2.0), new DailyNumeric(new DateTime(2019, 5, 5), 0.0) }), 4, true, "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<Values>\r\n  <DV D=\"2018-01-01T00:00:00\" V=\"0\" />\r\n  <DV D=\"2019-01-01T00:00:00\" V=\"1\" />\r\n  <DV D=\"2019-05-01T00:00:00\" V=\"2\" />\r\n  <DV D=\"2019-05-05T00:00:00\" V=\"0\" />\r\n</Values>");
        }

        private static IEnumerable<TestCaseData> TryAddValueTestSource()
        {
            yield return new TestCaseData(0, Array.Empty<(DateTime, double)>()).SetName($"{nameof(SetDataTests)}-EmptyTimelist");
            yield return new TestCaseData(1, new[] { (new DateTime(2018, 1, 1), 0.0), (new DateTime(2018, 1, 1), 0.0) }).SetName($"{nameof(SetDataTests)}-AddSameValue");
            yield return new TestCaseData(2, new[] { (new DateTime(2018, 1, 1), 0.0), (new DateTime(2019, 1, 1), 0.0) }).SetName($"{nameof(SetDataTests)}-AddDifferentValue");
        }

        [TestCaseSource(nameof(TryAddValueTestSource))]
        public void SetDataTests(int count, params (DateTime date, double value)[] first)
        {
            TimeNumberList newList = new TimeNumberList();
            foreach (var value in first)
            {
                newList.SetData(value.date, value.value);
            }
            Assert.AreEqual(count, newList.Count());
        }
        private static IEnumerable<TestCaseData> AnyTestSource()
        {
            var tests = TestLists();
            foreach (var (name, testList, _, any, _) in tests)
            {
                yield return new TestCaseData(any, testList).SetName($"Any-{name}");
            }
        }

        [TestCaseSource(nameof(AnyTestSource))]
        public void AnyTests(bool result, TimeNumberList listToTest)
        {
            Assert.AreEqual(result, listToTest.Any());
        }

        private static IEnumerable<TestCaseData> CleanValuesTestSource()
        {
            yield return new TestCaseData(new TimeNumberList(), new TimeNumberList())
                .SetName($"{nameof(CleanValuesTests)}-EmptyTimelist");
            yield return new TestCaseData(
                new TimeNumberList(new List<DailyNumeric>() { new DailyNumeric(new DateTime(2018, 1, 1), 0.0) }),
                new TimeNumberList(new List<DailyNumeric>()
                {
                    new DailyNumeric(new DateTime(2018, 1, 1), 0.0),
                    new DailyNumeric(new DateTime(2019, 1, 1), 0.0)
                })).SetName($"{nameof(CleanValuesTests)}-TwoIdenticalEntryList");
            yield return new TestCaseData(
                new TimeNumberList(new List<DailyNumeric>() { new DailyNumeric(new DateTime(2018, 1, 1), 0.0) }),
                new TimeNumberList(new List<DailyNumeric>()
                {
                    new DailyNumeric(new DateTime(2018, 1, 1), 0.0),
                    new DailyNumeric(new DateTime(2019, 1, 1), 0.0),
                    new DailyNumeric(new DateTime(2019, 5, 1), 0.0),
                    new DailyNumeric(new DateTime(2019, 5, 5), 0.0)
                })).SetName($"{nameof(CleanValuesTests)}-ManyEntrylist");
            yield return new TestCaseData(
                new TimeNumberList(new List<DailyNumeric>() { new DailyNumeric(new DateTime(2018, 1, 1), 0.0), new DailyNumeric(new DateTime(2019, 5, 1), 2.0) }),
                new TimeNumberList(new List<DailyNumeric>()
                {
                    new DailyNumeric(new DateTime(2018, 1, 1), 0.0),
                    new DailyNumeric(new DateTime(2019, 1, 1), 0.0),
                    new DailyNumeric(new DateTime(2019, 5, 1), 2.0),
                    new DailyNumeric(new DateTime(2019, 5, 5), 2.0)
                })).SetName($"{nameof(CleanValuesTests)}-ManyEntryList2");
            yield return new TestCaseData(
                new TimeNumberList(new List<DailyNumeric>() { new DailyNumeric(new DateTime(2018, 1, 1), 0.0), new DailyNumeric(new DateTime(2019, 1, 1), 1.0), new DailyNumeric(new DateTime(2019, 5, 1), 2.0), new DailyNumeric(new DateTime(2019, 5, 5), 0.0) }),
                new TimeNumberList(new List<DailyNumeric>()
                {
                    new DailyNumeric(new DateTime(2018, 1, 1), 0.0),
                    new DailyNumeric(new DateTime(2019, 1, 1), 1.0),
                    new DailyNumeric(new DateTime(2019, 5, 1), 2.0),
                    new DailyNumeric(new DateTime(2019, 5, 5), 0.0)
                })).SetName($"{nameof(CleanValuesTests)}-ManyEntryList3");
        }

        [TestCaseSource(nameof(CleanValuesTestSource))]
        public void CleanValuesTests(TimeNumberList expectedCleaned, TimeNumberList timelist)
        {
            timelist.CleanValues();
            int count = expectedCleaned.Count();
            Assert.AreEqual(count, timelist.Count());
            Assert.AreEqual(timelist, expectedCleaned);
        }
        private static IEnumerable<TestCaseData> ValuesTestSource()
        {
            foreach (var value in ValuesTestSourceData())
            {
                yield return new TestCaseData(value.Item2, value.Item3, value.Item4, value.Item5).SetName($"{nameof(ValuesTests)}-{value.Name}");
            }
        }

        [TestCaseSource(nameof(ValuesTestSource))]
        public void ValuesTests(double? expectedResult, DateTime expectedDate, DateTime date, params (DateTime date, double value)[] first)
        {
            var timelist = new TimeNumberList();
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

        [TestCaseSource(nameof(ValuesSpecialFuncTestSource))]
        public void ValuesSpecialFuncTests(double? expectedResult, DateTime expectedDate, DateTime date, (DateTime date, double value)[] first)
        {
            var timelist = new TimeNumberList();
            foreach (var value in first)
            {
                timelist.SetData(value.date, value.value);
            }

            double interpolator(DailyNumeric earlier, DailyNumeric later, DateTime chosenDate) => earlier.Value + (later.Value - earlier.Value) / (later.Day - earlier.Day).Days * (chosenDate - earlier.Day).Days;
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
        private static IEnumerable<TestCaseData> ValuesSpecialFuncTestSource()
        {
            foreach (var value in ValuesTestSourceData())
            {
                yield return new TestCaseData(value.Item2, value.Item3, value.Item4, value.Item5).SetName($"{nameof(ValuesSpecialFuncTests)}-{value.Name}");
            }
        }

        private static IEnumerable<(string Name, double?, DateTime, DateTime, (DateTime, double)[])> ValuesTestSourceData()
        {
            yield return ("EmptyTimeList",
                null,
                new DateTime(2018, 1, 1),
                new DateTime(2018, 1, 1),
                Array.Empty<(DateTime, double)>());
            yield return ("TwoEntryZeroValues",
                0.0,
                new DateTime(2018, 1, 1),
                new DateTime(2018, 1, 1),
                new[] { (new DateTime(2018, 1, 1), 0.0), (new DateTime(2019, 1, 1), 0.0) });
            yield return ("FourEntryZeroValuesDifferentDate",
                0.0,
                new DateTime(2018, 1, 1),
                new DateTime(2017, 1, 1),
                new[] { (new DateTime(2018, 1, 1), 0.0), (new DateTime(2019, 1, 1), 0.0), (new DateTime(2019, 5, 1), 0.0), (new DateTime(2019, 5, 5), 0.0) });
            yield return ("FourEntryZeroValuesSameDate",
                0.0,
                new DateTime(2018, 1, 1),
                new DateTime(2018, 1, 1),
                new[] { (new DateTime(2018, 1, 1), 0.0), (new DateTime(2019, 1, 1), 0.0), (new DateTime(2019, 5, 1), 0.0), (new DateTime(2019, 5, 5), 0.0) });
            yield return ("FourEntryZeroValuesTest3",
                0.0,
                new DateTime(2019, 5, 5),
                new DateTime(2020, 1, 1),
                new[] { (new DateTime(2018, 1, 1), 0.0), (new DateTime(2019, 1, 1), 0.0), (new DateTime(2019, 5, 1), 0.0), (new DateTime(2019, 5, 5), 0.0) });
            yield return ("FourEntryValues",
                0.0,
                new DateTime(2018, 1, 1),
                new DateTime(2018, 1, 1),
                new[] { (new DateTime(2018, 1, 1), 0.0), (new DateTime(2019, 1, 1), 0.0), (new DateTime(2019, 5, 1), 2.0), (new DateTime(2019, 5, 5), 2.0) });
            yield return ("FourEntryValuesSecondTest",
                1.05,
                new DateTime(2019, 3, 5),
                new DateTime(2019, 3, 5),
                new[] { (new DateTime(2018, 1, 1), 0.0), (new DateTime(2019, 1, 1), 0.0), (new DateTime(2019, 5, 1), 2.0), (new DateTime(2019, 5, 5), 2.0) });
            yield return ("FourEntryValues2",
                2.0,
                new DateTime(2019, 5, 1),
                new DateTime(2019, 5, 1),
                new[] { (new DateTime(2018, 1, 1), 0.0), (new DateTime(2019, 1, 1), 0.0), (new DateTime(2019, 5, 1), 2.0), (new DateTime(2019, 5, 5), 2.0) });
            yield return ("FourEntryValues2SecondTest",
                0.0,
                new DateTime(2018, 1, 1),
                new DateTime(2018, 1, 1),
                new[] { (new DateTime(2018, 1, 1), 0.0), (new DateTime(2019, 1, 1), 1.0), (new DateTime(2019, 5, 1), 2.0), (new DateTime(2019, 5, 5), 0.0) });
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
        public void WriteXmlTests(string expectedXml, TimeNumberList times)
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
        }

        [TestCaseSource(nameof(ReadSerializationData), new object[] { nameof(ReadXmlTests) })]
        public void ReadXmlTests(string expectedXml, TimeNumberList timelist)
        {
            var valuation = new TimeNumberList();
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
        public void XmlRoundTripTests(string expectedXml, TimeNumberList timelist)
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

            var valuation = new TimeNumberList();
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
