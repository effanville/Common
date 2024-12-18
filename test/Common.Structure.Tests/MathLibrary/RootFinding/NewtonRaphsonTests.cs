﻿using System;

using Effanville.Common.Structure.MathLibrary.RootFinding;

using NUnit.Framework;

namespace Effanville.Common.Structure.Tests.MathLibrary.RootFinding
{
    [TestFixture]
    public sealed class NewtonRaphsonTests
    {
        [Test]
        public void FindRoot()
        {
            var bisectionResult = RootFinder.NewtonRaphson.FindRoot(x => (x - 2) * (x - 4), x => 2 * x - 4, 0, 3);
            Assert.That(bisectionResult.Failure, Is.False);
            Assert.That(Math.Abs(bisectionResult.Data - 2), Is.LessThan(1e-8));
        }

        [Test]
        public void FindRootUnsafe()
        {
            var bisectionResult = RootFinder.NewtonRaphson.FindRootUnsafe(x => (x - 2) * (x - 4), x => 2 * x - 4, 0, 3);
            Assert.That(bisectionResult.Failure, Is.False);
            Assert.That(Math.Abs(bisectionResult.Data - 2), Is.LessThan(1e-8));
        }
    }
}
