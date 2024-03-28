using System.Collections.Generic;

using Effanville.Common.Console.Commands;
using Effanville.Common.Console.Options;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Effanville.Common.Console.Tests
{
    public sealed class TestCommand : ICommand
    {
        private readonly ILogger<TestCommand> _logger;
        public string Name { get; set; } = "Test";

        public IList<CommandOption> Options { get; } = new List<CommandOption>();

        public IList<ICommand> SubCommands { get; } = new List<ICommand>();

        public TestCommand(ILogger<TestCommand> logger) 
        {
            _logger = logger;
        }

        /// <inheritdoc/>
        public void WriteHelp(IConsole console) => this.WriteHelp(console, _logger);

        /// <inheritdoc/>
        public int Execute(IConsole console, IConfiguration config) => 0;

        /// <inheritdoc/>
        public bool Validate(IConsole console, IConfiguration config) 
            => this.Validate(config, console, _logger);
    }
}