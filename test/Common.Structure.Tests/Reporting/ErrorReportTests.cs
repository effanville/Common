using System;

using Effanville.Common.Structure.Reporting;

using NUnit.Framework;

namespace Effanville.Common.Structure.Tests.Reporting
{
    [TestFixture]
    public sealed class ErrorReportTests
    {
        [Test]
        public void ToStringTests()
        {
            var report = new ErrorReport();
            var time = DateTime.Now;
            report.TimeStamp = time;
            Assert.That(report.ToString(), Is.EqualTo($"[{time:yyyy-MM-ddTHH:mm:ss}] [Error] [Unknown] "));
        }

        [Test]
        public void ComparisonTests()
        {
            var report = new ErrorReport();
            var otherReport = new ErrorReport();
            var time = DateTime.Now;
            report.TimeStamp = time;
            Assert.That(report.CompareTo(otherReport), Is.EqualTo(0));
        }
    }
}
