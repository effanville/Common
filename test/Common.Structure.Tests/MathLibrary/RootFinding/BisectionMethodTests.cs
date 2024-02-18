using System;
using System.Collections.Generic;

using Common.Structure.MathLibrary.RootFinding;
using Common.Structure.Results;

using NUnit.Framework;

namespace Effanville.Common.Structure.Tests.MathLibrary.RootFinding
{
    [TestFixture]
    public sealed class BisectionMethodTests
    {
        private static readonly Func<double, double> QuadraticRootsAtTwoAndFour = x => (x - 2) * (x - 4);
        private static readonly Func<double, double> QuadraticRootsAtTwo = x => (x - 2) * (x - 2);

        private static IEnumerable<TestCaseData> SuccessTestData()
        {
            yield return new TestCaseData(new TestData<double>()
            {
                Lower = 0,
                Upper = 3,
                Func = QuadraticRootsAtTwoAndFour,
                ExpectedResult = 2,
                MaxIterations = 30
            }).SetName("CalcRoot-BasicQuadratic");

            yield return new TestCaseData(new TestData<double>()
            {
                Lower = 3,
                Upper = 5,
                Func = QuadraticRootsAtTwoAndFour,
                ExpectedResult = 4,
                MaxIterations = 30
            }).SetName("CalcRoot-BasicQuadraticOtherRoot");

        }

        [TestCaseSource(nameof(SuccessTestData))]
        public void FindRootSuccessTests(TestData<double> testData)
        {
            var rootFindingResult = RootFinder.Bisection.FindRoot(
                testData.Func,
                testData.Lower,
                testData.Upper,
                testData.MaxIterations,
                testData.Tolerance);
            Assert.IsFalse(rootFindingResult.Failure);
            Assert.That(Math.Abs(rootFindingResult.Data - testData.ExpectedResult), Is.LessThan(1e-8));
        }
        private static IEnumerable<TestCaseData> FailureTestData()
        {
            yield return new TestCaseData(new TestData<double>()
            {
                Lower = 1,
                Upper = 5,
                Func = QuadraticRootsAtTwo,
                ExpectedResult = double.NaN,
                MaxIterations = 30,
                ErrorMessage = "Root must be bracketed for bisection to work."
            }).SetName("FailureCalcRoot-MultipleRootAtTwo");

            yield return new TestCaseData(new TestData<double>()
            {
                Lower = -2,
                Upper = 1,
                Func = QuadraticRootsAtTwoAndFour,
                ExpectedResult = double.NaN,
                MaxIterations = 30,
                ErrorMessage = "Root must be bracketed for bisection to work."
            }).SetName("CalcRoot-BasicQuadraticGuessOutside");
        }

        [TestCaseSource(nameof(FailureTestData))]
        public void FindRootFailureTests(TestData<double> testData)
        {
            var rootFindingResult = RootFinder.Bisection.FindRoot(
                testData.Func,
                testData.Lower,
                testData.Upper,
                maxIterations: 20);
            Assert.IsTrue(rootFindingResult.Failure);
            var res = rootFindingResult as ErrorResult<double>;
            Assert.AreEqual(testData.ErrorMessage, res.Message);
            Assert.That(Math.Abs(rootFindingResult.Data - testData.ExpectedResult), Is.LessThan(1e-8));
        }
    }
}
