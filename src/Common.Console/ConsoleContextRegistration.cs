using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Reflection;

using Effanville.Common.Structure.Reporting;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Effanville.Common.Console;

/// <summary>
/// Contains extension methods to register the necessary services for the <see cref="ConsoleContext"/> to run.
/// </summary>
public static class ConsoleContextRegistration
{        
    /// <summary>
    /// Setup the given host with command line configuration and console context from the given command types.
    /// </summary>
    public static HostApplicationBuilder SetupConsole(
        this HostApplicationBuilder hostApplicationBuilder,
        string[] args,
        IEnumerable<Type> consoleCommandTypes)
    {
        hostApplicationBuilder.Logging
            .ClearProviders()
            .AddReporting(config => config.MinimumLogLevel = ReportType.Information);
        hostApplicationBuilder.Services.AddConsoleContext(consoleCommandTypes);
        hostApplicationBuilder.Configuration.AddConfiguration(args);
        return hostApplicationBuilder;
    }
    
    /// <summary>
    /// Register the command line args as configuration in the builder.
    /// </summary>
    public static IConfigurationBuilder AddConfiguration(
        this IConfigurationBuilder builder,
        string[] args)
    {
        string executingAssemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        string appSettingsFilePath = Path.Combine(executingAssemblyLocation, "appsettings.json");
        var configuration = new ConsoleCommandArgs(args);
        builder.AddCommandLine(configuration.GetEffectiveArgs())
            .AddJsonFile(appSettingsFilePath)
            .AddEnvironmentVariables();
        return builder;
    }

    /// <summary>
    /// Register the <see cref="ConsoleContext"/> in the container,
    /// as well as all relevant types to use.
    /// </summary>
    public static IServiceCollection AddConsoleContext(
        this IServiceCollection serviceCollection,
        IEnumerable<Type> consoleCommandTypes)
    {
        serviceCollection.AddSingleton<IFileSystem, FileSystem>();
        serviceCollection.AddConsoleCommands(consoleCommandTypes);
        serviceCollection.AddSingleton<IConsoleContext, ConsoleContext>();
        serviceCollection.AddHostedService<ConsoleHost>();
        return serviceCollection;
    }
}