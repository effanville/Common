using System;
using System.Linq;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Effanville.Common.Console.Commands
{
    /// <summary>
    /// Contains various extensions for the <see cref="ICommand"/> interface.
    /// </summary>
    public static class CommandExtensions
    {
        /// <summary>
        /// Writes a standard help information for the command.
        /// Writes all sub command names, followed by the available options.
        /// </summary>
        public static void WriteHelp(this ICommand cmd, ILogger logger)
        {
            if (cmd.SubCommands != null && cmd.SubCommands.Count > 0)
            {
                logger.Log(LogLevel.Information, "Sub Commands: ");
                foreach (var command in cmd.SubCommands)
                {
                    logger.Log(LogLevel.Information, command.Name);
                }
            }

            if (cmd.Options.Count <= 0)
            {
                return;
            }

            logger.Log(LogLevel.Information,"Valid options:");
            foreach (var option in cmd.Options)
            {
                logger.Log(LogLevel.Information,$"{option.Name} - {option.Description}");
            }
        }

        /// <summary>
        /// Standard validation routine ensuring all options and sub commands are validated.
        /// </summary>
        public static bool Validate(this ICommand cmd, IConfiguration config, ILogger logger)
        {
            if (cmd.SubCommands != null && cmd.SubCommands.Count > 0)
            {
                string commandName = config.GetValue<string>("CommandName");
                string[] commandNames = commandName.Split(';');
                int currentCommand = Array.FindIndex(commandNames, x => x == cmd.Name);
                if (currentCommand < commandNames.Length - 1)
                {
                    var subCommand = cmd.SubCommands.FirstOrDefault(command => command.Name == commandNames[currentCommand + 1]);
                    if (subCommand != null)
                    {
                        return subCommand.Validate(config);
                    }
                }
            }

            var options = cmd.Options;
            if (options == null || options.Count <= 0)
            {
                return false;
            }

            if (config == null)
            {
                return false;
            }

            bool isValid = true;
            // cycle through user given values filling in option values.
            foreach (var option in options)
            {
                string configValue = config.GetValue<string>(option.Name);
                if (!string.IsNullOrWhiteSpace(configValue))
                {
                    option.InputValue = configValue;
                }
            }

            foreach (var option in options)
            {
                if (option.Validate())
                {
                    continue;
                }

                string error = $"{option.GetPrettyErrorMessage()}";
                logger?.Log(LogLevel.Error, error);
                isValid = false;
            }

            return isValid;
        }
        
        /// <summary>
        /// A default execute algorithm that attempts to execute a sub command.
        /// Returns error if fails to execute a sub command.
        /// </summary>
        public static int Execute(this ICommand cmd, IConfiguration config, ILogger logger)
        {
            if (cmd.SubCommands == null || cmd.SubCommands.Count <= 0)
            {
                return 1;
            }

            string commandName = config.GetValue<string>("CommandName");
            string[] commandNames = commandName.Split(';');
            int currentCommand = Array.FindIndex(commandNames, x => x == cmd.Name);
            if (currentCommand >= commandNames.Length - 1)
            {
                return 1;
            }

            var subCommand = cmd.SubCommands.FirstOrDefault(command => command.Name == commandNames[currentCommand + 1]);
            logger?.Log(LogLevel.Information, $"Executing command {subCommand?.Name}");
            return subCommand?.Execute(config) ?? 1;
        }

        /// <summary>
        /// Provides a human readable way to output information on this <see cref="ICommand"/>
        /// </summary>
        public static string ToString(this ICommand cmd)
        {
            string[] optionNames = cmd.Options.Any()
                ? cmd.Options.Select(option => option.Name).ToArray()
                : Array.Empty<string>();
            return cmd.Name + $" options: {string.Join(", ", optionNames)}";
        }
    }
}