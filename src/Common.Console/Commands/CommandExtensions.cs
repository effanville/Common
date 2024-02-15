using System;
using System.Linq;

using Common.Structure.Reporting;

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
        public static void WriteHelp(this ICommand cmd, IConsole console)
        {
            if (cmd.SubCommands != null && cmd.SubCommands.Count > 0)
            {
                console.WriteLine("Sub commands:");
                foreach (var command in cmd.SubCommands)
                {
                    console.WriteLine($"{command.Name}");
                }
            }

            if (cmd.Options.Count <= 0)
            {
                return;
            }

            console.WriteLine("Valid options:");
            foreach (var option in cmd.Options)
            {
                console.WriteLine($"{option.Name} - {option.Description}");
            }
        }

        /// <summary>
        /// Standard validation routine ensuring all options and sub commands are validated.
        /// </summary>
        public static bool Validate(this ICommand cmd, string[] args, IConsole console)
            => Validate(cmd, args, console, null);

        /// <summary>
        /// Standard validation routine ensuring all options and sub commands are validated.
        /// </summary>
        public static bool Validate(this ICommand cmd, string[] args, IConsole console, IReportLogger logger)
        {
            if (cmd.SubCommands != null && cmd.SubCommands.Count > 0)
            {
                var subCommand = cmd.SubCommands.FirstOrDefault(command => command.Name == args[0]);
                if (subCommand != null)
                {
                    string[] commandArgs = args.Skip(1).ToArray();
                    return subCommand.Validate(console, logger, commandArgs);
                }
            }

            var options = cmd.Options;
            if (options == null || options.Count <= 0)
            {
                return false;
            }

            if (args == null)
            {
                return false;
            }

            bool isValid = true;
            // cycle through user given values filling in option values.
            for (int index = 0; index < args.Length - 1; index++)
            {
                if (!args[index].StartsWith("--"))
                {
                    continue;
                }

                string optionName = args[index].TrimStart('-');
                if (!options.Any(opt => opt.Name.Equals(optionName, StringComparison.OrdinalIgnoreCase)))
                {
                    string error = $"Option {optionName} is not a valid option.";
                    logger?.Log(ReportSeverity.Critical, ReportType.Error, $"{cmd.Name}.{nameof(Validate)}", error);
                    return false;
                }

                var option = options.First(opt => opt.Name.Equals(optionName, StringComparison.OrdinalIgnoreCase));
                option.InputValue = args[index + 1];
            }

            foreach (var option in options)
            {
                if (option.Validate())
                {
                    continue;
                }

                string error = $"{option.GetPrettyErrorMessage()}";
                logger?.Log(ReportSeverity.Critical, ReportType.Error, $"{nameof(Validate)}", error);
                isValid = false;
            }

            return isValid;
        }

        /// <summary>
        /// A default execute algorithm that attempts to execute a sub command.
        /// Returns error if fails to execute a sub command.
        /// </summary>
        public static int Execute(this ICommand cmd, IConsole console, string[] args)
            => Execute(cmd, console, null, args);

        /// <summary>
        /// A default execute algorithm that attempts to execute a sub command.
        /// Returns error if fails to execute a sub command.
        /// </summary>
        public static int Execute(this ICommand cmd, IConsole console, IReportLogger logger, string[] args)
        {
            var subCommand = cmd.SubCommands.FirstOrDefault(command =>
                command.Name.Equals(args[0], StringComparison.OrdinalIgnoreCase));

            if (subCommand == null)
            {
                return 1;
            }

            string[] commandArgs = args.Skip(1).ToArray();
            return subCommand.Execute(console, logger, commandArgs);
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