using System;

using Common.Structure.MathLibrary.Optimisation.Vector;

using NUnit.Framework;

namespace Common.Structure.Tests.MathLibrary.Optimisation.Vector
{
    [TestFixture]
    public sealed class BFGSTests
    {
        [TestCase(new double[] { 1.2, 1.2 })]
        [TestCase(new double[] { -1.2, 1.0 })]
        [TestCase(new double[] { -0.9, -0.5 })]
        [TestCase(new double[] { 10, 10 })]
        public void CalcMin_Rosenbrock(double[] startingPoint)
        {
            var function = new RosenbrockFunction();
            var min = BFGS.Minimise(
                startingPoint,
                gradientTolerance: 1e-5,
                point => function.Value(point),
                point => function.Gradient(point),
                tolerance: 1e-5,
                maxIterations: 1000);

            Assert.That(Math.Abs(min.MinimisingPoint[0] - function.GlobalMinimum[0]), Is.LessThan(1e-4));
            Assert.That(Math.Abs(min.MinimisingPoint[1] - function.GlobalMinimum[1]), Is.LessThan(1e-4));
        }

        [TestCase(new double[] { 120, 120 })]
        [TestCase(new double[] { -120, 100 })]
        [TestCase(new double[] { -90, -50 })]
        [TestCase(new double[] { 1000, 1000 })]
        public void CalcMin_BigRosenbrock(double[] startingPoint)
        {
            var function = new BigRosenbrockFunction();
            var min = BFGS.Minimise(
                startingPoint,
                gradientTolerance: 1e-5,
                point => function.Value(point),
                point => function.Gradient(point),
                tolerance: 1e-5,
                maxIterations: 1000);

            Assert.That(Math.Abs(min.MinimisingPoint[0] - function.GlobalMinimum[0]), Is.LessThan(1e-4));
            Assert.That(Math.Abs(min.MinimisingPoint[1] - function.GlobalMinimum[1]), Is.LessThan(1e-4));
        }
    }
}
