using System;
using System.Collections.Generic;

using Effanville.Common.Structure.MathLibrary.RootFinding;
using Effanville.Common.Structure.Results;

using NUnit.Framework;

namespace Effanville.Common.Structure.Tests.MathLibrary.RootFinding
{
    [TestFixture]
    public sealed class SecantMethodTests
    {
        private static readonly Func<double, double> QuadraticRootsAtTwoAndFour = x => (x - 2) * (x - 4);
        private static readonly Func<double, double> QuadraticRootsAtTwo = x => (x - 2) * (x - 2);

        private static IEnumerable<TestCaseData> BracketSuccessTestData()
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

            yield return new TestCaseData(new TestData<double>()
            {
                Lower = -2,
                Upper = 1,
                Func = QuadraticRootsAtTwoAndFour,
                ExpectedResult = 2,
                MaxIterations = 30
            }).SetName("CalcRoot-BasicQuadraticGuessOutside");

            yield return new TestCaseData(new TestData<double>()
            {
                Lower = 1,
                Upper = 5,
                Func = QuadraticRootsAtTwo,
                ExpectedResult = 2,
                MaxIterations = 100,
                Tolerance = 1e-9
            }).SetName("CalcRoot-MultipleRootAtTwo");
        }

        [TestCaseSource(nameof(BracketSuccessTestData))]
        public void FindRootSuccessTests(TestData<double> testData)
        {
            var rootFindingResult = RootFinder.Secant.FindRoot(testData.Func, testData.Lower, testData.Upper, testData.MaxIterations, testData.Tolerance);
            Assert.That(rootFindingResult.Failure, Is.False);
            Assert.That(Math.Abs(rootFindingResult.Data - testData.ExpectedResult), Is.LessThan(1e-8));
        }
        private static IEnumerable<TestCaseData> BracketFailureTestData()
        {
            yield return new TestCaseData(new TestData<double>()
            {
                Lower = 1,
                Upper = 5,
                Func = QuadraticRootsAtTwo,
                ExpectedResult = double.NaN,
                MaxIterations = 30,
                ErrorMessage = "Exceeded max number of iterations."
            }).SetName("FailureCalcRoot-MultipleRootAtTwo");
        }

        [TestCaseSource(nameof(BracketFailureTestData))]
        public void FindRootFailureTests(TestData<double> testData)
        {
            var rootFindingResult = RootFinder.Secant.FindRoot(testData.Func, testData.Lower, testData.Upper, maxIterations: 20);
            Assert.That(rootFindingResult.Failure, Is.True);
            var res = rootFindingResult as ErrorResult<double>;
            Assert.That(res.Message, Is.EqualTo(testData.ErrorMessage));
            Assert.That(Math.Abs(rootFindingResult.Data - testData.ExpectedResult), Is.LessThan(1e-8));
        }
    }
}
