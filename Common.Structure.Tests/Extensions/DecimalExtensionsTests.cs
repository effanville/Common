using System;
using System.Collections.Generic;

using Common.Structure.Extensions;

using NUnit.Framework;

namespace Common.Structure.Tests.Extensions
{
    [TestFixture]
    public sealed class DecimalExtensionsTests
    {
        private static IEnumerable<TestCaseData> TruncateToStringTestsData()
        {
            yield return new TestCaseData(0.000000000m, 2, "0");
            yield return new TestCaseData(0.42345678m, 2, "0.42");
            yield return new TestCaseData(12345.000000000m, 2, "12345");
            yield return new TestCaseData(123.12345m, 3, "123.123");
        }

        [TestCaseSource(nameof(TruncateToStringTestsData))]
        public void TruncateToStringTests(decimal value, int decimalPlaces, string expected)
        {
            string truncated = value.TruncateToString(decimalPlaces);
            Assert.AreEqual(expected, truncated);
        }
        private static IEnumerable<TestCaseData> TruncateTestsData()
        {
            yield return new TestCaseData(0.000000000m, 2, 0.00m);
            yield return new TestCaseData(0.42345678m, 2, 0.42m);
            yield return new TestCaseData(12345.000000000m, 2, 12345.00m);
            yield return new TestCaseData(123.12345m, 3, 123.123m);
        }

        [TestCaseSource(nameof(TruncateTestsData))]
        public void TruncateTests(decimal value, int decimalPlaces, decimal expected)
        {
            decimal truncated = value.Truncate(decimalPlaces);
            Assert.AreEqual(expected, truncated);
        }

        private static IEnumerable<TestCaseData> EqualsTestsData()
        {
            yield return new TestCaseData(0.0m, 0.0m, 0.0m, true);
            yield return new TestCaseData(0.0m, 0.1m, 0.0m, false);
            yield return new TestCaseData(0.0m, 0.0m, 0.0000001m, true);
            yield return new TestCaseData(0.01m, 0.0101m, 0.001m, true);
            yield return new TestCaseData(0.01m, 0.0001m, 0.001m, false);
            yield return new TestCaseData(0.01m, 0.011m, 0.0001m, false);
            yield return new TestCaseData(10m, 20m, 10m, false);
            yield return new TestCaseData(10m, 20m, 9m, false);
            yield return new TestCaseData(10m, 20m, 11m, true);
        }

        [TestCaseSource(nameof(EqualsTestsData))]
        public void EqualsTests(decimal first, decimal second, decimal tol, bool expected)
        {
            bool actual = first.Equals(second, tol);
            Assert.AreEqual(expected, actual);
        }

        private static IEnumerable<TestCaseData> EqualsRelativeTestsData()
        {
            yield return new TestCaseData(0.0m, 0.0m, 0.0m, false, true);
            yield return new TestCaseData(0.0m, 0.1m, 0.0m, false, false);
            yield return new TestCaseData(0.0m, 0.0m, 0.0000001m, false, true);
            yield return new TestCaseData(0.01m, 0.0101m, 0.001m, false, true);
            yield return new TestCaseData(0.01m, 0.0001m, 0.001m, false, false);
            yield return new TestCaseData(0.01m, 0.011m, 0.0001m, false, false);
            yield return new TestCaseData(10m, 20m, 10m, false, false);
            yield return new TestCaseData(10m, 20m, 9m, false, false);
            yield return new TestCaseData(10m, 20m, 11m, false, true);
            yield return new TestCaseData(0.0m, 0.0m, 0.0m, true, true);
            yield return new TestCaseData(0.0m, 0.1m, 0.0m, true, false);
            yield return new TestCaseData(0.0m, 0.0m, 0.0000001m, true, true);
            yield return new TestCaseData(5.0m, 5.0m, 0.0000001m, true, true);
            yield return new TestCaseData(0.01m, 0.0101m, 0.001m, true, true);
            yield return new TestCaseData(0.01m, 0.0001m, 0.001m, true, false);
            yield return new TestCaseData(0.01m, 0.011m, 0.0001m, true, false);
            yield return new TestCaseData(10m, 20m, 1.0m, true, false);
            yield return new TestCaseData(10m, 20m, 0.95m, true, false);
            yield return new TestCaseData(10m, 20m, 1.05m, true, true);
        }

        [TestCaseSource(nameof(EqualsRelativeTestsData))]
        public void EqualsRelativeTests(decimal first, decimal second, decimal tol, bool isRelative, bool expected)
        {
            bool actual = first.Equals(second, tol, isRelative);
            Assert.AreEqual(expected, actual);
        }

        private static IEnumerable<TestCaseData> PowTestsData()
        {
            yield return new TestCaseData(1.0m, 0, 1.0m);
            yield return new TestCaseData(1.0m, 1, 1.0m);
            yield return new TestCaseData(2.0m, 0, 1.0m);
            yield return new TestCaseData(2.0m, 2, 4.0m);
            yield return new TestCaseData(2.0m, 10, 1024.0m);
            yield return new TestCaseData(2.5m, 2, 6.25m);
        }

        [TestCaseSource(nameof(PowTestsData))]
        public void PowTests(decimal first, int exp, decimal expected)
        {
            decimal actual = first.Pow(exp);
            Assert.AreEqual(expected, actual);
        }

        private static IEnumerable<TestCaseData> PowErrorTestsData()
        {
            yield return new TestCaseData(1.0m, -1);
        }

        [TestCaseSource(nameof(PowErrorTestsData))]
        public void PowErrorTests(decimal first, int exp)
        {
            _ = Assert.Throws<ArgumentOutOfRangeException>(() => { _ = first.Pow(exp); });
        }
    }
}
