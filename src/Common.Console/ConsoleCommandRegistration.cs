using System;
using System.Collections.Generic;

using Effanville.Common.Console.Commands;
using Effanville.Common.Structure.Reporting.LogAspect;

using Microsoft.Extensions.DependencyInjection;

namespace Effanville.Common.Console;

/// <summary>
/// Contains registration methods for adding <see cref="ICommand"/>s
/// into the DI container.
/// </summary>
public static class ConsoleCommandRegistration
{
    /// <summary>
    /// Registers the provided types into the container as
    /// types of <see cref="ICommand"/>
    /// </summary>
    /// <param name="serviceCollection">The service collection to add to.</param>
    /// <param name="consoleCommandTypes">The types to add. These are expected to be types that implement <see cref="ICommand"/></param>
    /// <returns>The service collection.</returns>
    public static IServiceCollection AddConsoleCommands(
        this IServiceCollection serviceCollection,
        IEnumerable<Type> consoleCommandTypes)
    {
        foreach (Type commandType in consoleCommandTypes)
        {
            if(commandType.IsAssignableTo(typeof(ILogInterceptable)))
            {
                serviceCollection.AddProxiedScoped(typeof(ICommand), commandType);
            }
            else
            {
                serviceCollection.AddScoped(typeof(ICommand), commandType);
            }
        }

        return serviceCollection;
    }
}