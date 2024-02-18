using System;

using Common.Structure.Reporting;

using Microsoft.Extensions.DependencyInjection;

namespace Effanville.Common.Console;

/// <summary>
/// Registration extensions for console logging and an <see cref="IConsole"/> instance.
/// </summary>
public static class ConsoleInstanceRegistration
{
    /// <summary>
    /// Add console logging and report logging to the <see cref="IServiceCollection"/>.
    /// </summary>
    public static IServiceCollection AddReporting(
        this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IReportLogger, LogReporter>(_ =>
            new LogReporter(ReportAction, saveInternally: true));
        return serviceCollection.AddSingleton<IConsole, ConsoleInstance>(
            _ => new ConsoleInstance(WriteError, WriteLine));
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
        string message = $"[{DateTime.Now}] [{reportType.ToLogString()}] [{location}] {text}";
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