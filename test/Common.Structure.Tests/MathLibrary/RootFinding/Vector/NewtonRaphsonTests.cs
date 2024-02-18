using System;

using Common.Structure.MathLibrary.RootFinding;

using NUnit.Framework;

namespace Common.Structure.Tests.MathLibrary.RootFinding.Vector
{
    [TestFixture]
    internal sealed class NewtonRaphsonTests
    {
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
            var bisectionResult = RootFinder.Vector.NewtonRaphson.FindRoot(func, x => derivative(x), new double[] { 1, 2 });
            Assert.IsFalse(bisectionResult.Failure);
            double[] value = bisectionResult.Data;
            Assert.That(Math.Abs(value[0] - 2), Is.LessThan(1e-8));
            Assert.That(Math.Abs(value[1] - 3), Is.LessThan(1e-8));
        }
    }
}
