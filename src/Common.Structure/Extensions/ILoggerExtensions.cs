using System;
using System.Runtime.CompilerServices;

using Microsoft.Extensions.Logging;

namespace Effanville.Common.Structure.Extensions;

/// <summary>
/// Containes extension methods for an <see cref="ILogger"/>.
/// </summary>
public static class ILoggerExtensions
{
    /// <summary>
    /// Logs an Error report for the exception.
    /// </summary>
    /// <param name="logger">The logger to use</param>
    /// <param name="exception">The exception to report</param>
    /// <param name="location">The location the report pertains to, defaults to <see cref="CallerMemberNameAttribute"/>.</param>
    public static void Exception(this ILogger logger, Exception exception, [CallerMemberName] string location = null)
        => logger.LogError("{0}. {2}", location, exception);

    /// <summary>
    /// Logs an Error report for the exception.
    /// </summary>
    /// <param name="logger">The logger to use</param>
    /// <param name="message">The message specifying more information about the report.</param>
    /// <param name="exception">The exception to report</param>
    /// <param name="location">The location the report pertains to, defaults to <see cref="CallerMemberNameAttribute"/>.</param>
    public static void Exception(this ILogger logger, string message, Exception exception, [CallerMemberName] string location = null)
        => logger.LogError("{0}. {1}. {2}", location, message, exception);

    /// <summary>
    /// Logs an Error.
    /// </summary>
    /// <param name="logger">The logger to use</param>
    /// <param name="message">The message specifying more information about the report.</param>
    /// <param name="location">The location the report pertains to, defaults to <see cref="CallerMemberNameAttribute"/>.</param>
    public static void Error(this ILogger logger, string message, [CallerMemberName] string location = null)
        => logger.LogError("{0}. {1}", location, message);

    /// <summary>
    /// Logs a Warning
    /// </summary>
    /// <param name="logger">The logger to use</param>
    /// <param name="message">The message specifying more information about the report.</param>
    /// <param name="location">The location the report pertains to, defaults to <see cref="CallerMemberNameAttribute"/>.</param>
    public static void Warn(this ILogger logger, string message, [CallerMemberName] string location = null)
        => logger.LogWarning("{0}. {1}", location, message);

    /// <summary>
    /// Logs an Info report.
    /// </summary>
    /// <param name="logger">The logger to use</param>
    /// <param name="message">The message specifying more information about the report.</param>
    /// <param name="location">The location the report pertains to, defaults to <see cref="CallerMemberNameAttribute"/>.</param>
    public static void Info(this ILogger logger, string message, [CallerMemberName] string location = null)
        => logger.LogInformation("{0}. {1}", location, message);

    /// <summary>
    /// Logs a Debug report
    /// </summary>
    /// <param name="logger">The logger to use</param>
    /// <param name="message">The message specifying more information about the report.</param>
    /// <param name="location">The location the report pertains to, defaults to <see cref="CallerMemberNameAttribute"/>.</param>
    public static void Debug(this ILogger logger, string message, [CallerMemberName] string location = null)
        => logger.LogDebug("{0}. {1}", location, message);
}
