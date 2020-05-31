namespace StructureCommon.Reporting
{
    /// <summary>
    /// Report logger that does nothing, but declares successfully reported.
    /// </summary>
    public class NothingReportLogger : IReportLogger
    {
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
        public bool LogUsefulErrorWithStrings(string location, string message)
        {
            return true;
        }

        /// <inheritdoc/>
        public bool LogUsefulWithStrings(string type, string location, string message)
        {
            return true;
        }

        /// <inheritdoc/>
        public bool LogWithStrings(string severity, string type, string location, string message)
        {
            return true;
        }
    }
}
