﻿using System.Linq;

using Effanville.Common.Structure.Reporting;

using NUnit.Framework;

namespace Effanville.Common.Structure.Tests.Reporting
{
    [TestFixture]
    public sealed class ErrorReportsTests
    {
        [Test]
        public void AccessAfterClearingTest()
        {
            ErrorReports reports = new ErrorReports();
            string errorString = "some problem";
            reports.AddErrorReport(ReportSeverity.Useful, ReportType.Error, ReportLocation.Unknown, errorString);

            var shallowCopy = reports.GetReports();
            Assert.AreEqual(1, shallowCopy.Count);

            reports.Clear();

            Assert.AreEqual(1, shallowCopy.Count);
            var onlyOne = shallowCopy.Single();
            Assert.AreEqual(errorString, onlyOne.Message);
        }
    }
}