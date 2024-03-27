using System;
using System.Collections.Generic;
using System.Linq;

using Effanville.Common.Console.Commands;

using Microsoft.Extensions.Logging;

namespace Effanville.Common.Console
{
    /// <inheritdoc />
    public sealed class ConsoleContext : IConsoleContext
    {
        /// <summary>
        /// The names to use for displaying the help.
        /// </summary>
        private readonly HashSet<string> _helpNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "help", "--help" };

        /// <summary>
        /// The chosen command.
        /// </summary>
        private ICommand _command;

        private readonly string[] _args;

        /// <summary>
        /// The console mechanism.
        /// </summary>
        private readonly IConsole _console;

        /// <summary>
        /// The logger for the context.
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// A list of valid commands.
        /// </summary>
        private List<ICommand> _validCommands;
        
        /// <summary>
        /// Construct an instance.
        /// </summary>
        public ConsoleContext(ConsoleCommandArgs args, IEnumerable<ICommand> validCommands, IConsole console, ILogger<ConsoleContext> logger)
        {
            _validCommands = validCommands.ToList();
            _args = args.Args;
            _console = console;
            _logger = logger;
        }

        /// <inheritdoc/>
        public int ValidateAndExecute()
        {
            if (IsHelpRequired())
            {
                WriteHelp();
            }

            if (!Validate())
            {
                _logger.Log(LogLevel.Error, "Command line input failed validation.");
                _console.WriteError("Options specified were not valid.");
                return (int)ExitCode.OptionError;
            }

            int exitCode = Execute();
            if (exitCode == 0)
            {
                return exitCode;
            }

            _logger.Log(LogLevel.Error, "Error when Executing command line input.");
            _console.WriteError("Exit code does not suggest success.");

            return exitCode;
        }

        /// <summary>
        /// Whether help is needed for this context.
        /// </summary>
        /// <returns></returns>
        private bool IsHelpRequired() 
            => _args.Length == 0 || (_args.Length > 0 && _helpNames.Contains(_args[0]));

        /// <summary>
        /// The mechanism for writing help.
        /// </summary>
        private void WriteHelp()
        {
            _console.WriteLine("Valid commands:");
            foreach (var command in _validCommands)
            {
                _console.WriteLine($"{command.Name}");
            }
        }

        /// <inheritdoc />
        public bool Validate()
        {
            if (_args.Length == 0)
            {
                _console.WriteError("Could not locate suitable command to execute.");
                return false;
            }
            _command = _validCommands.FirstOrDefault(command => command.Name.Equals(_args[0], StringComparison.OrdinalIgnoreCase));
            if (_command == null)
            {
                _logger.Log(LogLevel.Error, "Could not locate suitable command to validate.");
                _console.WriteError("Could not locate suitable command to validate.");
                return false;
            }

            string[] commandArgs = _args.Skip(1).ToArray();
            return _command.Validate(_console, commandArgs);
        }

        /// <inheritdoc />
        public int Execute()
        {
            if (_command == null)
            {
                _command = _validCommands.FirstOrDefault(command => command.Name.Equals(_args[0], StringComparison.OrdinalIgnoreCase));
                if (_command == null)
                {
                    _logger.Log(LogLevel.Error, "Could not locate suitable command to execute.");
                    _console.WriteError("Could not locate suitable command to execute.");
                    return (int)ExitCode.CommandError;
                }
            }

            string[] commandArgs = _args.Skip(1).ToArray();
            if (commandArgs.Any() && _helpNames.Contains(commandArgs.FirstOrDefault()))
            {
                _command.WriteHelp(_console);
                return (int)ExitCode.Success;
            }

            return _command.Execute(_console, commandArgs);
        }
    }
}
