﻿using System;
using System.IO;
using System.Text;
using System.Xml;
using NUnit.Framework;
using StructureCommon.DataStructures;

namespace StructureCommon.Tests.DataStructures
{
    public class DailyValuationTests
    {
        [TestCase("1/1/2018", 1, "1/1/2019", 0.0, -1)]
        [TestCase("1/1/2020", 1, "1/1/2019", 0.0, 1)]
        [TestCase("1/1/2018", 1, "1/1/2018", 0.0, 0)]
        public void ComparisonTests(DateTime firstDate, double firstValue, DateTime secondDate, double secondValue, int result)
        {
            DailyValuation first = new DailyValuation(firstDate, firstValue);

            DailyValuation second = new DailyValuation(secondDate, secondValue);
            int comparison = first.CompareTo(second);

            Assert.AreEqual(result, comparison);
        }

        [TestCase("1/1/2018", 1)]
        public void CopyTests(DateTime date, double value)
        {
            DailyValuation data = new DailyValuation(date, value);

            DailyValuation newData = data.Copy();

            newData.SetDay(DateTime.Parse("1/1/2019"));
            Assert.AreNotEqual(data, newData);
        }

        [TestCase("2018/1/31", 278.671, "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<DailyValuation>\r\n  <Day>2018-01-31T00:00:00</Day>\r\n  <Value>278.671</Value>\r\n</DailyValuation>")]
        [TestCase("2018/1/31", 0.0, "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<DailyValuation>\r\n  <Day>2018-01-31T00:00:00</Day>\r\n  <Value>0</Value>\r\n</DailyValuation>")]
        [TestCase(null, null, "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<DailyValuation>\r\n  <Day>0001-01-01T00:00:00</Day>\r\n  <Value>0</Value>\r\n</DailyValuation>")]
        public void WriteXmlOldTests(DateTime day, double value, string expectedXml)
        {
            var val = new DailyValuation(day, value);
            using (StringWriter fs = new StringWriter())
            {
                XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
                xmlWriterSettings.NewLineOnAttributes = false;
                xmlWriterSettings.Encoding = Encoding.UTF8;
                xmlWriterSettings.Indent = true;
                using (XmlWriter writer = XmlWriter.Create(fs, xmlWriterSettings))
                {
                    val.WriteXmlOld(writer);
                }

                string output = fs.ToString();

                Assert.AreEqual(expectedXml, output);
            }
        }

        [TestCase("2018/1/31", 278.671, "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<DV D=\"2018-01-31T00:00:00\" V=\"278.671\" />")]
        [TestCase("2018/1/31", 0.0, "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<DV D=\"2018-01-31T00:00:00\" V=\"0\" />")]
        [TestCase("2018/1/31", double.NaN, "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<DV D=\"2018-01-31T00:00:00\" V=\"NaN\" />")]
        [TestCase("2018/1/31", -2345.67865, "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<DV D=\"2018-01-31T00:00:00\" V=\"-2345.67865\" />")]
        [TestCase("2018/1/31", double.NegativeInfinity, "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<DV D=\"2018-01-31T00:00:00\" V=\"-Infinity\" />")]
        [TestCase(null, null, "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<DV D=\"0001-01-01T00:00:00\" V=\"0\" />")]
        public void WriteXmlTests(DateTime day, double value, string expectedXml)
        {
            var val = new DailyValuation(day, value);
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

                Assert.AreEqual(expectedXml, output);
            }
        }

        [TestCase("2018/1/31", 278.671, "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<DailyValuation>\r\n  <Day>2018-01-31T00:00:00</Day>\r\n  <Value>278.671</Value>\r\n</DailyValuation>")]
        [TestCase("2018/1/31", 0.0, "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<DailyValuation>\r\n  <Day>2018-01-31T00:00:00</Day>\r\n  <Value>0</Value>\r\n</DailyValuation>")]
        [TestCase(null, null, "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<DailyValuation>\r\n  <Day>0001-01-01T00:00:00</Day>\r\n  <Value>0</Value>\r\n</DailyValuation>")]
        [TestCase("2018/1/31", 278.671, "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<DV D=\"2018-01-31T00:00:00\" V=\"278.671\" />")]
        [TestCase("2018/1/31", 0.0, "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<DV D=\"2018-01-31T00:00:00\" V=\"0\" />")]
        [TestCase(null, null, "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<DV D=\"0001-01-01T00:00:00\" V=\"0\" />")]
        [TestCase("2018/1/31", double.NaN, "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<DV D=\"2018-01-31T00:00:00\" V=\"NaN\" />")]
        [TestCase("2018/1/31", -2345.67865, "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<DV D=\"2018-01-31T00:00:00\" V=\"-2345.67865\" />")]
        [TestCase("2018/1/31", double.NegativeInfinity, "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<DV D=\"2018-01-31T00:00:00\" V=\"-Infinity\" />")]
        public void ReadXmlTests(DateTime day, double value, string actualXml)
        {
            var valuation = new DailyValuation();
            using (StringReader fs = new StringReader(actualXml))
            {
                XmlReaderSettings xmlWriterSettings = new XmlReaderSettings();
                using (XmlReader reader = XmlReader.Create(fs, xmlWriterSettings))
                {
                    valuation.ReadXml(reader);
                }

                Assert.AreEqual(new DailyValuation(day, value), valuation);
            }
        }

        [TestCase("2018/1/31", 278.671)]
        [TestCase("2018/1/31", double.NaN)]
        [TestCase("2018/1/31", -2345.67865)]
        [TestCase("2018/1/31", double.NegativeInfinity)]
        [TestCase("2018/1/31", 0.0)]
        [TestCase(null, null)]
        public void XmlRoundTripTests(DateTime day, double value)
        {
            string output;
            var val = new DailyValuation(day, value);
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

            var valuation = new DailyValuation();
            using (StringReader fs = new StringReader(output))
            {
                XmlReaderSettings xmlWriterSettings = new XmlReaderSettings();
                using (XmlReader reader = XmlReader.Create(fs, xmlWriterSettings))
                {
                    valuation.ReadXml(reader);
                }

                Assert.AreEqual(valuation, val);
            }
        }
    }
}
