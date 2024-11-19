using Effanville.Common.Structure.Extensions;

using NUnit.Framework;

namespace Effanville.Common.Structure.Tests.Extensions
{
    [TestFixture]
    public sealed class EnumExtensionsTests
    {
        private enum TestEnum1
        {
            Value,
            ValueToo
        }

        private enum TestEnum2
        {
            Value,
            Val
        }

        [Test]
        public void LongestEntryTests()
        {
            int length = EnumExtensions.LongestEntry<TestEnum1>();
            Assert.That(length, Is.EqualTo(8));

            int enum2Length = EnumExtensions.LongestEntry<TestEnum2>();
            Assert.That(enum2Length, Is.EqualTo(5));
        }
    }
}
