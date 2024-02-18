using Common.Structure.Extensions;

using NUnit.Framework;

namespace Common.Structure.Tests.Extensions
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
            Assert.AreEqual(8, length);

            int enum2Length = EnumExtensions.LongestEntry<TestEnum2>();
            Assert.AreEqual(5, enum2Length);
        }
    }
}
