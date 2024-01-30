using System;
using System.Collections.Generic;
using System.IO.Abstractions;

using Microsoft.Extensions.DependencyInjection;

namespace Effanville.Common.Console;

/// <summary>
/// Contains extension methods to register the necessary services for the <see cref="ConsoleContext"/> to run.
/// </summary>
public static class ConsoleContextRegistration
{
    /// <summary>
    /// Register the <see cref="ConsoleContext"/> in the container,
    /// as well as all relevant types to use.
    /// </summary>
    public static IServiceCollection AddConsoleContext(
        this IServiceCollection serviceCollection,
        IEnumerable<Type> consoleCommandTypes,
        string[] args)
    {
        serviceCollection.AddSingleton<IFileSystem, FileSystem>();
        serviceCollection.AddReporting();
        serviceCollection.AddConsoleCommands(consoleCommandTypes);
        serviceCollection.AddScoped(_ => new ConsoleCommandArgs(args));
        serviceCollection.AddHostedService<ConsoleHost>();
        return serviceCollection;
    }
}