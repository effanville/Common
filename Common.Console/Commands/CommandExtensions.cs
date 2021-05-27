using System;
using System.Linq;

namespace Common.Console.Commands
{
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

            if (cmd.Options.Count > 0)
            {
                console.WriteLine("Valid options:");
                foreach (var option in cmd.Options)
                {
                    console.WriteLine($"{option.Name} - {option.Description}");
                }
            }
        }

        /// <summary>
        /// Standard validation routine ensuring all options and sub commands are validated.
        /// </summary>
        public static bool Validate(this ICommand cmd, string[] args, IConsole console)
        {
            if (cmd.SubCommands != null && cmd.SubCommands.Count > 0)
            {
                var subCommand = cmd.SubCommands.FirstOrDefault(command => command.Name == args[0]);
                if (subCommand != null)
                {
                    string[] commandArgs = args.Skip(1).ToArray();
                    return subCommand.Validate(console, commandArgs);
                }
            }

            var options = cmd.Options;
            if (options != null && options.Count > 0)
            {
                // cycle through user given values filling in option values.
                for (int index = 0; index < args.Length - 1; index++)
                {
                    if (!args[index].StartsWith("--"))
                    {
                        continue;
                    }
                    else
                    {
                        string optionName = args[index].TrimStart('-');
                        if (!options.Any(opt => opt.Name.Equals(optionName, StringComparison.OrdinalIgnoreCase)))
                        {
                            console.WriteError($"Option {optionName} is not a valid option for the {cmd.Name} command.");
                            return false;
                        }

                        var option = options.First(opt => opt.Name.Equals(optionName, StringComparison.OrdinalIgnoreCase));
                        option.InputValue = args[index + 1];
                    }
                }

                foreach (var option in options)
                {
                    bool valid = option.Validate();
                    if (!valid)
                    {
                        console.WriteError($"{option.GetPrettyErrorMessage()}");
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// A default execute algorithm that attempts to execute a sub command.
        /// Returns error if fails to execute a sub command.
        /// </summary>
        public static int Execute(this ICommand cmd, string[] args)
        {
            var subCommand = cmd.SubCommands.FirstOrDefault(command => command.Name == args[0]);

            if (subCommand != null)
            {
                string[] commandArgs = args.Skip(1).ToArray();
                return subCommand.Execute(commandArgs);
            }

            return 1;
        }

        public static string ToString(this ICommand cmd)
        {
            string[] optionNames = cmd.Options.Any() ? cmd.Options.Select(option => option.Name).ToArray() : Array.Empty<string>();
            return cmd.Name + $" options: {string.Join(", ", optionNames)}";
        }
    }
}
