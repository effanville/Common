using System;
using System.Collections.Generic;

using Common.Structure.MathLibrary.Optimisation.Scalar;

using NUnit.Framework;

namespace Common.Structure.Tests.MathLibrary.Optimisation.Scalar
{
    [TestFixture]
    public sealed class BracketMethodTests
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

            public BracketMethod.ExtremumBracket ExpectedResult
            {
                get;
                set;
            }
        }

        private static IEnumerable<TestCaseData> BracketTestData()
        {
            yield return new TestCaseData(new TestData()
            {
                Lower = -1,
                Upper = 1,
                Func = value => value * value,
                ExpectedResult = new BracketMethod.ExtremumBracket(-1, 1, 4.61803398875, 1, 1, 21.326237921250232)
            }).SetName("CalcBracket-BasicQuadratic");
            yield return new TestCaseData(new TestData()
            {
                Lower = -100,
                Upper = 100,
                Func = x => (x - 3) * (x - 3),
                ExpectedResult = new BracketMethod.ExtremumBracket(-100, 100, 301.61803398874997, 10609, 9409, 89172.730223306236)
            }).SetName("CalcBracket-QuadraticCenteredAtThree");
        }

        [TestCaseSource(nameof(BracketTestData))]
        [Test]
        public void CalcBracket(TestData testData)
        {
            var actualResult = BracketMethod.Bracket(testData.Lower, testData.Upper, testData.Func);
            Assert.That(actualResult, Is.Not.Null);
            Assert.That(actualResult.Failure, Is.False);
            Assert.That(actualResult.Data, Is.EqualTo(testData.ExpectedResult));
        }

        private static IEnumerable<TestCaseData> BracketFromBoundsTestData()
        {
            yield return new TestCaseData(new TestData()
            {
                Lower = -1,
                Upper = 1,
                Func = value => value * value,
                ExpectedResult = new BracketMethod.ExtremumBracket(-1, -0.23606797749982034, 1, 1, 0.055728090000855678, 1)
            }).SetName("CalcBracketFromBounds-BasicQuadratic");
            yield return new TestCaseData(new TestData()
            {
                Lower = -100,
                Upper = 100,
                Func = x => (x - 3) * (x - 3),
                ExpectedResult = new BracketMethod.ExtremumBracket(-100, -23.606797749982036, 100, 10609, 707.92168650844917, 9409)
            }).SetName("CalcBracketFromBounds-QuadraticCenteredAtThree");
        }

        [TestCaseSource(nameof(BracketFromBoundsTestData))]
        [Test]
        public void CalcBracketFromBounds(TestData testData)
        {
            var actualResult = BracketMethod.BracketFromBounds(testData.Lower, testData.Upper, testData.Func, 1000, 2, 2);
            Assert.That(actualResult, Is.Not.Null);
            Assert.That(actualResult.Failure, Is.False);
            Assert.That(actualResult.Data, Is.EqualTo(testData.ExpectedResult));
        }
    }
}
