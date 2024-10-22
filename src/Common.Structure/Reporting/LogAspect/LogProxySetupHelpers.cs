using System;

using Castle.DynamicProxy;

using Microsoft.Extensions.DependencyInjection;

namespace Effanville.Common.Structure.Reporting.LogAspect;

/// <summary>
/// Helpers for setting up Logging proxies.
/// </summary>
public static class LogProxySetupHelpers
{
    /// <summary>
    /// Add the proxy class to the DI container.
    /// </summary>
    public static IServiceCollection AddProxiedScoped<TService, TImplementation>(
        this IServiceCollection services)
        where TService : class
        where TImplementation : class, TService
    {
        services.AddScoped<TImplementation>();
        services.AddScoped(provider =>
        {
            TImplementation implementation = provider.GetRequiredService<TImplementation>();
            ProxyGenerator proxyGenerator = new ProxyGenerator();
            LogInterceptor logInterceptor = provider.GetRequiredService<LogInterceptor>();

            return proxyGenerator.CreateInterfaceProxyWithTarget<TService>(implementation, logInterceptor);
        });

        return services;
    }
    
    /// <summary>
    /// Add the proxy class to the DI container.
    /// </summary>
    public static IServiceCollection AddProxiedScoped(
        this IServiceCollection services, Type inter, Type instance)
    {
        services.AddScoped(instance);
        services.AddScoped(inter, provider =>
        {
            object implementation = provider.GetRequiredService(instance);
            ProxyGenerator proxyGenerator = new ProxyGenerator();
            LogInterceptor logInterceptor = provider.GetRequiredService<LogInterceptor>();

            return proxyGenerator.CreateInterfaceProxyWithTarget(inter, implementation, logInterceptor);
        });

        return services;
    }
}