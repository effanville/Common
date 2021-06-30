using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using NUnit.Framework;
using Common.Structure.DataStructures;

namespace Common.Structure.Tests.DataStructures
{
    public class TimeList_Tests
    {
        [TestCaseSource(nameof(TryAddValueTestSource))]
        public void TryAddValueTests(int count, params (DateTime date, double value)[] first)
        {
            TimeList newList = new TimeList();
            foreach (var value in first)
            {
                _ = newList.TryAddValue(value.date, value.value);
            }
            Assert.AreEqual(count, newList.Count());
        }

        private static IEnumerable<TestCaseData> TryAddValueTestSource()
        {
            yield return new TestCaseData(0, Array.Empty<(DateTime, double)>());
            yield return new TestCaseData(1, new[] { (new DateTime(2018, 1, 1), 0.0), (new DateTime(2018, 1, 1), 0.0) });
            yield return new TestCaseData(2, new[] { (new DateTime(2018, 1, 1), 0.0), (new DateTime(2019, 1, 1), 0.0) });
        }

        [TestCaseSource(nameof(AnyTestSource))]
        public void AnyTests(bool result, params (DateTime date, double value)[] first)
        {
            TimeList newList = new TimeList();
            foreach (var value in first)
            {
                _ = newList.TryAddValue(value.date, value.value);
            }
            Assert.AreEqual(result, newList.Any());
        }

        private static IEnumerable<TestCaseData> AnyTestSource()
        {
            yield return new TestCaseData(false, Array.Empty<(DateTime, double)>());
            yield return new TestCaseData(true, new[] { (new DateTime(2018, 1, 1), 0.0), (new DateTime(2018, 1, 1), 0.0) });
        }

        [TestCaseSource(nameof(CleanValuesTestSource))]
        public void CleanValuesTests((DateTime date, double value)[] result, params (DateTime date, double value)[] first)
        {
            var timelist = new TimeList();
            foreach (var value in first)
            {
                _ = timelist.TryAddValue(value.date, value.value);
            }

            timelist.CleanValues();

            Assert.AreEqual(result.Length, timelist.Count());
            for (int i = 0; i < result.Length; ++i)
            {
                Assert.AreEqual(result[i].date, timelist.Values[i].Day, $"Index {i} date not correct");
                Assert.AreEqual(result[i].value, timelist.Values[i].Value, $"Index {i} value not correct");
            }
        }

        private static IEnumerable<TestCaseData> CleanValuesTestSource()
        {
            yield return new TestCaseData(Array.Empty<(DateTime, double)>(), Array.Empty<(DateTime, double)>());
            yield return new TestCaseData(new[] { (new DateTime(2018, 1, 1), 0.0) }, new[] { (new DateTime(2018, 1, 1), 0.0), (new DateTime(2019, 1, 1), 0.0) });
            yield return new TestCaseData(new[] { (new DateTime(2018, 1, 1), 0.0) }, new[] { (new DateTime(2018, 1, 1), 0.0), (new DateTime(2019, 1, 1), 0.0), (new DateTime(2019, 5, 1), 0.0), (new DateTime(2019, 5, 5), 0.0) });
            yield return new TestCaseData(new[] { (new DateTime(2018, 1, 1), 0.0), (new DateTime(2019, 5, 1), 2.0) }, new[] { (new DateTime(2018, 1, 1), 0.0), (new DateTime(2019, 1, 1), 0.0), (new DateTime(2019, 5, 1), 2.0), (new DateTime(2019, 5, 5), 2.0) });
            yield return new TestCaseData(new[] { (new DateTime(2018, 1, 1), 0.0), (new DateTime(2019, 1, 1), 1.0), (new DateTime(2019, 5, 1), 2.0), (new DateTime(2019, 5, 5), 0.0) }, new[] { (new DateTime(2018, 1, 1), 0.0), (new DateTime(2019, 1, 1), 1.0), (new DateTime(2019, 5, 1), 2.0), (new DateTime(2019, 5, 5), 0.0) });
        }

        [TestCaseSource(nameof(ValuesTestSource))]
        public void ValuesTests(double? expectedResult, DateTime expectedDate, DateTime date, params (DateTime date, double value)[] first)
        {
            var timelist = new TimeList();
            foreach (var value in first)
            {
                _ = timelist.TryAddValue(value.date, value.value);
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
                _ = timelist.TryAddValue(value.date, value.value);
            }

            Func<DailyValuation, DailyValuation, DateTime, double> interpolator = (earlier, later, chosenDate) => earlier.Value + (later.Value - earlier.Value) / (later.Day - earlier.Day).Days * (chosenDate - earlier.Day).Days;
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
            yield return new TestCaseData("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<Values>\r\n  <DV D=\"2018-01-01T00:00:00\" V=\"0\" />\r\n  <DV D=\"2019-01-01T00:00:00\" V=\"1\" />\r\n  <DV D=\"2019-05-01T00:00:00\" V=\"2\" />\r\n  <DV D=\"2019-05-05T00:00:00\" V=\"0\" />\r\n</Values>", new[] { (new DateTime(2018, 1, 1), 0.0), (new DateTime(2019, 1, 1), 1.0), (new DateTime(2019, 5, 1), 2.0), (new DateTime(2019, 5, 5), 0.0) })
                .SetName($"{testName}-new1");
                            yield return new TestCaseData("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<Values>\r\n  <DV D=\"2018-01-01T00:00:00\" V=\"0\" />\r\n  <DV D=\"2019-01-01T00:00:00\" V=\"1\" />\r\n  <DV D=\"2019-05-01T00:00:00\" V=\"2\" />\r\n  <DV D=\"2019-05-05T00:00:00\" V=\"0\" />\r\n</Values>", new[] { (new DateTime(2018, 1, 1), 0.0), (new DateTime(2019, 1, 1), 1.0), (new DateTime(2019, 5, 1), 2.0), (new DateTime(2019, 5, 5), 0.0) })
                .SetName($"{testName}-new1");
        }

        [TestCaseSource(nameof(WriteSerializationData), new object[] { nameof(WriteXmlTests) })]
        public void WriteXmlTests(string expectedXml, (DateTime date, double value)[] times)
        {
            var timelist = new TimeList();
            foreach (var val in times)
            {
                _ = timelist.TryAddValue(val.date, val.value);
            }
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

                string output = fs.ToString();

                Assert.AreEqual(expectedXml, output);
            }
        }

        private static IEnumerable<TestCaseData> ReadSerializationData(string testName)
        {
            yield return new TestCaseData("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<Values>\r\n  <DV D=\"2018-01-01T00:00:00\" V=\"0\" />\r\n  <DV D=\"2019-01-01T00:00:00\" V=\"1\" />\r\n  <DV D=\"2019-05-01T00:00:00\" V=\"2\" />\r\n  <DV D=\"2019-05-05T00:00:00\" V=\"0\" />\r\n</Values>", new[] { (new DateTime(2018, 1, 1), 0.0), (new DateTime(2019, 1, 1), 1.0), (new DateTime(2019, 5, 1), 2.0), (new DateTime(2019, 5, 5), 0.0) })
                .SetName($"{testName}-new1");
            yield return new TestCaseData("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<Values>\r\n  <DV D=\"2018-01-01T00:00:00\" V=\"0\" />\r\n  <DV D=\"2019-01-01T00:00:00\" V=\"1\" />\r\n  <DV D=\"2019-05-01T00:00:00\" V=\"2\" />\r\n  <DV D=\"2019-05-05T00:00:00\" V=\"0\" />\r\n</Values>", new[] { (new DateTime(2018, 1, 1), 0.0), (new DateTime(2019, 1, 1), 1.0), (new DateTime(2019, 5, 1), 2.0), (new DateTime(2019, 5, 5), 0.0) })
                .SetName($"{testName}-old1");
            yield return new TestCaseData("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<Values>\r\n  <DailyValuation>\r\n    <Day>2018-01-01T00:00:00</Day>\r\n    <Value>0</Value>\r\n</Values>", new[] { (new DateTime(2018, 1, 1), 0.0) })
                .SetName($"{testName}-old2");
        }

        [TestCaseSource(nameof(ReadSerializationData), new object[] { nameof(ReadXmlTests)})]
        public void ReadXmlTests(string expectedXml, (DateTime date, double value)[] first)
        {
            var timelist = new TimeList();
            foreach (var val in first)
            {
                _ = timelist.TryAddValue(val.date, val.value);
            }
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

        [TestCaseSource(nameof(WriteSerializationData), new object[] { nameof(XmlRoundTripTests)})]
        public void XmlRoundTripTests(string expectedXml, (DateTime date, double value)[] first)
        {
            var timelist = new TimeList();
            foreach (var vall in first)
            {
                _ = timelist.TryAddValue(vall.date, vall.value);
            }
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
