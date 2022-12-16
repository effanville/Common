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
            var bisectionResult = RootFinder.NewtonRaphson.FindRoot(x => (x - 2) * (x - 4), x => 2 * x - 4, 0, 3);
            Assert.IsFalse(bisectionResult.IsError());
            Assert.That(Math.Abs(bisectionResult.Value - 2), Is.LessThan(1e-8));
        }

        [Test]
        public void FindRootUnsafe()
        {
            var bisectionResult = RootFinder.NewtonRaphson.FindRootUnsafe(x => (x - 2) * (x - 4), x => 2 * x - 4, 0, 3);
            Assert.IsFalse(bisectionResult.IsError());
            Assert.That(Math.Abs(bisectionResult.Value - 2), Is.LessThan(1e-8));
        }
    }
}
