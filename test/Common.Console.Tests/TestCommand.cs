using System.Collections.Generic;

using Common.Structure.Reporting;

using Effanville.Common.Console.Commands;
using Effanville.Common.Console.Options;

namespace Effanville.Common.Console.Tests
{
    public sealed class TestCommand : ICommand
    {
        public string Name => "Test";

        public IList<CommandOption> Options { get; } = new List<CommandOption>();

        public IList<ICommand> SubCommands { get; } = new List<ICommand>();

        /// <inheritdoc/>
        public void WriteHelp(IConsole console) => CommandExtensions.WriteHelp(this, console);

        /// <inheritdoc/>
        public int Execute(IConsole console, IReportLogger logger, string[] args) => 0;

        /// <inheritdoc/>
        public int Execute(IConsole console, string[] args) => Execute(console, null, args);

        /// <inheritdoc/>
        public bool Validate(IConsole console, IReportLogger logger, string[] args) 
            => this.Validate(args, console, logger);

        /// <inheritdoc/>
        public bool Validate(IConsole console, string[] args) 
            => Validate(console, null, args);
    }
}