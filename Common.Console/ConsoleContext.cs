﻿using System;
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
        private readonly HashSet<string> HelpNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "help", "--help" };

        /// <summary>
        /// The chosen command.
        /// </summary>
        private ICommand fCommand;

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

        public ConsoleContext(string[] args, IConsole console)
            : this(args, new FileSystem(), console, null)
        {
        }

        public ConsoleContext(string[] args, IConsole console, IReportLogger logger)
            : this(args, new FileSystem(), console, logger)
        {
        }

        public ConsoleContext(string[] args, IFileSystem fileSystem, IConsole console, IReportLogger logger)
            : this(args, new ConsoleGlobals(fileSystem, console, logger))
        {
        }

        public ConsoleContext(string[] args, ConsoleGlobals globals)
        {
            Args = args;
            Globals = globals;
        }

        /// <summary>
        /// Performs a standard routine from the inputs. Validates the arguments before execution.
        /// </summary>
        /// <param name="Args">The command line arguments specified.</param>
        /// <param name="console">The console to write with.</param>
        /// <param name="validCommands">The valid commands for this context.</param>
        public static void SetAndExecute(string[] Args, IConsole console, List<ICommand> validCommands)
        {
            SetAndExecute(Args, new FileSystem(), console, null, validCommands);
        }

        /// <summary>
        /// Performs a standard routine from the inputs. Validates the arguments before execution.
        /// </summary>
        /// <param name="Args">The command line arguments specified.</param>
        /// <param name="console">The console to write with.</param>
        /// <param name="logger">The logging routine.</param>
        /// <param name="validCommands">The valid commands for this context.</param>
        public static void SetAndExecute(string[] Args, IConsole console, IReportLogger logger, List<ICommand> validCommands)
        {
            SetAndExecute(Args, new FileSystem(), console, logger, validCommands);
        }

        /// <summary>
        /// Performs a standard routine from the inputs. Validates the arguments before execution.
        /// </summary>
        /// <param name="Args">The command line arguments specified.</param>
        /// <param name="fileSystem">The file system for this interaction.</param>
        /// <param name="console">The console to write with.</param>
        /// <param name="logger">The logging routine.</param>
        /// <param name="validCommands">The valid commands for this context.</param>
        public static void SetAndExecute(string[] Args, IFileSystem fileSystem, IConsole console, IReportLogger logger, List<ICommand> validCommands)
        {
            var context = new ConsoleContext(Args, fileSystem, console, logger);
            context.SetValidCommands(validCommands);
            context.ValidateAndExecute();
        }

        /// <summary>
        /// Performs a standard routine from the inputs. Validates the arguments before execution.
        /// </summary>
        /// <param name="Args">The command line arguments specified.</param>
        /// <param name="globals">The global scope objects for this context.</param>
        /// <param name="validCommands">The valid commands for this context.</param>
        public static void SetAndExecute(string[] Args, ConsoleGlobals globals, List<ICommand> validCommands)
        {
            var context = new ConsoleContext(Args, globals);
            context.SetValidCommands(validCommands);
            context.ValidateAndExecute();
        }

        /// <summary>
        /// Sets the commands that are valid in this context.
        /// </summary>
        private void SetValidCommands(List<ICommand> commands)
        {
            ValidCommands = commands;
        }

        private void ValidateAndExecute()
        {
            if (IsHelpRequired())
            {
                WriteHelp();
            }

            if (!Validate())
            {
                _ = Logger.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.Parsing, "Options specified were not valid.");
                Console.WriteError("Options specified were not valid.");
                return;
            }

            int exitcode = Execute();
            if (exitcode != 0)
            {
                _ = Logger.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.Unknown, "Error when Executing command line input.");
                Console.WriteError("Exit code does not suggest success.");
            }
        }

        /// <summary>
        /// Whether help is needed for this context.
        /// </summary>
        /// <returns></returns>
        private bool IsHelpRequired()
        {
            return Args.Length == 0 || (Args.Length > 0 && HelpNames.Contains(Args[0]));
        }

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
            fCommand = ValidCommands.FirstOrDefault(comd => comd.Name == Args[0]);
            if (fCommand == null)
            {
                Console.WriteError("Could not locate suitable command to execute.");
                return false;
            }

            string[] commandArgs = Args.Skip(1).ToArray();
            return fCommand.Validate(commandArgs, Console);
        }

        /// <summary>
        /// Executes the chosen command.
        /// </summary>
        /// <returns></returns>
        public int Execute()
        {
            if (fCommand == null)
            {
                fCommand = ValidCommands.FirstOrDefault(comd => comd.Name == Args[0]);
                if (fCommand == null)
                {
                    Console.WriteError("Could not locate suitable command to execute.");
                    return -1;
                }
            }

            string[] commandArgs = Args.Skip(1).ToArray();
            if (commandArgs.Any())
            {
                if (HelpNames.Contains(commandArgs[0]))
                {
                    fCommand.WriteHelp(Console);
                    return 0;
                }
            }

            return fCommand.Execute(Console, commandArgs);
        }
    }
}
