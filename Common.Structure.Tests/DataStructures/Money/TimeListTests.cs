﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using NUnit.Framework;
using Common.Structure.DataStructures;

namespace Common.Structure.Tests.DataStructures.Money
{
    [TestFixture]
    public sealed class TimeListTests
    {
        private static string enl = TestConstants.EnvNewLine;

        private static IEnumerable<(string name, TimeList testList, int count, bool any, string XmlString)> TestLists()
        {
            yield return (
                "empty",
                TimeListTestData.GetTestTimeList(TimeListTestData.EmptyListKey),
                0,
                false,
                $"<?xml version=\"1.0\" encoding=\"utf-16\"?>{enl}<Values />");
            yield return (
                "single",
                TimeListTestData.GetTestTimeList(TimeListTestData.SingleEntryKey),
                1,
                true,
                $"<?xml version=\"1.0\" encoding=\"utf-16\"?>{enl}<Values>{enl}  <DV D=\"2018-01-01T00:00:00\" V=\"1000\" />{enl}</Values>");
            yield return (
                "repeatedValue",
                new TimeList(new List<DailyValuation>() { new DailyValuation(new DateTime(2018, 1, 1), 0.0m), new DailyValuation(new DateTime(2018, 1, 1), 0.0m) }),
                1,
                true,
                $"<?xml version=\"1.0\" encoding=\"utf-16\"?>{enl}<Values>{enl}  <DV D=\"2018-01-01T00:00:00\" V=\"0.0\" />{enl}  <DV D=\"2018-01-01T00:00:00\" V=\"0.0\" />{enl}</Values>");
            yield return (
                "standardList",
                new TimeList(new List<DailyValuation>()
                {
                    new DailyValuation(new DateTime(2018, 1, 1), 0.0m),
                    new DailyValuation(new DateTime(2019, 1, 1), 1.0m),
                    new DailyValuation(new DateTime(2019, 5, 1), 2.0m),
                    new DailyValuation(new DateTime(2019, 5, 5), 0.0m)
                }),
                4,
                true,
                $"<?xml version=\"1.0\" encoding=\"utf-16\"?>{enl}<Values>{enl}  <DV D=\"2018-01-01T00:00:00\" V=\"0.0\" />{enl}  <DV D=\"2019-01-01T00:00:00\" V=\"1.0\" />{enl}  <DV D=\"2019-05-01T00:00:00\" V=\"2.0\" />{enl}  <DV D=\"2019-05-05T00:00:00\" V=\"0.0\" />{enl}</Values>");
        }

        private static IEnumerable<TestCaseData> TryAddValueTestSource()
        {
            yield return new TestCaseData(0, Array.Empty<(DateTime, decimal)>()).SetName($"{nameof(SetDataTests)}-EmptyTimeList");
            yield return new TestCaseData(1, new[] { (new DateTime(2018, 1, 1), 0.0m), (new DateTime(2018, 1, 1), 0.0m) }).SetName($"{nameof(SetDataTests)}-AddSameValue");
            yield return new TestCaseData(2, new[] { (new DateTime(2018, 1, 1), 0.0m), (new DateTime(2019, 1, 1), 0.0m) }).SetName($"{nameof(SetDataTests)}-AddDifferentValue");
        }

        [TestCaseSource(nameof(TryAddValueTestSource))]
        public void SetDataTests(int count, params (DateTime date, decimal value)[] first)
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
            foreach (var (name, testList, _, any, _) in tests)
            {
                yield return new TestCaseData(any, testList).SetName($"Any-{name}");
            }
        }

        [TestCaseSource(nameof(AnyTestSource))]
        public void AnyTests(bool result, TimeList listToTest)
        {
            Assert.AreEqual(result, listToTest.Any());
        }

        private static IEnumerable<TestCaseData> CleanValuesTestSource()
        {
            yield return new TestCaseData(new TimeList(), new TimeList()).SetName($"{nameof(CleanValuesTests)}-EmptyTimelist");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.SingleEntryZeroValueKey),
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryZeroValuesKey)).SetName($"{nameof(CleanValuesTests)}-TwoEntryTimelist");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.SingleEntryZeroValueKey),
                TimeListTestData.GetTestTimeList(TimeListTestData.FourEntryZeroValuesKey)).SetName($"{nameof(CleanValuesTests)}-FourEntrylist");
            yield return new TestCaseData(
                new TimeList(new List<DailyValuation>() { new DailyValuation(new DateTime(2018, 1, 1), 0.0m), new DailyValuation(new DateTime(2019, 5, 1), 2.0m) }),
                new TimeList(new List<DailyValuation>() { new DailyValuation(new DateTime(2018, 1, 1), 0.0m), new DailyValuation(new DateTime(2019, 1, 1), 0.0m), new DailyValuation(new DateTime(2019, 5, 1), 2.0m), new DailyValuation(new DateTime(2019, 5, 5), 2.0m) })).SetName($"{nameof(CleanValuesTests)}-MultipleTimelist");
            yield return new TestCaseData(
                new TimeList(new List<DailyValuation>() { new DailyValuation(new DateTime(2018, 1, 1), 0.0m), new DailyValuation(new DateTime(2019, 1, 1), 1.0m), new DailyValuation(new DateTime(2019, 5, 1), 2.0m), new DailyValuation(new DateTime(2019, 5, 5), 0.0m) }),
                new TimeList(new List<DailyValuation>() { new DailyValuation(new DateTime(2018, 1, 1), 0.0m), new DailyValuation(new DateTime(2019, 1, 1), 1.0m), new DailyValuation(new DateTime(2019, 5, 1), 2.0m), new DailyValuation(new DateTime(2019, 5, 5), 0.0m) })).SetName($"{nameof(CleanValuesTests)}-MultipleTimelist2");
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
        public void ValuesTests(decimal? expectedResult, DateTime expectedDate, DateTime date, TimeList timelist)
        {
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

        private static IEnumerable<TestCaseData> ValuesTestSource()
        {
            foreach (var value in ValuesTestSourceData())
            {
                yield return new TestCaseData(value.Item2, value.Item3, value.Item4, value.Item5).SetName($"{nameof(ValuesTests)}-{value.Name}");
            }
        }

        [TestCaseSource(nameof(ValuesSpecialFuncTestSource))]
        public void ValuesSpecialFuncTests(decimal? expectedResult, DateTime expectedDate, DateTime date, TimeList timelist)
        {
            decimal interpolator(DailyValuation earlier, DailyValuation later, DateTime chosenDate) => earlier.Value + (later.Value - earlier.Value) / (later.Day - earlier.Day).Days * (chosenDate - earlier.Day).Days;
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

        private static IEnumerable<(string Name, decimal?, DateTime, DateTime, TimeList)> ValuesTestSourceData()
        {
            yield return ("EmptyTimeList",
                null,
                new DateTime(2018, 1, 1),
                new DateTime(2018, 1, 1),
                TimeListTestData.GetTestTimeList(TimeListTestData.EmptyListKey));
            yield return ("TwoEntryZeroValues",
                0.0m,
                new DateTime(2018, 1, 1),
                new DateTime(2018, 1, 1),
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryZeroValuesKey));
            yield return ("FourEntryZeroValuesDifferentDate",
                0.0m,
                new DateTime(2018, 1, 1),
                new DateTime(2017, 1, 1),
                TimeListTestData.GetTestTimeList(TimeListTestData.FourEntryZeroValuesKey));
            yield return ("FourEntryZeroValuesSameDate",
                0.0m,
                new DateTime(2018, 1, 1),
                new DateTime(2018, 1, 1),
                TimeListTestData.GetTestTimeList(TimeListTestData.FourEntryZeroValuesKey));
            yield return ("FourEntryZeroValuesTest3",
                0.0m,
                new DateTime(2019, 5, 5),
                new DateTime(2020, 1, 1),
                TimeListTestData.GetTestTimeList(TimeListTestData.FourEntryZeroValuesKey));
            yield return ("FourEntryValues",
                0.0m,
                new DateTime(2018, 1, 1),
                new DateTime(2018, 1, 1),
                TimeListTestData.GetTestTimeList(TimeListTestData.FourEntryKey));
            yield return ("FourEntryValuesSecondTest",
                1.0500000000000000000000000021m,
                new DateTime(2019, 3, 5),
                new DateTime(2019, 3, 5),
                TimeListTestData.GetTestTimeList(TimeListTestData.FourEntryKey));
            yield return ("FourEntryValues2",
                2.0m,
                new DateTime(2019, 5, 1),
                new DateTime(2019, 5, 1),
                TimeListTestData.GetTestTimeList(TimeListTestData.FourEntryKey));
            yield return ("FourEntryValues2SecondTest",
                0.0m,
                new DateTime(2018, 1, 1),
                new DateTime(2018, 1, 1),
                TimeListTestData.GetTestTimeList(TimeListTestData.FourEntryKey2));
        }

        private static IEnumerable<TestCaseData> WriteSerializationData(string testName)
        {
            var tests = TestLists();
            foreach (var (name, testList, _, _, XmlString) in tests)
            {
                yield return new TestCaseData(XmlString, testList).SetName($"{testName}-{name}");
            }
        }

        [TestCaseSource(nameof(WriteSerializationData), new object[] { nameof(WriteXmlTests) })]
        public void WriteXmlTests(string expectedXml, TimeList times)
        {
            using (StringWriter fs = new StringWriter())
            {
                XmlWriterSettings xmlWriterSettings = new XmlWriterSettings
                {
                    NewLineOnAttributes = false,
                    Encoding = Encoding.UTF8,
                    Indent = true
                };
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
            foreach (var (name, testList, _, _, XmlString) in tests)
            {
                yield return new TestCaseData(XmlString, testList).SetName($"{testName}-{name}");
            }

            yield return new TestCaseData(
                $"<?xml version=\"1.0\" encoding=\"utf-16\"?>{enl}<Values>{enl}<DailyValuation>{enl}<Day>2018-01-01T00:00:00</Day>{enl}<Value>0</Value>{enl}</DailyValuation>{enl}<DailyValuation>{enl}<Day>2019-01-01T00:00:00</Day>{enl}<Value>1.0</Value>{enl}</DailyValuation>{enl}<DailyValuation>{enl}<Day>2019-05-01T00:00:00</Day>{enl}<Value>2.0</Value>{enl}</DailyValuation>{enl}<DailyValuation>{enl}<Day>2019-05-05T00:00:00</Day>{enl}<Value>0</Value>{enl}</DailyValuation>{enl}</Values>",
                new TimeList(
                    new List<DailyValuation>()
                    {
                        new DailyValuation(new DateTime(2018, 1, 1), 0.0m),
                        new DailyValuation(new DateTime(2019, 1, 1), 1.0m),
                        new DailyValuation(new DateTime(2019, 5, 1), 2.0m),
                        new DailyValuation(new DateTime(2019, 5, 5), 0.0m)
                    }))
                .SetName($"{testName}-old1");
            yield return new TestCaseData(
                $"<?xml version=\"1.0\" encoding=\"utf-16\"?>{enl}<Values>{enl}<DailyValuation>{enl}<Day>2018-01-01T00:00:00</Day>{enl}<Value>0</Value>{enl}</DailyValuation>{enl}</Values>",
                new TimeList(new List<DailyValuation>() { new DailyValuation(new DateTime(2018, 1, 1), 0.0m) }))
                .SetName($"{testName}-old2");
            yield return new TestCaseData($"<?xml version=\"1.0\" encoding=\"utf-16\"?>{enl}<Values />", new TimeList(new List<DailyValuation>()))
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
                XmlWriterSettings xmlWriterSettings = new XmlWriterSettings
                {
                    NewLineOnAttributes = false,
                    Encoding = Encoding.UTF8,
                    Indent = true
                };
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
