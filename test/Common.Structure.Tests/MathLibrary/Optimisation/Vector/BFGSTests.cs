using System;

using Effanville.Common.Structure.MathLibrary.Optimisation.Vector;

using NUnit.Framework;

namespace Effanville.Common.Structure.Tests.MathLibrary.Optimisation.Vector
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

            Assert.That(Math.Abs(min.Data.Point[0] - function.GlobalMinimum.Point[0]), Is.LessThan(1e-4));
            Assert.That(Math.Abs(min.Data.Point[1] - function.GlobalMinimum.Point[1]), Is.LessThan(1e-4));
            Assert.That(Math.Abs(min.Data.Value - function.GlobalMinimum.Value), Is.LessThan(1e-4));
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

            Assert.That(Math.Abs(min.Data.Point[0] - function.GlobalMinimum.Point[0]), Is.LessThan(1e-4));
            Assert.That(Math.Abs(min.Data.Point[1] - function.GlobalMinimum.Point[1]), Is.LessThan(1e-4));
            Assert.That(Math.Abs(min.Data.Value - function.GlobalMinimum.Value), Is.LessThan(1e-4));
        }

        [TestCase(new double[] { 1, 1 })]
        [TestCase(new double[] { 3, 2 })]
        public void CalcMin_Beale(double[] startingPoint)
        {
            var function = new BealeFunction();
            var min = BFGS.Minimise(
                startingPoint,
                gradientTolerance: 1e-5,
                point => function.Value(point),
                point => function.Gradient(point),
                tolerance: 1e-5,
                maxIterations: 1000);

            Assert.That(Math.Abs(min.Data.Point[0] - function.GlobalMinimum.Point[0]), Is.LessThan(1e-4));
            Assert.That(Math.Abs(min.Data.Point[1] - function.GlobalMinimum.Point[1]), Is.LessThan(1e-4));
            Assert.That(Math.Abs(min.Data.Value - function.GlobalMinimum.Value), Is.LessThan(1e-4));
        }

        [TestCase(new double[] { 1, 1 })]
        [TestCase(new double[] { 3, 2 })]
        public void CalcMin_Matyas(double[] startingPoint)
        {
            var function = new MatyasFunction();
            var min = BFGS.Minimise(
                startingPoint,
                gradientTolerance: 1e-5,
                point => function.Value(point),
                point => function.Gradient(point),
                tolerance: 1e-5,
                maxIterations: 1000);

            Assert.That(Math.Abs(min.Data.Point[0] - function.GlobalMinimum.Point[0]), Is.LessThan(1e-4));
            Assert.That(Math.Abs(min.Data.Point[1] - function.GlobalMinimum.Point[1]), Is.LessThan(1e-4));
            Assert.That(Math.Abs(min.Data.Value - function.GlobalMinimum.Value), Is.LessThan(1e-4));
        }

        [TestCase(new double[] { 2, 4 }, 3)]
        [TestCase(new double[] { -5, -5 }, 1)]
        [TestCase(new double[] { 3.12, 2.44 }, 0)]
        [TestCase(new double[] { -3.0, -3.0 }, 2)]
        public void CalcMin_Himmelblau(double[] startingPoint, int localMinIndex)
        {
            var function = new HimmelblauFunction();
            var min = BFGS.Minimise(
                startingPoint,
                gradientTolerance: 1e-5,
                point => function.Value(point),
                point => function.Gradient(point),
                tolerance: 1e-5,
                maxIterations: 1000);

            Assert.That(Math.Abs(min.Data.Point[0] - function.LocalMinima[localMinIndex].Point[0]), Is.LessThan(1e-4));
            Assert.That(Math.Abs(min.Data.Point[1] - function.LocalMinima[localMinIndex].Point[1]), Is.LessThan(1e-4));
            Assert.That(Math.Abs(min.Data.Value - function.LocalMinima[localMinIndex].Value), Is.LessThan(1e-4));
        }
    }
}
