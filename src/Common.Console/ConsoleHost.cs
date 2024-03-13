using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Effanville.Common.Console.Commands;
using Effanville.Common.Structure.Extensions;
using Effanville.Common.Structure.Reporting;

using Microsoft.Extensions.Hosting;

namespace Effanville.Common.Console;

/// <summary>
/// A <see cref="IHostedService"/> implementation that enables a console instance to run.
/// </summary>
public sealed class ConsoleHost : IHostedService
{
    internal int? _exitCode;
    private readonly ConsoleCommandArgs _commandArgs;
    private readonly IFileSystem _fileSystem;
    private readonly IConsole _console;
    private readonly IReportLogger _logger;
    private readonly List<ICommand> _validCommands;
    private readonly IHostApplicationLifetime _applicationLifetime;

    /// <summary>
    /// Construct an instance.
    /// </summary>
    public ConsoleHost(
        ConsoleCommandArgs commandArgs,
        IFileSystem fileSystem,
        IConsole console,
        IReportLogger logger,
        IEnumerable<ICommand> validCommands,
        IHostApplicationLifetime applicationLifetime)
    {
        _commandArgs = commandArgs;
        _fileSystem = fileSystem;
        _console = console;
        _logger = logger;
        _applicationLifetime = applicationLifetime;
        _validCommands = validCommands.ToList();
    }

    /// <summary>
    /// The call to start the host running.
    /// </summary>
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.Log(ReportSeverity.Useful, ReportType.Information, $"FPDConsole", "FPDConsole.exe - version 1");
        _applicationLifetime.ApplicationStarted.Register(() =>
        {
            try
            {
                _exitCode = ConsoleContext.SetAndExecute(_commandArgs.Args, _fileSystem, _console, _logger, _validCommands);
            }
            finally
            {
                _applicationLifetime.StopApplication();
            }
        });
        return Task.CompletedTask;
    }

    /// <summary>
    /// The call to stop the host from running.
    /// </summary>
    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.Log(ReportSeverity.Useful, ReportType.Information, "FPDConsole",
            "Completed processing. Shutting Down.");
        string logPath = _fileSystem.Path.Combine(_fileSystem.Directory.GetCurrentDirectory(),
            $"{DateTime.Now.FileSuitableDateTimeValue()}-consoleLog.log");
        _logger.WriteReportsToFile(logPath);
        Environment.ExitCode = _exitCode.GetValueOrDefault(-1);
        return Task.CompletedTask;
    }
}