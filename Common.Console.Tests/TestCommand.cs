using System.Collections.Generic;

using Common.Console.Commands;
using Common.Console.Options;
using Common.Structure.Reporting;

namespace Common.Console.Tests
{
    public sealed class TestCommand : ICommand
    {
        public TestCommand()
        {
        }

        public string Name => "Test";

        public IList<CommandOption> Options
        {
            get;
        } = new List<CommandOption>();

        public IList<ICommand> SubCommands
        {
            get;
        } = new List<ICommand>();

        /// <inheritdoc/>
        public void WriteHelp(IConsole console)
        {
            CommandExtensions.WriteHelp(this, console);
        }

        /// <inheritdoc/>
        public int Execute(IConsole console, IReportLogger logger, string[] args)
        {
            return CommandExtensions.Execute(this, console, logger, args);
        }

        /// <inheritdoc/>
        public int Execute(IConsole console, string[] args)
        {
            return Execute(console, null, args);
        }

        /// <inheritdoc/>
        public bool Validate(IConsole console, IReportLogger logger, string[] args)
        {
            return CommandExtensions.Validate(this, args, console, logger);
        }

        /// <inheritdoc/>
        public bool Validate(IConsole console, string[] args)
        {
            return Validate(console, null, args);
        }
    }
}