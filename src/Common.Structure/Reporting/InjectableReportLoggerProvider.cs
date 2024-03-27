using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Effanville.Common.Structure.Reporting;

[ProviderAlias("ReportLogger")]
public sealed class InjectableReportLoggerProvider : ILoggerProvider
{
    private readonly IOptionsMonitor<ReportLoggerConfiguration> _config;
    private readonly IReportLogger _internalLogger;

    /// <summary>
    /// Create a logger provider for the DI framework.
    /// </summary>
    public InjectableReportLoggerProvider(IOptionsMonitor<ReportLoggerConfiguration> config, IReportLogger internalLogger)
    {
        _config = config;
        _internalLogger = internalLogger;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        string persistenceFilePath = _config.CurrentValue.LogPersistenceFilePath;
        _internalLogger.WriteReportsToFile(persistenceFilePath);
    }

    /// <inheritdoc />
    public ILogger CreateLogger(string categoryName) => new InjectableReportLogger(categoryName, _internalLogger);
}