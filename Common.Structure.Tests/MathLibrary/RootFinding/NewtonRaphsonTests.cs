using System;

using Common.Structure.MathLibrary.RootFinding;

using NUnit.Framework;

namespace Common.Structure.Tests.MathLibrary.RootFinding
{
    [TestFixture]
    public sealed class NewtonRaphsonTests
    {
        [Test]
        public void FindRoot()
        {
            var bisectionResult = NewtonRaphsonMethod.FindRootSafe(x => (x - 2) * (x - 4), x => 2 * x - 4, 0, 3);
            Assert.IsFalse(bisectionResult.IsError());
            Assert.That(Math.Abs(bisectionResult.Value - 2), Is.LessThan(1e-8));
        }

        [Test]
        public void FindRootUnsafe()
        {
            var bisectionResult = NewtonRaphsonMethod.FindRootUnsafe(x => (x - 2) * (x - 4), x => 2 * x - 4, 0, 3);
            Assert.IsFalse(bisectionResult.IsError());
            Assert.That(Math.Abs(bisectionResult.Value - 2), Is.LessThan(1e-8));
        }


        [Test]
        public void FindRootMulti()
        {
            double[] func(double[] x) => new double[] { (x[0] - 2) * (x[0] - 4), (x[1] - 3) * (x[1] - 5) };
            double[,] derivative(double[] input)
            {
                double[,] result = new double[input.Length, input.Length];
                result[0, 0] = 2 * input[0] - 6;
                result[1, 1] = 2 * input[1] - 8;

                return result;
            }
            var bisectionResult = NewtonRaphsonMethod.FindMultiDimRoot(func, x => derivative(x), new double[] { 1, 2 });
            Assert.IsFalse(bisectionResult.IsError());
            double[] value = bisectionResult.Value;
            Assert.That(Math.Abs(value[0] - 2), Is.LessThan(1e-8));
            Assert.That(Math.Abs(value[1] - 3), Is.LessThan(1e-8));
        }
    }
}
