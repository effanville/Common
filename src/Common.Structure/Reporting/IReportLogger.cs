using System.IO.Abstractions;

namespace Effanville.Common.Structure.Reporting;

/// <summary>
/// Report Logger contract. Allows for reporting using types or strings.
/// </summary>
public interface IReportLogger
{
    /// <summary>
    /// The store of reports logged by the report logger.
    /// </summary>
    ErrorReports Reports { get; }

    /// <summary>
    /// Set whether the logger stores an internal record of the report.
    /// </summary>
    bool SaveInternally { get; set; }

    /// <summary>
    /// Logs a <see cref="ReportSeverity.Useful"/> report using the type enums.
    /// </summary>
    /// <param name="type">The type of report being logged.</param>
    /// <param name="location">The location the report pertains to.</param>
    /// <param name="message">The message specifying more information about the report.</param>
    void Log(ReportType type, string location, string message);

    /// <summary>
    /// Write the reports to a suitable file.
    /// </summary>
    void WriteReportsToFile(string filePath, IFileSystem fileSystem, out string error);
}
