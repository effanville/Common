using System;

using Effanville.Common.Structure.Reporting;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Effanville.Common.Console;

/// <summary>
/// Registration extensions for console logging and an <see cref="IConsole"/> instance.
/// </summary>
public static class ConsoleInstanceRegistration
{    
    /// <summary>
    /// Add console and ReportLogger logging to the <see cref="ILoggingBuilder"/>.
    /// </summary>
    public static ILoggingBuilder AddReporting(
        this ILoggingBuilder builder,
        Action<ReportLoggerConfiguration> configure = null)
    {
        builder.AddReportLogger(configure, ReportAction);
        return builder;
    }

    private static void WriteError(string text)
    {
        var color = System.Console.ForegroundColor;
        System.Console.ForegroundColor = ConsoleColor.Red;
        System.Console.WriteLine(text);
        System.Console.ForegroundColor = color;
    }

    private static void WriteLine(string text) => System.Console.WriteLine(text);

    private static void ReportAction(ReportSeverity severity, ReportType reportType, string location, string text)
    {
        string message = $"[{DateTime.Now:yyyyMMddTHH:mm:ss.fff}] [{reportType.ToLogString()}] [{location}] {text}";
        if (reportType == ReportType.Error)
        {
            WriteError(message);
        }
        else
        {
            WriteLine(message);
        }
    }
}