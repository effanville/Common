using System;
using System.IO.Abstractions;
using System.Runtime.CompilerServices;

namespace Effanville.Common.Structure.Reporting;

/// <summary>
/// Extension methods for a <see cref="IReportLogger"/>
/// </summary>
public static class IReportLoggerExtensions
{
    /// <summary>
    /// Logs an <see cref="ReportType.Error"/> report with severity <see cref="ReportSeverity.Useful"/> for the exception.
    /// </summary>
    /// <param name="logger">The logger to use</param>
    /// <param name="name">The name to associate to the logger</param>
    /// <param name="exception">The exception to report</param>
    /// <param name="location">The location the report pertains to, defaults to <see cref="CallerMemberNameAttribute"/>.</param>
    public static void Exception(this IReportLogger logger, string name, Exception exception, [CallerMemberName] string location = null)
        => logger.Log(ReportType.Error, name, $"{location}. {exception}");

    /// <summary>
    /// Logs an <see cref="ReportType.Error"/> report with severity <see cref="ReportSeverity.Useful"/> for the exception.
    /// </summary>
    /// <param name="logger">The logger to use</param>
    /// <param name="name">The name to associate to the logger</param>
    /// <param name="message">The message specifying more information about the report.</param>
    /// <param name="exception">The exception to report</param>
    /// <param name="location">The location the report pertains to, defaults to <see cref="CallerMemberNameAttribute"/>.</param>
    public static void Exception(this IReportLogger logger, string name, string message, Exception exception, [CallerMemberName] string location = null)
        => logger.Log(ReportType.Error, name, !string.IsNullOrEmpty(message) ? $"{location}. {message}. {exception}" : $"{location}. {exception}");

    /// <summary>
    /// Logs an <see cref="ReportType.Error"/> report with severity <see cref="ReportSeverity.Useful"/>.
    /// </summary>
    /// <param name="logger">The logger to use</param>
    /// <param name="name">The name to associate to the logger</param>
    /// <param name="message">The message specifying more information about the report.</param>
    /// <param name="location">The location the report pertains to, defaults to <see cref="CallerMemberNameAttribute"/>.</param>
    public static void Error(this IReportLogger logger, string name, string message, [CallerMemberName] string location = null)
        => logger.Log(ReportType.Error, name, $"{location}. {message}");

    /// <summary>
    /// Logs an <see cref="ReportType.Warning"/> report with severity <see cref="ReportSeverity.Useful"/>.
    /// </summary>
    /// <param name="logger">The logger to use</param>
    /// <param name="name">The name to associate to the logger</param>
    /// <param name="message">The message specifying more information about the report.</param>
    /// <param name="location">The location the report pertains to, defaults to <see cref="CallerMemberNameAttribute"/>.</param>
    public static void Warn(this IReportLogger logger, string name, string message, [CallerMemberName] string location = null)
        => logger.Log(ReportType.Warning, name, $"{location}. {message}");

    /// <summary>
    /// Logs an <see cref="ReportType.Information"/> report with severity <see cref="ReportSeverity.Useful"/>.
    /// </summary>
    /// <param name="logger">The logger to use</param>
    /// <param name="message">The message specifying more information about the report.</param>
    /// <param name="location">The location the report pertains to, defaults to <see cref="CallerMemberNameAttribute"/>.</param>
    public static void Info(this IReportLogger logger, string message, [CallerMemberName] string location = null)
        => logger.Log(ReportType.Information, location, message);

    /// <summary>
    /// Logs an <see cref="ReportType.Information"/> report with severity <see cref="ReportSeverity.Useful"/>.
    /// </summary>
    /// <param name="logger">The logger to use</param>
    /// <param name="name">The name to associate to the logger</param>
    /// <param name="message">The message specifying more information about the report.</param>
    /// <param name="location">The location the report pertains to, defaults to <see cref="CallerMemberNameAttribute"/>.</param>
    public static void Info(this IReportLogger logger, string name, string message, [CallerMemberName] string location = null)
        => logger.Log(ReportType.Information, name, $"{location}. {message}");

    /// <summary>
    /// Logs an <see cref="ReportType.Debug"/> report with severity <see cref="ReportSeverity.Useful"/>.
    /// </summary>
    /// <param name="logger">The logger to use</param>
    /// <param name="name">The name to associate to the logger</param>
    /// <param name="message">The message specifying more information about the report.</param>
    /// <param name="location">The location the report pertains to, defaults to <see cref="CallerMemberNameAttribute"/>.</param>
    public static void Debug(this IReportLogger logger, string name, string message, [CallerMemberName] string location = null)
        => logger.Log(ReportType.Debug, name, $"{location}. {message}");

    /// <summary>
    /// Write the reports to a suitable file.
    /// </summary>
    public static void WriteReportsToFile(this IReportLogger logger, string filePath)
        => logger.WriteReportsToFile(filePath, new FileSystem(), out _);

    /// <summary>
    /// Write the reports to a suitable file.
    /// </summary>
    public static void WriteReportsToFile(this IReportLogger logger, string filePath, out string message)
        => logger.WriteReportsToFile(filePath, new FileSystem(), out message);

    /// <summary>
    /// Write the reports to a suitable file.
    /// </summary>
    public static void WriteReportsToFile(this IReportLogger logger, string filePath, IFileSystem fileSystem)
        => logger.WriteReportsToFile(filePath, fileSystem, out _);
}
