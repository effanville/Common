using System;
using System.Collections.Generic;
using System.Linq;

using Effanville.Common.Console.Commands;

using Microsoft.Extensions.Configuration;
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

        private readonly IConfiguration _config;

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
        public ConsoleContext(IConfiguration config, IEnumerable<ICommand> validCommands, ILogger<ConsoleContext> logger)
        {
            _validCommands = validCommands.ToList();
            _config = config;
            _logger = logger;
        }

        /// <inheritdoc/>
        public int ValidateAndExecute()
        {
            if (IsHelpRequired())
            { 
                WriteHelp();
                return (int)ExitCode.Success;
            }

            if (!Validate())
            {
                _logger.Log(LogLevel.Error, "Command line input failed validation.");
                return (int)ExitCode.OptionError;
            }

            int exitCode = Execute();
            if (exitCode == 0)
            {
                return exitCode;
            }

            _logger.Log(LogLevel.Error, "Error when Executing command line input.");

            return exitCode;
        }

        /// <summary>
        /// Whether help is needed for this context.
        /// </summary>
        /// <returns></returns>
        private bool IsHelpRequired() 
            => string.IsNullOrEmpty(_config.GetValue<string>("CommandName")) 
               || !string.IsNullOrWhiteSpace(_config.GetValue<string>("help"))
               || _helpNames.Contains(_config.GetValue<string>("CommandName"));

        /// <summary>
        /// The mechanism for writing help.
        /// </summary>
        private void WriteHelp()
        {
            _logger.Log(LogLevel.Information, "Valid commands:");
            foreach (var command in _validCommands)
            {
                _logger.Log(LogLevel.Information, $"{command.Name}");
                command.WriteHelp();
            }
        }

        /// <inheritdoc />
        public bool Validate()
        {
            string commandNames = _config.GetValue<string>("CommandName");
            if (string.IsNullOrWhiteSpace(commandNames))
            {
                _logger.Log(LogLevel.Error, "Could not locate suitable command to execute.");
                return false;
            }

            string[] orderedCommandNames = commandNames.Split(';');
            _command = _validCommands.FirstOrDefault(command => command.Name.Equals(orderedCommandNames[0], StringComparison.OrdinalIgnoreCase));
            if (_command == null)
            {
                _logger.Log(LogLevel.Error, "Could not locate suitable command to validate.");
                return false;
            }

            return _command.Validate(_config);
        }

        /// <inheritdoc />
        public int Execute()
        {
            if (_command == null)
            {
                string commandNames = _config.GetValue<string>("CommandName");
                _command = _validCommands.FirstOrDefault(command => command.Name.Equals(commandNames, StringComparison.OrdinalIgnoreCase));
                if (_command == null)
                {
                    _logger.Log(LogLevel.Error, "Could not locate suitable command to execute.");
                    return (int)ExitCode.CommandError;
                }
            }

            if (_config != null && _helpNames.Contains(_config.GetValue<string>("CommandName")))
            {
                _command.WriteHelp();
                return (int)ExitCode.Success;
            }

            return _command.Execute(_config);
        }
    }
}
