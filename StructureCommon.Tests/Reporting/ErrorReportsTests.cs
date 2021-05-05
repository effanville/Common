using System.Linq;
using NUnit.Framework;
using StructureCommon.Reporting;

namespace StructureCommon.Tests.Reporting
{
    [TestFixture]
    public sealed class ErrorReportsTests
    {
        [Test]
        public void AccessAfterClearingTest()
        {
            ErrorReports reports = new ErrorReports();
            string errorString = "some problem";
            reports.AddError(errorString);

            var shallowCopy = reports.GetReports();
            Assert.AreEqual(1, shallowCopy.Count);

            reports.Clear();

            Assert.AreEqual(1, shallowCopy.Count);
            var onlyOne = shallowCopy.Single();
            Assert.AreEqual(errorString, onlyOne.Message);
        }
    }
}
