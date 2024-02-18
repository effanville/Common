using Effanville.Common.Structure.MathLibrary;

using NUnit.Framework;

namespace Effanville.Common.Structure.Tests.MathLibrary
{
    [TestFixture]
    internal sealed class HelpersTests
    {
        [TestCase(5, 5, 5)]
        [TestCase(5, -2, -5)]
        [TestCase(-2, -2, -2)]
        [TestCase(5, 0.0, 5)]
        public void SignTests(double value, double sign, double expectedValue)
        {
            double signOutput = Helpers.Sign(value, sign);
            Assert.AreEqual(expectedValue, signOutput);
        }
    }
}
