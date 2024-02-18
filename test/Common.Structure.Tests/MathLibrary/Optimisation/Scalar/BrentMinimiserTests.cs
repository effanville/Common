using System;
using System.Collections.Generic;

using Common.Structure.MathLibrary.Optimisation;
using Common.Structure.MathLibrary.Optimisation.Scalar;

using NUnit.Framework;

namespace Effanville.Common.Structure.Tests.MathLibrary.Optimisation.Scalar
{
    [TestFixture]
    public sealed class BrentMinimiserTests
    {
        public sealed class TestData
        {
            public double Lower
            {
                get;
                set;
            }
            public double Upper
            {
                get;
                set;
            }
            public Func<double, double> Func
            {
                get;
                set;
            }

            public OptimisationSuccessResult<ScalarFuncEval> ExpectedResult
            {
                get;
                set;
            }
        }

        private static IEnumerable<TestCaseData> MinimumTestData()
        {
            yield return new TestCaseData(new TestData()
            {
                Lower = 0,
                Upper = 1,
                Func = value => value * value,
                ExpectedResult = new OptimisationSuccessResult<ScalarFuncEval>(new ScalarFuncEval(0, 0), ExitCondition.Converged, 1)
            }).SetName("StandardQuadratic");
            // Following three all should have 3 as solution.
            yield return new TestCaseData(new TestData()
            {
                Lower = -100,
                Upper = 100,
                Func = x => (x - 3) * (x - 3),
                ExpectedResult = new OptimisationSuccessResult<ScalarFuncEval>(new ScalarFuncEval(3, 0), ExitCondition.Converged, 1)
            }).SetName("QuadraticCenteredAtThree");

            yield return new TestCaseData(new TestData()
            {
                Lower = -10,
                Upper = 10,
                Func = x => (x - 3) * (x - 3),
                ExpectedResult = new OptimisationSuccessResult<ScalarFuncEval>(new ScalarFuncEval(3, 0), ExitCondition.Converged, 1)
            }).SetName("QuadraticCenteredAtThree2");

            yield return new TestCaseData(new TestData()
            {
                Lower = 0,
                Upper = 5,
                Func = x => (x - 3) * (x - 3),
                ExpectedResult = new OptimisationSuccessResult<ScalarFuncEval>(new ScalarFuncEval(3, 0), ExitCondition.Converged, 1)
            }).SetName("QuadraticCenteredAtThree3");

            yield return new TestCaseData(new TestData()
            {
                Lower = 0,
                Upper = 5,
                Func = x => (x - 3) * (x - 3) * (x + 1),
                ExpectedResult = new OptimisationSuccessResult<ScalarFuncEval>(new ScalarFuncEval(3, 0), ExitCondition.Converged, 1)
            }).SetName("CubicCenteredAtThree");

        }

        [TestCaseSource(nameof(MinimumTestData))]
        public void CalculateMinimum(TestData data)
        {
            var min = Brent.Minimise(data.Lower, data.Upper, data.Func, 1e-8, 100);
            Assert.Multiple(() =>
            {
                Assert.AreEqual(data.ExpectedResult.Data.Point, min.Data.Point, 1e-8);
                Assert.AreEqual(data.ExpectedResult.Data.Value, min.Data.Value, 1e-8);
                var optRes = min as IOptimisationResult<ScalarFuncEval>;
                Assert.That(optRes.ReasonForExit, Is.EqualTo(data.ExpectedResult.ReasonForExit));
            });
        }
    }
}
