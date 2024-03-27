using System;

using Microsoft.Extensions.Logging;

namespace Effanville.Common.Structure.Reporting;

public sealed class InjectableReportLogger : ILogger
{
    private readonly string _name;
    private readonly IReportLogger _internalLogger;

    public InjectableReportLogger(string name, IReportLogger internalLogger)
    {
        _name = name;
        _internalLogger = internalLogger;
    }

    internal IReportLogger GetInternalLogger()
        => _internalLogger;

    /// <inheritdoc />
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
        Func<TState, Exception, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }

        ReportType reportType = logLevel switch
        {
            LogLevel.Critical => ReportType.Error,
            LogLevel.Error => ReportType.Error,
            LogLevel.Warning => ReportType.Warning,
            LogLevel.Information => ReportType.Information,
            LogLevel.Debug => ReportType.Information,
            LogLevel.Trace => ReportType.Information,
            LogLevel.None => ReportType.Information,
            _ => ReportType.Error
        };

        _internalLogger.Log(reportType, location: _name, formatter(state, exception));
    }

    /// <inheritdoc />
    public bool IsEnabled(LogLevel logLevel) => true;

    /// <inheritdoc />
    public IDisposable BeginScope<TState>(TState state) where TState : notnull => default!;
}