using System.Collections.Generic;

using Effanville.Common.Console.Options;

using Microsoft.Extensions.Configuration;

namespace Effanville.Common.Console.Commands
{
    /// <summary>
    /// A command for a console, namely a string that contains specified
    /// interactions for a user.
    /// </summary>
    /// <example>
    /// The command line writing <code>MyProgram.exe commandName --option "value"</code>
    /// would manifest itself as a command with Name commandName and with an option called
    /// option that the user has specified with a certain value.
    /// </example>
    public interface ICommand
    {
        /// <summary>
        /// The name of this command. This also is the expected first argument
        /// on the command line.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The options for this command.
        /// </summary>
        IList<CommandOption> Options { get; }

        /// <summary>
        /// Any sub commands for this command.
        /// </summary>
        IList<ICommand> SubCommands { get; }

        /// <summary>
        /// The method to write help for this command.
        /// </summary>
        void WriteHelp();

        /// <summary>
        /// The mechanism for validating the input option values.
        /// </summary>
        /// <param name="config">The command line arguments.</param>
        /// <returns>Was validation successful.</returns>
        bool Validate(IConfiguration config);

        /// <summary>
        /// Execute the given command.
        /// </summary>
        /// <param name="config">The command line arguments.</param>
        /// <returns>The exit code of the command.</returns>
        int Execute(IConfiguration config);
    }
}