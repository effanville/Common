using System;

using Common.Structure.MathLibrary.RootFinding;

using NUnit.Framework;

namespace Effanville.Common.Structure.Tests.MathLibrary.RootFinding
{
    [TestFixture]
    public sealed class RootBracketTests
    {
        [Test]
        public void FindRootUsingBisection()
        {
            var bisectionResult = RootFinder.FindBracketForRoot(0, 3, x => (x - 2) * (x - 4));
            Assert.IsFalse(bisectionResult.Failure);
            Assert.That(Math.Abs(bisectionResult.Data.LowerBound - 0), Is.LessThan(1e-8));
            Assert.That(Math.Abs(bisectionResult.Data.UpperBound - 3), Is.LessThan(1e-8));
        }
    }
}
