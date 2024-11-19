using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

using Effanville.Common.Structure.DataStructures.Numeric;

using NUnit.Framework;

namespace Effanville.Common.Structure.Tests.DataStructures.Numeric
{
    public sealed class DailyNumericTests
    {
        private static string enl = TestConstants.EnvNewLine;

        [TestCase("1/1/2018", 1, "1/1/2019", 0.0, -1)]
        [TestCase("1/1/2020", 1, "1/1/2019", 0.0, 1)]
        [TestCase("1/1/2018", 1, "1/1/2018", 0.0, 0)]
        public void ComparisonTests(DateTime firstDate, double firstValue, DateTime secondDate, double secondValue, int result)
        {
            DailyNumeric first = new DailyNumeric(firstDate, firstValue);

            DailyNumeric second = new DailyNumeric(secondDate, secondValue);
            int comparison = first.CompareTo(second);

            Assert.That(comparison, Is.EqualTo(result));
        }

        [Test]
        public void CreateNewDeepCopy()
        {
            var first = new DailyNumeric(new DateTime(2018, 1, 1), 4);
            var second = new DailyNumeric(first);

            second.Day = new DateTime(2014, 1, 1);
            second.Value = 6;
            Assert.That(second.Day, Is.Not.EqualTo(first.Day));
            Assert.That(second.Value, Is.Not.EqualTo(first.Value));
        }

        [TestCase("1/1/2018", 1)]
        public void CopyTests(DateTime date, double value)
        {
            DailyNumeric data = new DailyNumeric(date, value);

            DailyNumeric newData = data.Copy();

            newData.Day = (DateTime.Parse("1/1/2019"));
            Assert.That(newData, Is.Not.EqualTo(data));
        }

        private static IEnumerable<TestCaseData> WriteXmlTestCases()
        {
            yield return new TestCaseData(
                new DateTime(2018, 1, 31),
                278.671,
                $"<?xml version=\"1.0\" encoding=\"utf-16\"?>{enl}<DV D=\"2018-01-31T00:00:00\" V=\"278.671\" />");
            yield return new TestCaseData(
                new DateTime(2018, 1, 31),
                0.0,
                $"<?xml version=\"1.0\" encoding=\"utf-16\"?>{enl}<DV D=\"2018-01-31T00:00:00\" V=\"0\" />");
            yield return new TestCaseData(
                new DateTime(2018, 1, 31),
                double.NaN,
                $"<?xml version=\"1.0\" encoding=\"utf-16\"?>{enl}<DV D=\"2018-01-31T00:00:00\" V=\"NaN\" />");
            yield return new TestCaseData(
                new DateTime(2018, 1, 31),
                -2345.67865,
                $"<?xml version=\"1.0\" encoding=\"utf-16\"?>{enl}<DV D=\"2018-01-31T00:00:00\" V=\"-2345.67865\" />");
            yield return new TestCaseData(
                new DateTime(2018, 1, 31),
                double.NegativeInfinity,
                $"<?xml version=\"1.0\" encoding=\"utf-16\"?>{enl}<DV D=\"2018-01-31T00:00:00\" V=\"-Infinity\" />");
            yield return new TestCaseData(
                null,
                null,
                $"<?xml version=\"1.0\" encoding=\"utf-16\"?>{enl}<DV D=\"0001-01-01T00:00:00\" V=\"0\" />");
        }

        [TestCaseSource(nameof(WriteXmlTestCases))]
        public void WriteXmlTests(DateTime day, double value, string expectedXml)
        {
            var val = new DailyNumeric(day, value);
            using (StringWriter fs = new StringWriter())
            {
                XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
                xmlWriterSettings.NewLineOnAttributes = false;
                xmlWriterSettings.Encoding = Encoding.UTF8;
                xmlWriterSettings.Indent = true;
                using (XmlWriter writer = XmlWriter.Create(fs, xmlWriterSettings))
                {
                    val.WriteXml(writer);
                }

                string output = fs.ToString();

                Assert.That(output, Is.EqualTo(expectedXml));
            }
        }

        private static IEnumerable<TestCaseData> ReadXmlTestsCases()
        {
            yield return new TestCaseData(
                new DateTime(2018, 1, 31),
                278.671,
                $"<?xml version=\"1.0\" encoding=\"utf-16\"?>{enl}<DV D=\"2018-01-31T00:00:00\" V=\"278.671\" />");
            yield return new TestCaseData(
                new DateTime(2018, 1, 31),
                0.0,
                $"<?xml version=\"1.0\" encoding=\"utf-16\"?>{enl}<DV D=\"2018-01-31T00:00:00\" V=\"0\" />");
            yield return new TestCaseData(
                null,
                null,
                $"<?xml version=\"1.0\" encoding=\"utf-16\"?>{enl}<DV D=\"0001-01-01T00:00:00\" V=\"0\" />");
            yield return new TestCaseData(
                new DateTime(2018, 1, 31),
                double.NaN,
                $"<?xml version=\"1.0\" encoding=\"utf-16\"?>{enl}<DV D=\"2018-01-31T00:00:00\" V=\"NaN\" />");
            yield return new TestCaseData(
                new DateTime(2018, 1, 31),
                -2345.67865,
                $"<?xml version=\"1.0\" encoding=\"utf-16\"?>{enl}<DV D=\"2018-01-31T00:00:00\" V=\"-2345.67865\" />");
            yield return new TestCaseData(
                new DateTime(2018, 1, 31),
                double.NegativeInfinity,
                $"<?xml version=\"1.0\" encoding=\"utf-16\"?>{enl}<DV D=\"2018-01-31T00:00:00\" V=\"-Infinity\" />");
        }

        [TestCaseSource(nameof(ReadXmlTestsCases))]
        public void ReadXmlTests(DateTime day, double value, string actualXml)
        {
            var valuation = new DailyNumeric();
            using (StringReader fs = new StringReader(actualXml))
            {
                XmlReaderSettings xmlWriterSettings = new XmlReaderSettings();
                using (XmlReader reader = XmlReader.Create(fs, xmlWriterSettings))
                {
                    valuation.ReadXml(reader);
                }

                Assert.That(valuation, Is.EqualTo(new DailyNumeric(day, value)));
            }
        }

        [TestCase("2018/1/31", 278.671)]
        [TestCase("2018/1/31", double.NaN)]
        [TestCase("2018/1/31", -2345.67865)]
        [TestCase("2018/1/31", double.NegativeInfinity)]
        [TestCase("2018/1/31", 0.0)]
        [TestCase("1/1/1", 0.0)]
        public void XmlRoundTripTests(DateTime day, double value)
        {
            string output;
            var val = new DailyNumeric(day, value);
            using (StringWriter fs = new StringWriter())
            {
                XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
                xmlWriterSettings.NewLineOnAttributes = false;
                xmlWriterSettings.Encoding = Encoding.UTF8;
                xmlWriterSettings.Indent = true;
                using (XmlWriter writer = XmlWriter.Create(fs, xmlWriterSettings))
                {
                    val.WriteXml(writer);
                }

                output = fs.ToString();
            }

            var valuation = new DailyNumeric();
            using (StringReader fs = new StringReader(output))
            {
                XmlReaderSettings xmlWriterSettings = new XmlReaderSettings();
                using (XmlReader reader = XmlReader.Create(fs, xmlWriterSettings))
                {
                    valuation.ReadXml(reader);
                }

                Assert.That(val, Is.EqualTo(valuation));
            }
        }
    }
}
