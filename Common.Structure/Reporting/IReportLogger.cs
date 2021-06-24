using System.IO.Abstractions;

namespace Common.Structure.Reporting
{
    /// <summary>
    /// Report Logger contract. Allows for reporting using types or strings.
    /// </summary>
    public interface IReportLogger
    {
        ErrorReports Reports
        {
            get;
        }

        /// <summary>
        /// Logs a report using the type enums.
        /// </summary>
        /// <param name="severity">The seriousness of the report being logged.</param>
        /// <param name="type">The type of report being logged.</param>
        /// <param name="location">The location the report pertains to.</param>
        /// <param name="message">The message specifying more information about the report.</param>
        bool Log(ReportSeverity severity, ReportType type, ReportLocation location, string message);

        /// <summary>
        /// Logs a report with severity <see cref="ReportSeverity.Useful"/> using the other type enums.
        /// </summary>
        /// <param name="type">The type of report being logged.</param>
        /// <param name="location">The location the report pertains to.</param>
        /// <param name="message">The message specifying more information about the report.</param>
        bool LogUseful(ReportType type, ReportLocation location, string message);

        /// <summary>
        /// Logs an <see cref="ReportType.Error"/> report with severity <see cref="ReportSeverity.Useful"/> using
        /// the other type enums.
        /// </summary>
        /// <param name="location">The location the report pertains to.</param>
        /// <param name="message">The message specifying more information about the report.</param>
        bool LogUsefulError(ReportLocation location, string message);

        /// <summary>
        /// Write the reports to a suitable file.
        /// </summary>
		void WriteReportsToFile(string filePath);

        /// <summary>
        /// Write the reports to a suitable file.
        /// </summary>
		void WriteReportsToFile(string filePath, IFileSystem fileSystem);
    }
}
