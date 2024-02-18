using System;
using NUnit.Framework;
using Common.Structure.Reporting;

namespace Common.Structure.Tests.Reporting
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
            Assert.AreEqual($"[{time}]-(Error) - [Unknown] - ", report.ToString());
        }

        [Test]
        public void ComparisonTests()
        {
            var report = new ErrorReport();
            var otherReport = new ErrorReport();
            var time = DateTime.Now;
            report.TimeStamp = time;
            Assert.AreEqual(0, report.CompareTo(otherReport));
        }
    }
}
