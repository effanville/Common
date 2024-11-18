using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Effanville.Common.Console;

/// <summary>
/// A <see cref="IHostedService"/> implementation that enables a console instance to run.
/// </summary>
public sealed class ConsoleHost : IHostedService
{
    internal int? ExitCode;
    private readonly IConsoleContext _consoleContext;
    private readonly ILogger<ConsoleHost> _logger;
    private readonly IHostApplicationLifetime _applicationLifetime;

    /// <summary>
    /// Construct an instance.
    /// </summary>
    public ConsoleHost(
        IConsoleContext consoleContext,
        ILogger<ConsoleHost> logger,
        IHostApplicationLifetime applicationLifetime)
    {
        _consoleContext = consoleContext;
        _logger = logger;
        _applicationLifetime = applicationLifetime;
    }

    private void RunInBackground()
    {
        try
        {
            ExitCode = _consoleContext.ValidateAndExecute();
        }
        finally
        {
            _applicationLifetime.StopApplication();
        }
    }

    /// <summary>
    /// The call to start the host running.
    /// </summary>
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.Log(LogLevel.Information, "Starting Processing.");
        _applicationLifetime.ApplicationStarted.Register(() =>
        {
            Task.Run(RunInBackground, cancellationToken);
        });
        return Task.CompletedTask;
    }

    /// <summary>
    /// The call to stop the host from running.
    /// </summary>
    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.Log(LogLevel.Information, "Completed processing. Shutting Down.");
        Environment.ExitCode = ExitCode.GetValueOrDefault(-1);
        return Task.CompletedTask;
    }
}