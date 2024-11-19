using System;
using System.Globalization;

using Effanville.Common.UI.Wpf.Converters;

using NUnit.Framework;

namespace Effanville.Common.UI.Wpf.Tests.Converters
{
    public sealed class UKDateConverterTests
    {
        private static readonly CultureInfo UKEnglishCulture = new CultureInfo("en-GB");

        [TestCase("13/2/2019", "13/02/2019")]
        public void CanConvert(object input, object expected)
        {
            DateTime inputTime = DateTime.Parse(input.ToString(), UKEnglishCulture);
            StringToUKDateConverter converter = new StringToUKDateConverter();
            Assert.That(converter.Convert(inputTime, null, null, UKEnglishCulture), Is.EqualTo(expected));
        }

        [TestCase("13/2/2019", "2019-02-13")]
        public void CanConvertBack(object input, object expected)
        {
            DateTime expectedTime = DateTime.Parse(expected.ToString(), UKEnglishCulture);
            StringToUKDateConverter converter = new StringToUKDateConverter();
            Assert.That(converter.ConvertBack(input, null, null, UKEnglishCulture), Is.EqualTo(expectedTime));
        }

        [TestCase("13/2/2019")]
        [TestCase("13/2/2019")]
        public void RoundTripConvert(object input)
        {
            DateTime inputTime = DateTime.Parse(input.ToString(), UKEnglishCulture);
            StringToUKDateConverter converter = new StringToUKDateConverter();
            object converted = converter.Convert(inputTime, null, null, UKEnglishCulture);
            object convertBack = converter.ConvertBack(converted, null, null, UKEnglishCulture);
            Assert.That(convertBack, Is.EqualTo(inputTime));
        }

        [TestCase("13/02/2019")]
        public void RountTripConvertBack(object input)
        {
            StringToUKDateConverter converter = new StringToUKDateConverter();
            object converted = converter.ConvertBack(input, null, null, UKEnglishCulture);
            object convertBack = converter.Convert(converted, null, null, UKEnglishCulture);
            Assert.That(convertBack, Is.EqualTo(input));
        }
    }
}
