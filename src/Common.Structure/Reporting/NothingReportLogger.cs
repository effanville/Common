using System.IO.Abstractions;

namespace Common.Structure.Reporting
{
    /// <summary>
    /// Report logger that does nothing, but declares successfully reported.
    /// </summary>
    public class NothingReportLogger : IReportLogger
    {
        /// <inheritdoc/>
        public ErrorReports Reports
        {
            get;
        }

        bool IReportLogger.SaveInternally
        {
            get; set;
        }

        /// <inheritdoc/>
        public IReport Critical()
        {
            return null;
        }

        /// <inheritdoc/>
        public bool Log(ReportSeverity severity, ReportType type, ReportLocation location, string message)
        {
            return true;
        }

        /// <inheritdoc/>
        public bool LogUseful(ReportType type, ReportLocation location, string message)
        {
            return true;
        }

        /// <inheritdoc/>
        public bool LogUsefulError(ReportLocation location, string message)
        {
            return true;
        }

        /// <inheritdoc/>
        public void WriteReportsToFile(string filePath)
        {
        }

        /// <inheritdoc/>
        public void WriteReportsToFile(string filePath, out string error)
        {
            error = null;
        }

        /// <inheritdoc/>
        public void WriteReportsToFile(string filePath, IFileSystem fileSystem)
        {
        }

        /// <inheritdoc/>
		public void WriteReportsToFile(string filePath, IFileSystem fileSystem, out string error)
        {
            error = null;
        }

        /// <inheritdoc/>
        public void Error(string location, string message)
        {
        }

        /// <inheritdoc/>
        public void Log(ReportType type, string location, string message)
        {
        }

        /// <inheritdoc/>
        public void Log(ReportSeverity severity, ReportType type, string location, string message)
        {
        }

        /// <inheritdoc/>
        public void Warning(string location, string message)
        {
        }
    }
}
