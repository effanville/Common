﻿using System;
using System.Globalization;
using NUnit.Framework;
using UICommon.Converters;

namespace UICommon.Tests.Converters
{
    public sealed class UKDateConverterTests
    {
        private static readonly CultureInfo UKEnglishCulture = new CultureInfo("en-GB");

        [TestCase("13/2/2019", "13/02/2019 00:00:00")]
        public void CanConvert(object input, object expected)
        {
            DateTime inputTime = DateTime.Parse(input.ToString());
            StringToUKDateConverter converter = new StringToUKDateConverter();
            Assert.AreEqual(expected, converter.Convert(inputTime, null, null, UKEnglishCulture));
        }

        [TestCase("13/2/2019", "2019-02-13 00:00:00")]
        public void CanConvertBack(object input, object expected)
        {
            DateTime expectedTime = DateTime.Parse(expected.ToString());
            StringToUKDateConverter converter = new StringToUKDateConverter();
            Assert.AreEqual(expectedTime, converter.ConvertBack(input, null, null, UKEnglishCulture));
        }

        [TestCase("13/2/2019")]
        [TestCase("13/2/2019")]
        public void RountTripConvert(object input)
        {
            DateTime inputTime = DateTime.Parse(input.ToString());
            StringToUKDateConverter converter = new StringToUKDateConverter();
            object converted = converter.Convert(inputTime, null, null, UKEnglishCulture);
            object convertBack = converter.ConvertBack(converted, null, null, UKEnglishCulture);
            Assert.AreEqual(inputTime, convertBack);
        }

        [TestCase("13/2/2019")]
        [TestCase("13/2/2019")]
        public void RountTripConvertBack(object input)
        {
            StringToUKDateConverter converter = new StringToUKDateConverter();
            object converted = converter.ConvertBack(input, null, null, UKEnglishCulture);
            object convertBack = converter.Convert(converted, null, null, UKEnglishCulture);
            Assert.AreEqual(input, convertBack);
        }
    }
}
