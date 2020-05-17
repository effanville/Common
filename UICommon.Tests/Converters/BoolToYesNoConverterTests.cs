﻿using NUnit.Framework;
using UICommon.Converters;

namespace UICommon.Tests.Converters
{
    [TestFixture]
    public class BoolToYesNoConverterTests
    {
        [TestCase(true, "Yes")]
        [TestCase(false, "No")]
        [TestCase("yes", null)]
        public void CanConvert(object input, object expected)
        {
            var converter = new BoolToYesNoConverter();
            Assert.AreEqual(expected, converter.Convert(input, null, null, null));
        }

        [TestCase("Yes", true)]
        [TestCase("No", false)]
        [TestCase(4, null)]
        public void CanConvertBack(object input, object expected)
        {
            var converter = new BoolToYesNoConverter();
            Assert.AreEqual(expected, converter.ConvertBack(input, null, null, null));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void RountTripConvert(object input)
        {
            var converter = new BoolToYesNoConverter();
            var converted = converter.Convert(input, null, null, null);
            var convertBack = converter.ConvertBack(converted, null, null, null);
            Assert.AreEqual(input, convertBack);
        }

        [TestCase("Yes")]
        [TestCase("No")]
        public void RountTripConvertBack(object input)
        {
            var converter = new BoolToYesNoConverter();
            var converted = converter.ConvertBack(input, null, null, null);
            var convertBack = converter.Convert(converted, null, null, null);
            Assert.AreEqual(input, convertBack);
        }
    }
}