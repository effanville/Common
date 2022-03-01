using System;
using System.Collections.Generic;

using Common.Structure.MathLibrary.Optimisation;
using Common.Structure.MathLibrary.Optimisation.Scalar;

using NUnit.Framework;

namespace Common.Structure.Tests.MathLibrary.Optimisation
{
    [TestFixture]
    public sealed class GoldenSectionMinimiserTests
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

            public ScalarMinResult ExpectedResult
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
                ExpectedResult = new ScalarMinResult(0, 0, ExitCondition.BoundTolerance)
            }).SetName("StandardQuadratic");
            // Following three all should have 3 as solution.
            yield return new TestCaseData(new TestData()
            {
                Lower = -100,
                Upper = 100,
                Func = x => (x - 3) * (x - 3),
                ExpectedResult = new ScalarMinResult(3, 0, ExitCondition.BoundTolerance)
            }).SetName("QuadraticCenteredAtThree");

            yield return new TestCaseData(new TestData()
            {
                Lower = -10,
                Upper = 10,
                Func = x => (x - 3) * (x - 3),
                ExpectedResult = new ScalarMinResult(3, 0, ExitCondition.BoundTolerance)
            }).SetName("QuadraticCenteredAtThree2");

            yield return new TestCaseData(new TestData()
            {
                Lower = 0,
                Upper = 5,
                Func = x => (x - 3) * (x - 3),
                ExpectedResult = new ScalarMinResult(3, 0, ExitCondition.BoundTolerance)
            }).SetName("QuadraticCenteredAtThree3");

            yield return new TestCaseData(new TestData()
            {
                Lower = 0,
                Upper = 5,
                Func = x => (x - 3) * (x - 3) * (x + 1),
                ExpectedResult = new ScalarMinResult(3, 0, ExitCondition.BoundTolerance)
            }).SetName("CubicCenteredAtThree");

        }

        [TestCaseSource(nameof(MinimumTestData))]
        public void CalculateMinimumMathNet(TestData data)
        {
            ScalarMinResult min = GoldenSectionSearch.MinimumFromMathNet(data.Lower, data.Upper, data.Func, 1e-8, 100);
            Assert.Multiple(() =>
            {
                Assert.AreEqual(data.ExpectedResult.MinimisingPoint, min.MinimisingPoint, 1e-8);
                Assert.AreEqual(data.ExpectedResult.MinimisingValue, min.MinimisingValue, 1e-8);
                Assert.That(min.ReasonForExit, Is.EqualTo(data.ExpectedResult.ReasonForExit));
            });
        }

        [TestCaseSource(nameof(MinimumTestData))]
        public void CalculateMinimum(TestData data)
        {
            ScalarMinResult min = GoldenSectionSearch.Minimise(data.Lower, data.Upper, data.Func, 1e-8, 100);
            Assert.Multiple(() =>
            {
                Assert.AreEqual(data.ExpectedResult.MinimisingPoint, min.MinimisingPoint, 1e-8);
                Assert.AreEqual(data.ExpectedResult.MinimisingValue, min.MinimisingValue, 1e-8);
                Assert.That(min.ReasonForExit, Is.EqualTo(data.ExpectedResult.ReasonForExit));
            });
        }
    }
}
