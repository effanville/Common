using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;

namespace Effanville.Common.Structure.Reporting;

/// <summary>
/// Provides DI registration methods for loggers.
/// </summary>
public static class ServiceRegistration
{
    /// <summary>
    /// Add report logging and logging providers to the <see cref="IServiceCollection"/>
    /// </summary>
    public static ILoggingBuilder AddReportLogger(
        this ILoggingBuilder builder,
        Action<ReportSeverity, ReportType, string, string> reportAction = null)
    {
        builder.AddConfiguration();

        builder.Services.AddReporting(reportAction);
        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, InjectableReportLoggerProvider>());

        LoggerProviderOptions.RegisterProviderOptions<ReportLoggerConfiguration, InjectableReportLoggerProvider>(builder.Services);

        return builder;
    }

    /// <summary>
    /// Add report logging and logging providers with configuration to the <see cref="IServiceCollection"/>
    /// </summary>
    public static ILoggingBuilder AddReportLogger(
        this ILoggingBuilder builder,
        Action<ReportLoggerConfiguration> configure,
        Action<ReportSeverity, ReportType, string, string> reportAction = null)
    {
        builder.AddReportLogger(reportAction);
        builder.Services.Configure(configure);
        return builder;
    }
    
    /// <summary>
    /// Add report logging to the <see cref="IServiceCollection"/>.
    /// </summary>
    public static IServiceCollection AddReporting(
        this IServiceCollection serviceCollection,
        Action<ReportSeverity, ReportType, string, string> reportAction = null)
        => serviceCollection.AddSingleton<IReportLogger, LogReporter>(_ =>
            new LogReporter(reportAction, saveInternally: true));
}