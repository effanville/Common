using System.IO.Abstractions;

namespace Effanville.Common.Structure.Reporting;

/// <summary>
/// Report logger that does nothing, but declares successfully reported.
/// </summary>
public class NothingReportLogger : IReportLogger
{
    /// <inheritdoc/>
    public ErrorReports Reports { get; }

    bool IReportLogger.SaveInternally { get; set; }

    /// <inheritdoc/>
	public void WriteReportsToFile(string filePath, IFileSystem fileSystem, out string error) => error = null;

    /// <inheritdoc/>
    public void Log(ReportType type, string location, string message)
    {
    }
}
