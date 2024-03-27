using System;

using Effanville.Common.Structure.Extensions;

namespace Effanville.Common.Structure.Reporting;

/// <summary>
/// Configuration for a <see cref="IReportLogger"/> provider.
/// </summary>
public sealed class ReportLoggerConfiguration
{
    /// <summary>
    /// The minimum log level for the logger.
    /// </summary>
    public ReportType MinimumLogLevel { get; set; } = ReportType.Information;

    public string LogPersistenceFilePath => $"{DateTime.Now.FileSuitableDateTimeValue()}-consoleLog.log";
}