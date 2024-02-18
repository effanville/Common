using System;
using System.IO.Abstractions;

namespace Common.Structure.Reporting
{
    /// <summary>
    /// Report Logger contract. Allows for reporting using types or strings.
    /// </summary>
    public interface IReportLogger
    {
        /// <summary>
        /// Return a Report constructor with <see cref="ReportSeverity.Critical"/>.
        /// </summary>
        IReport Critical();

        /// <summary>
        /// The store of reports logged by the report logger.
        /// </summary>
        ErrorReports Reports
        {
            get;
        }

        /// <summary>
        /// Set whether the logger stores an internal record of the report.
        /// </summary>
        bool SaveInternally
        {
            get; set;
        }

        /// <summary>
        /// Logs a <see cref="ReportSeverity.Useful"/> report using the type enums.
        /// </summary>
        /// <param name="type">The type of report being logged.</param>
        /// <param name="location">The location the report pertains to.</param>
        /// <param name="message">The message specifying more information about the report.</param>
        void Log(ReportType type, string location, string message);

        /// <summary>
        /// Logs a report using the type enums.
        /// </summary>
        /// <param name="severity">The seriousness of the report being logged.</param>
        /// <param name="type">The type of report being logged.</param>
        /// <param name="location">The location the report pertains to.</param>
        /// <param name="message">The message specifying more information about the report.</param>
        void Log(ReportSeverity severity, ReportType type, string location, string message);

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
        [Obsolete("User should use the log method instead.")]
        bool LogUseful(ReportType type, ReportLocation location, string message);

        /// <summary>
        /// Logs an <see cref="ReportType.Error"/> report with severity <see cref="ReportSeverity.Useful"/> using
        /// the other type enums.
        /// </summary>
        /// <param name="location">The location the report pertains to.</param>
        /// <param name="message">The message specifying more information about the report.</param>
        [Obsolete("User should use the Error method instead.")]
        bool LogUsefulError(ReportLocation location, string message);

        /// <summary>
        /// Logs an <see cref="ReportType.Error"/> report with severity <see cref="ReportSeverity.Useful"/>.
        /// </summary>
        /// <param name="location">The location the report pertains to.</param>
        /// <param name="message">The message specifying more information about the report.</param>
        void Error(string location, string message);

        /// <summary>
        /// Logs an <see cref="ReportType.Warning"/> report with severity <see cref="ReportSeverity.Useful"/>.
        /// </summary>
        /// <param name="location">The location the report pertains to.</param>
        /// <param name="message">The message specifying more information about the report.</param>
        void Warning(string location, string message);

        /// <summary>
        /// Write the reports to a suitable file.
        /// </summary>
		void WriteReportsToFile(string filePath);

        /// <summary>
        /// Write the reports to a suitable file.
        /// </summary>
        void WriteReportsToFile(string filePath, out string error);

        /// <summary>
        /// Write the reports to a suitable file.
        /// </summary>
		void WriteReportsToFile(string filePath, IFileSystem fileSystem);

        /// <summary>
        /// Write the reports to a suitable file.
        /// </summary>
        void WriteReportsToFile(string filePath, IFileSystem fileSystem, out string error);
    }
}
