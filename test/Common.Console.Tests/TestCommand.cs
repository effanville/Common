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
        private readonly IConfiguration _config;

        public string Name { get; set; } = "Test";

        public IList<CommandOption> Options { get; } = new List<CommandOption>();

        public IList<ICommand> SubCommands { get; } = new List<ICommand>();

        public TestCommand(ILogger<TestCommand> logger,
            IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        /// <inheritdoc/>
        public void WriteHelp() => this.WriteHelp(_logger);

        /// <inheritdoc/>
        public int Execute() => 0;

        /// <inheritdoc/>
        public bool Validate()
            => this.Validate(_config, _logger);
    }
}