namespace Effanville.Common.Structure.Reporting
{
    /// <summary>
    /// Builds a report.
    /// </summary>
    internal class ReportBuilder : IReport
    {
        IReportLogger _logger;
        ReportSeverity _severity;
        ReportType _type;
        string _location;
        public ReportBuilder(IReportLogger logger, ReportSeverity reportSeverity)
        {
            _logger = logger;
            _severity = reportSeverity;
        }

        public IReport Type(ReportType type)
        {
            _type = type;
            return this;
        }

        public IReport Location(string location)
        {
            _location = location;
            return this;
        }

        public void WithMessage(string message)
        {
            _logger.Log(_severity, _type, _location, message);
        }
    }
}
