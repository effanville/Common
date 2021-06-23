using NUnit.Framework;
using Common.UI.Converters;

namespace Common.UI.Tests.Converters
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
            Assert.AreEqual(expected, converter.Convert(input, null, null, null));
        }

        [TestCase("Yes", true)]
        [TestCase("No", false)]
        [TestCase(4, null)]
        public void CanConvertBack(object input, object expected)
        {
            BoolToYesNoConverter converter = new BoolToYesNoConverter();
            Assert.AreEqual(expected, converter.ConvertBack(input, null, null, null));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void RountTripConvert(object input)
        {
            BoolToYesNoConverter converter = new BoolToYesNoConverter();
            object converted = converter.Convert(input, null, null, null);
            object convertBack = converter.ConvertBack(converted, null, null, null);
            Assert.AreEqual(input, convertBack);
        }

        [TestCase("Yes")]
        [TestCase("No")]
        public void RountTripConvertBack(object input)
        {
            BoolToYesNoConverter converter = new BoolToYesNoConverter();
            object converted = converter.ConvertBack(input, null, null, null);
            object convertBack = converter.Convert(converted, null, null, null);
            Assert.AreEqual(input, convertBack);
        }
    }
}
