using System;
using System.Collections.Generic;

using Effanville.Common.Structure.MathLibrary.Vectors;

using NUnit.Framework;

namespace Effanville.Common.Structure.Tests.MathLibrary.Vectors
{
    [TestFixture]
    internal sealed class VectorStatsTests
    {
        private static IEnumerable<TestCaseData> MaxTestCases()
        {
            yield return new TestCaseData(null, 0, double.NaN);
            yield return new TestCaseData(new List<double>(), 0, double.NaN);
            yield return new TestCaseData(new List<double>(), 1, double.NaN);
            yield return new TestCaseData(new List<double>() { 1.0 }, 1, 1.0);
            yield return new TestCaseData(new List<double>() { 1.0, 4.0 }, 1, 4.0);
            yield return new TestCaseData(new List<double>() { 4.0, 1.0 }, 1, 1.0);
            yield return new TestCaseData(new List<double>() { 6.0, 4.0 }, 2, 6.0);
            yield return new TestCaseData(new List<double>() { -6.0, -4.0 }, 2, -4.0);
            yield return new TestCaseData(new List<double>() { 6.0, -4.0 }, 2, 6.0);
        }

        [TestCaseSource(nameof(MaxTestCases))]
        public void MaxTests(List<double> values, int number, double expected)
        {
            double max = DoubleVector.Max(values, number, double.NaN);
            Assert.AreEqual(expected, max);
        }

        private static IEnumerable<TestCaseData> MinTestCases()
        {
            yield return new TestCaseData(null, 0, double.NaN);
            yield return new TestCaseData(new List<double>(), 0, double.NaN);
            yield return new TestCaseData(new List<double>(), 1, double.NaN);
            yield return new TestCaseData(new List<double>() { 1.0 }, 1, 1.0);
            yield return new TestCaseData(new List<double>() { 1.0, 4.0 }, 1, 4.0);
            yield return new TestCaseData(new List<double>() { 4.0, 1.0 }, 1, 1.0);
            yield return new TestCaseData(new List<double>() { 6.0, 4.0 }, 2, 4.0);
            yield return new TestCaseData(new List<double>() { -6.0, -4.0 }, 2, -6.0);
            yield return new TestCaseData(new List<double>() { 6.0, -4.0 }, 2, -4.0);
        }

        [TestCaseSource(nameof(MinTestCases))]
        public void MinTests(List<double> values, int number, double expected)
        {
            double min = DoubleVector.Min(values, number, double.NaN);
            Assert.AreEqual(expected, min);
        }

        private static IEnumerable<TestCaseData> MeanTestCases()
        {
            yield return new TestCaseData(null, 0, double.NaN);
            yield return new TestCaseData(new List<double>(), 0, double.NaN);
            yield return new TestCaseData(new List<double>(), 1, double.NaN);
            yield return new TestCaseData(new List<double>() { 1.0 }, 1, 1.0);
            yield return new TestCaseData(new List<double>() { 1.0, 4.0 }, 1, 4.0);
            yield return new TestCaseData(new List<double>() { 4.0, 1.0 }, 1, 1.0);
            yield return new TestCaseData(new List<double>() { 6.0, 4.0 }, 2, 5.0);
            yield return new TestCaseData(new List<double>() { -6.0, -4.0 }, 2, -5.0);
            yield return new TestCaseData(new List<double>() { 6.0, -4.0 }, 2, 1.0);
        }

        [TestCaseSource(nameof(MeanTestCases))]
        public void MeanTests(List<double> values, int number, double expected)
        {
            double mean = DoubleVector.Mean(values, number);
            Assert.AreEqual(expected, mean);
        }

        private static IEnumerable<TestCaseData> VarianceTestCases()
        {
            yield return new TestCaseData(null, 0, double.NaN);
            yield return new TestCaseData(new List<double>(), 0, double.NaN);
            yield return new TestCaseData(new List<double>(), 1, double.NaN);
            yield return new TestCaseData(new List<double>() { 1.0 }, 1, double.NaN);
            yield return new TestCaseData(new List<double>() { 1.0, 4.0 }, 1, double.NaN);
            yield return new TestCaseData(new List<double>() { 4.0, 1.0, 2.0 }, 2, 0.5);
            yield return new TestCaseData(new List<double>() { 6.0, 4.0 }, 2, 2.0);
            yield return new TestCaseData(new List<double>() { -6.0, -4.0, 3.0 }, 2, 24.5);
            yield return new TestCaseData(new List<double>() { 6.0, -4.0, 2.345 }, 2, 20.129512500000004);
        }

        [TestCaseSource(nameof(VarianceTestCases))]
        public void VarianceTests(List<double> values, int number, double expected)
        {
            double variance = DoubleVector.Variance(values, number);
            Assert.AreEqual(expected, variance);
        }

        private static IEnumerable<TestCaseData> StdDevTestCases()
        {
            yield return new TestCaseData(null, 0, double.NaN);
            yield return new TestCaseData(new List<double>(), 0, double.NaN);
            yield return new TestCaseData(new List<double>(), 1, double.NaN);
            yield return new TestCaseData(new List<double>() { 1.0 }, 1, double.NaN);
            yield return new TestCaseData(new List<double>() { 1.0, 4.0 }, 1, double.NaN);
            yield return new TestCaseData(new List<double>() { 4.0, 1.0, 2.0 }, 2, 0.70710678118654757);
            yield return new TestCaseData(new List<double>() { 6.0, 4.0 }, 2, 1.4142135623730951);
            yield return new TestCaseData(new List<double>() { -6.0, -4.0, 3.0 }, 2, 4.9497474683058327);
            yield return new TestCaseData(new List<double>() { 6.0, -4.0, 2.345 }, 2, 4.4865925266286446);
        }

        [TestCaseSource(nameof(StdDevTestCases))]
        public void StdDevTests(List<double> values, int number, double expected)
        {
            double stdDev = DoubleVector.StandardDev(values, number);
            Assert.AreEqual(expected, stdDev);
        }

        private static IEnumerable<TestCaseData> VarianceStdDevTestCases()
        {
            yield return new TestCaseData(null, 0);
            yield return new TestCaseData(new List<double>(), 0);
            yield return new TestCaseData(new List<double>(), 1);
            yield return new TestCaseData(new List<double>() { 1.0 }, 1);
            yield return new TestCaseData(new List<double>() { 1.0, 4.0 }, 1);
            yield return new TestCaseData(new List<double>() { 4.0, 1.0, 2.0 }, 2);
            yield return new TestCaseData(new List<double>() { 6.0, 4.0 }, 2);
            yield return new TestCaseData(new List<double>() { -6.0, -4.0, 3.0 }, 2);
            yield return new TestCaseData(new List<double>() { 6.0, -4.0, 2.345 }, 2);
        }

        [TestCaseSource(nameof(VarianceStdDevTestCases))]
        public void VarianceAndStdDevAgreeTests(List<double> values, int number)
        {
            double stdDev = DoubleVector.StandardDev(values, number);
            double variance = DoubleVector.Variance(values, number);
            Assert.AreEqual(Math.Sqrt(variance), stdDev);
        }
    }
}
