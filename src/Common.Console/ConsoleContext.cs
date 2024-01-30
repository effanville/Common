using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;

using Common.Console.Commands;
using Common.Structure.Reporting;

namespace Common.Console
{
    /// <summary>
    /// Contains the context for a console application, as well as the
    /// validation and execution routines.
    /// </summary>
    public sealed class ConsoleContext
    {
        /// <summary>
        /// The names to use for displaying the help.
        /// </summary>
        private readonly HashSet<string> _helpNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "help", "--help" };

        /// <summary>
        /// The chosen command.
        /// </summary>
        private ICommand _command;

        /// <summary>
        /// The command line arguments specified.
        /// </summary>
        public string[] Args
        {
            get;
        }

        /// <summary>
        /// The console mechanism.
        /// </summary>
        public IConsole Console
        {
            get;
        }

        /// <summary>
        /// The logger for the context.
        /// </summary>
        public IReportLogger Logger
        {
            get;
        }

        /// <summary>
        /// Global scope objects for this context.
        /// </summary>
        public ConsoleGlobals Globals
        {
            get;
        }

        /// <summary>
        /// A list of valid commands.
        /// </summary>
        public List<ICommand> ValidCommands
        {
            get;
            private set;
        }

        private ConsoleContext(string[] args, IFileSystem fileSystem, IConsole console, IReportLogger logger)
            : this(args, new ConsoleGlobals(fileSystem, console, logger))
        {
        }

        private ConsoleContext(string[] args, ConsoleGlobals globals)
        {
            Args = args;
            Globals = globals;
            Console = Globals.Console;
            Logger = Globals.ReportLogger;
        }

        /// <summary>
        /// Performs a standard routine from the inputs. Validates the arguments before execution.
        /// </summary>
        /// <param name="args">The command line arguments specified.</param>
        /// <param name="console">The console to write with.</param>
        /// <param name="validCommands">The valid commands for this context.</param>
        public static int SetAndExecute(string[] args, IConsole console, List<ICommand> validCommands) 
            => SetAndExecute(args, new FileSystem(), console, null, validCommands);

        /// <summary>
        /// Performs a standard routine from the inputs. Validates the arguments before execution.
        /// </summary>
        /// <param name="args">The command line arguments specified.</param>
        /// <param name="console">The console to write with.</param>
        /// <param name="logger">The logging routine.</param>
        /// <param name="validCommands">The valid commands for this context.</param>
        public static int SetAndExecute(string[] args, IConsole console, IReportLogger logger, List<ICommand> validCommands) 
            => SetAndExecute(args, new FileSystem(), console, logger, validCommands);

        /// <summary>
        /// Performs a standard routine from the inputs. Validates the arguments before execution.
        /// </summary>
        /// <param name="args">The command line arguments specified.</param>
        /// <param name="fileSystem">The file system for this interaction.</param>
        /// <param name="console">The console to write with.</param>
        /// <param name="logger">The logging routine.</param>
        /// <param name="validCommands">The valid commands for this context.</param>
        public static int SetAndExecute(string[] args, IFileSystem fileSystem, IConsole console, IReportLogger logger, List<ICommand> validCommands)
        {
            var context = new ConsoleContext(args, fileSystem, console, logger);
            context.SetValidCommands(validCommands);
            return context.ValidateAndExecute();
        }

        /// <summary>
        /// Performs a standard routine from the inputs. Validates the arguments before execution.
        /// </summary>
        /// <param name="args">The command line arguments specified.</param>
        /// <param name="globals">The global scope objects for this context.</param>
        /// <param name="validCommands">The valid commands for this context.</param>
        public static int SetAndExecute(string[] args, ConsoleGlobals globals, List<ICommand> validCommands)
        {
            var context = new ConsoleContext(args, globals);
            context.SetValidCommands(validCommands);
            return context.ValidateAndExecute();
        }

        /// <summary>
        /// Sets the commands that are valid in this context.
        /// </summary>
        private void SetValidCommands(List<ICommand> commands) 
            => ValidCommands = commands;

        private int ValidateAndExecute()
        {
            if (IsHelpRequired())
            {
                WriteHelp();
            }

            if (!Validate())
            {
                Logger.Log(ReportSeverity.Critical, ReportType.Error, $"{nameof(Validate)}", "Command line input failed validation.");
                Console.WriteError("Options specified were not valid.");
                return (int)ExitCode.OptionError;
            }

            int exitCode = Execute();
            if (exitCode == 0)
            {
                return exitCode;
            }

            Logger.Log(ReportSeverity.Critical, ReportType.Error, $"{nameof(Execute)}", "Error when Executing command line input.");
            Console.WriteError("Exit code does not suggest success.");

            return exitCode;
        }

        /// <summary>
        /// Whether help is needed for this context.
        /// </summary>
        /// <returns></returns>
        private bool IsHelpRequired() 
            => Args.Length == 0 || (Args.Length > 0 && _helpNames.Contains(Args[0]));

        /// <summary>
        /// The mechanism for writing help.
        /// </summary>
        private void WriteHelp()
        {
            Console.WriteLine("Valid commands:");
            foreach (var command in ValidCommands)
            {
                Console.WriteLine($"{command.Name}");
            }
        }

        /// <summary>
        /// Validates the chosen command.
        /// </summary>
        /// <returns></returns>
        public bool Validate()
        {
            if (Args.Length == 0)
            {
                Console.WriteError("Could not locate suitable command to execute.");
                return false;
            }
            _command = ValidCommands.FirstOrDefault(command => command.Name.Equals(Args[0], StringComparison.OrdinalIgnoreCase));
            if (_command == null)
            {
                Logger.Log(ReportSeverity.Critical, ReportType.Error, $"{nameof(Validate)}", "Could not locate suitable command to validate.");
                Console.WriteError("Could not locate suitable command to validate.");
                return false;
            }

            string[] commandArgs = Args.Skip(1).ToArray();
            return _command.Validate(commandArgs, Console, Logger);
        }

        /// <summary>
        /// Executes the chosen command.
        /// </summary>
        /// <returns></returns>
        public int Execute()
        {
            if (_command == null)
            {
                _command = ValidCommands.FirstOrDefault(command => command.Name.Equals(Args[0], StringComparison.OrdinalIgnoreCase));
                if (_command == null)
                {
                    Logger.Log(ReportSeverity.Critical, ReportType.Error, $"{nameof(Execute)}", "Could not locate suitable command to execute.");
                    Console.WriteError("Could not locate suitable command to execute.");
                    return (int)ExitCode.CommandError;
                }
            }

            string[] commandArgs = Args.Skip(1).ToArray();
            if (commandArgs.Any())
            {
                if (_helpNames.Contains(commandArgs.FirstOrDefault()))
                {
                    _command.WriteHelp(Console);
                    return (int)ExitCode.Success;
                }
            }

            return _command.Execute(Console, Logger, commandArgs);
        }
    }
}
