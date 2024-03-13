using System.Collections.Generic;

using Effanville.Common.Structure.MathLibrary;

using NUnit.Framework;

namespace Effanville.Common.Structure.Tests.MathLibrary
{
    [TestFixture]
    public sealed class ResidualsTests
    {
        public static IEnumerable<TestCaseData> MSETestCases()
        {
            yield return new TestCaseData(new[] { 1.0 }, new[] { 2.0, 0.0 }, -1.0).SetName("Invalid lengths");
            yield return new TestCaseData(new[] { 1.0, 1.0 }, new[] { 1.0, 1.0 }, 0.0);
            yield return new TestCaseData(new[] { 1.0, 1.0 }, new[] { 2.0, 1.0 }, 1.0);
            yield return new TestCaseData(new[] { 1.0, 1.0 }, new[] { 2.0, 0.0 }, 2.0);
            yield return new TestCaseData(new[] { 1.0, 1.0 }, new[] { 1.5, 0.0 }, 1.25);
        }

        [TestCaseSource(nameof(MSETestCases))]
        public void MeanSquareErrorTests(double[] a, double[] b, double expected)
        {
            double mse = Residuals.MeanSquareError(a, b);
            Assert.AreEqual(expected, mse);
        }
    }
}
