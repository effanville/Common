using System.Runtime.CompilerServices;

namespace Effanville.Common.Structure.Reporting;

/// <summary>
/// Extension methods for a <see cref="IReportLogger"/>
/// </summary>
public static class IReportLoggerExtensions
{
    /// <summary>
    /// Logs an <see cref="ReportType.Debug"/> report with severity <see cref="ReportSeverity.Useful"/>.
    /// </summary>
    /// <param name="logger">The logger to use</param>
    /// <param name="name">The name to associate to the logger</param>
    /// <param name="message">The message specifying more information about the report.</param>
    /// <param name="location">The location the report pertains to, defaults to <see cref="CallerMemberNameAttribute"/>.</param>
    public static void Debug(this IReportLogger logger, string name, string message, [CallerMemberName] string location = null)
        => logger.Log(ReportType.Debug, name, $"{location}. {message}");
}
