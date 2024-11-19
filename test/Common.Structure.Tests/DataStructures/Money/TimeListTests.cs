using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

using Effanville.Common.Structure.DataStructures;

using NUnit.Framework;

namespace Effanville.Common.Structure.Tests.DataStructures.Money
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
            Assert.That(newList.Count(), Is.EqualTo(count));
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
            Assert.That(listToTest.Any(), Is.EqualTo(result));
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
            Assert.That(timelist.Count(), Is.EqualTo(count));
            Assert.That(expectedCleaned, Is.EqualTo(timelist));
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

                Assert.That(output, Is.EqualTo(expectedXml));
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

                Assert.That(valuation, Is.EqualTo(timelist));
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

            Assert.That(output, Is.EqualTo(expectedXml));

            var valuation = new TimeList();
            using (StringReader fs = new StringReader(output))
            {
                XmlReaderSettings xmlWriterSettings = new XmlReaderSettings();
                using (XmlReader reader = XmlReader.Create(fs, xmlWriterSettings))
                {
                    valuation.ReadXml(reader);
                }

                Assert.That(timelist, Is.EqualTo(valuation));
            }
        }
    }
}
