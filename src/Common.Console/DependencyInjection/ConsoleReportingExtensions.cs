using System;

using Effanville.Common.Structure.Reporting;

using Microsoft.Extensions.Logging;

namespace Effanville.Common.Console.DependencyInjection;

/// <summary>
/// Registration extensions for an <see cref="IReportLogger"/> and for console logging.
/// </summary>
public static class ConsoleReportingExtensions
{
    /// <summary>
    /// Add console and ReportLogger logging to the <see cref="ILoggingBuilder"/>.
    /// </summary>
    public static ILoggingBuilder AddReporting(
        this ILoggingBuilder builder,
        Action<ReportLoggerConfiguration> configure = null)
        => builder.AddReportLogger(configure, ReportAction);

    private static void ReportAction(ReportSeverity severity, ReportType reportType, string location, string text)
    {
        string message = $"{DateTime.Now:yyyy-MM-ddTHH:mm:ss.fff} | {reportType.ToLogString()} | {location,-15} | {text}";
        if (reportType == ReportType.Error)
        {
            WriteError(message);
        }
        else
        {
            WriteLine(message);
        }
    }

    private static void WriteError(string text)
    {
        var color = System.Console.ForegroundColor;
        System.Console.ForegroundColor = ConsoleColor.Red;
        System.Console.WriteLine(text);
        System.Console.ForegroundColor = color;
    }

    private static void WriteLine(string text) => System.Console.WriteLine(text);
}