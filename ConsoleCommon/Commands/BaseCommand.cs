using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleCommon.Options;
using StructureCommon.Reporting;

namespace ConsoleCommon.Commands
{
    /// <summary>
    /// An abstract implementation of the <see cref="ICommand"/>
    /// that contains standard logic for validation of a command.
    /// </summary>
    public abstract class BaseCommand : ICommand
    {
        protected ICommand fSubCommand;
        protected IConsole fConsole;
        protected IReportLogger fLogger;

        /// <inheritdoc/>
        public abstract string Name
        {
            get;
        }

        /// <inheritdoc/>
        public virtual IList<ICommand> SubCommands
        {
            get;
        } = new List<ICommand>();

        /// <inheritdoc/>
        public virtual string[] OptionsByName
        {
            get
            {
                return Options.Any() ? Options.Select(option => option.Name).ToArray() : Array.Empty<string>();
            }
        }

        /// <inheritdoc/>
        public virtual IList<CommandOption> Options
        {
            get;
        } = new List<CommandOption>();

        public BaseCommand(IConsole console, IReportLogger logger)
        {
            fConsole = console;
            fLogger = logger;
        }

        public BaseCommand(IConsole console)
            : this(console, null)
        {
        }

        /// <inheritdoc/>
        public virtual void WriteHelp(IConsole console)
        {
            if (SubCommands != null && SubCommands.Count > 0)
            {
                console.WriteLine("Sub commands:");
                foreach (var command in SubCommands)
                {
                    console.WriteLine($"{command.Name}");
                }
            }

            if (Options.Count > 0)
            {
                console.WriteLine("Valid options:");
                foreach (var option in Options)
                {
                    console.WriteLine($"{option.Name} - {option.Description}");
                }
            }
        }

        /// <inheritdoc/>
        /// <summary>
        /// The base Execute here only can execute any given subcommand.
        /// Does not know enough to execute the options.
        /// </summary>
        public virtual int Execute(string[] args)
        {
            if (fSubCommand != null)
            {
                string[] commandArgs = args.Skip(1).ToArray();
                return fSubCommand.Execute(commandArgs);
            }

            return 0;
        }

        /// <inheritdoc/>
        public virtual bool ValidateOptions(string[] args)
        {
            bool optionsValid = true;
            if (Options != null && Options.Count > 0)
            {
                for (int index = 0; index < args.Length - 1; index++)
                {
                    if (!args[index].StartsWith("--"))
                    {
                        continue;
                    }
                    else
                    {
                        string optionName = args[index].TrimStart('-');
                        if (!Options.Any(opt => opt.Name.Equals(optionName)))
                        {
                            fConsole.WriteError($"Option {optionName} is not a valid option for the {Name} command.");
                            return false;
                        }

                        var option = Options.First(opt => opt.Name.Equals(optionName));
                        option.InputValue = args[index + 1];

                        bool valid = option.Validate();
                        if (!valid)
                        {
                            optionsValid = false;
                        }
                    }
                }
            }

            if (!optionsValid)
            {
                fConsole.WriteError("Options not valid.");
                return false;
            }

            if (SubCommands != null && SubCommands.Count > 0)
            {
                fSubCommand = SubCommands.FirstOrDefault(command => command.Name == args[0]);
                if (fSubCommand != null)
                {
                    string[] commandArgs = args.Skip(1).ToArray();
                    return fSubCommand.ValidateOptions(commandArgs);
                }
            }

            return true;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return Name + $" options: {string.Join(", ", OptionsByName)}";
        }
    }
}
