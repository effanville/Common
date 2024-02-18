using System.Collections.Generic;

using Common.Structure.Extensions;

using NUnit.Framework;

namespace Effanville.Common.Structure.Tests.Extensions
{
    [TestFixture]
    public sealed class DoubleExtensionsTests
    {
        private static IEnumerable<TestCaseData> TruncateToStringTestsData()
        {
            yield return new TestCaseData(0.000000000, 2, "0");
            yield return new TestCaseData(0.42345678, 2, "0.42");
            yield return new TestCaseData(12345.000000000, 2, "12345");
            yield return new TestCaseData(123.12345, 3, "123.123");
        }

        [TestCaseSource(nameof(TruncateToStringTestsData))]
        public void TruncateToStringTests(double value, int decimalPlaces, string expected)
        {
            string truncated = value.TruncateToString(decimalPlaces);
            Assert.AreEqual(expected, truncated);
        }
        private static IEnumerable<TestCaseData> TruncateTestsData()
        {
            yield return new TestCaseData(0.0000000001, 2, 0.00);
            yield return new TestCaseData(0.42345678, 2, 0.42);
            yield return new TestCaseData(12345.000000000, 2, 12345.00);
            yield return new TestCaseData(123.12345, 3, 123.123);
        }

        [TestCaseSource(nameof(TruncateTestsData))]
        public void TruncateTests(double value, int decimalPlaces, double expected)
        {
            double truncated = value.Truncate(decimalPlaces);
            Assert.AreEqual(expected, truncated);
        }

        private static IEnumerable<TestCaseData> EqualsTestsData()
        {
            yield return new TestCaseData(0.0, 0.0, 0.0, true);
            yield return new TestCaseData(0.0, 0.1, 0.0, false);
            yield return new TestCaseData(0.0, 0.0, 0.0000001, true);
            yield return new TestCaseData(0.01, 0.0101, 0.001, true);
            yield return new TestCaseData(0.01, 0.0001, 0.001, false);
            yield return new TestCaseData(0.01, 0.011, 0.0001, false);
            yield return new TestCaseData(10, 20, 10, false);
            yield return new TestCaseData(10, 20, 9, false);
            yield return new TestCaseData(10, 20, 11, true);
        }

        [TestCaseSource(nameof(EqualsTestsData))]
        public void EqualsTests(double first, double second, double tol, bool expected)
        {
            bool actual = first.Equals(second, tol);
            Assert.AreEqual(expected, actual);
        }

        private static IEnumerable<TestCaseData> EqualsRelativeTestsData()
        {
            yield return new TestCaseData(0.0, 0.0, 0.0, false, true);
            yield return new TestCaseData(0.0, 0.1, 0.0, false, false);
            yield return new TestCaseData(0.0, 0.0, 0.0000001, false, true);
            yield return new TestCaseData(0.01, 0.0101, 0.001, false, true);
            yield return new TestCaseData(0.01, 0.0001, 0.001, false, false);
            yield return new TestCaseData(0.01, 0.011, 0.0001, false, false);
            yield return new TestCaseData(10, 20, 10, false, false);
            yield return new TestCaseData(10, 20, 9, false, false);
            yield return new TestCaseData(10, 20, 11, false, true);
            yield return new TestCaseData(0.0, 0.0, 0.0, true, true);
            yield return new TestCaseData(0.0, 0.1, 0.0, true, false);
            yield return new TestCaseData(0.0, 0.0, 0.0000001, true, true);
            yield return new TestCaseData(5.0, 5.0, 0.0000001, true, true);
            yield return new TestCaseData(0.01, 0.0101, 0.001, true, true);
            yield return new TestCaseData(0.01, 0.0001, 0.001, true, false);
            yield return new TestCaseData(0.01, 0.011, 0.0001, true, false);
            yield return new TestCaseData(10, 20, 1.0, true, false);
            yield return new TestCaseData(10, 20, 0.95, true, false);
            yield return new TestCaseData(10, 20, 1.05, true, true);
        }

        [TestCaseSource(nameof(EqualsRelativeTestsData))]
        public void EqualsRelativeTests(double first, double second, double tol, bool isRelative, bool expected)
        {
            bool actual = first.Equals(second, tol, isRelative);
            Assert.AreEqual(expected, actual);
        }
    }
}
