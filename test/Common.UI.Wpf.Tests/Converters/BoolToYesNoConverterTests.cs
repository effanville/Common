using Effanville.Common.UI.Wpf.Converters;

using NUnit.Framework;

namespace Effanville.Common.UI.Wpf.Tests.Converters
{
    [TestFixture]
    public class BoolToYesNoConverterTests
    {
        [TestCase(true, "Yes")]
        [TestCase(false, "No")]
        [TestCase("yes", null)]
        public void CanConvert(object input, object expected)
        {
            BoolToYesNoConverter converter = new BoolToYesNoConverter();
            Assert.That(converter.Convert(input, null, null, null), Is.EqualTo(expected));
        }

        [TestCase("Yes", true)]
        [TestCase("No", false)]
        [TestCase(4, null)]
        public void CanConvertBack(object input, object expected)
        {
            BoolToYesNoConverter converter = new BoolToYesNoConverter();
            Assert.That(converter.ConvertBack(input, null, null, null), Is.EqualTo(expected));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void RountTripConvert(object input)
        {
            BoolToYesNoConverter converter = new BoolToYesNoConverter();
            object converted = converter.Convert(input, null, null, null);
            object convertBack = converter.ConvertBack(converted, null, null, null);
            Assert.That(convertBack, Is.EqualTo(input));
        }

        [TestCase("Yes")]
        [TestCase("No")]
        public void RountTripConvertBack(object input)
        {
            BoolToYesNoConverter converter = new BoolToYesNoConverter();
            object converted = converter.ConvertBack(input, null, null, null);
            object convertBack = converter.Convert(converted, null, null, null);
            Assert.That(convertBack, Is.EqualTo(input));
        }
    }
}
