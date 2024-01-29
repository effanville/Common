using System;

namespace Common.Console
{
    /// <summary>
    /// An instance of an <see cref="IConsole"/> allowing for
    /// writing lines and errors.
    /// </summary>
    public sealed class ConsoleInstance : IConsole
    {
        private readonly Action<string> _writeAction;
        private readonly Action<string> _writeErrorAction;

        /// <summary>
        /// Construct an instance.
        /// </summary>
        /// <param name="writeErrorAction">The action for writing an error.</param>
        /// <param name="writeAction">The action for writing a normal line.</param>
        public ConsoleInstance(Action<string> writeErrorAction, Action<string> writeAction)
        {
            _writeErrorAction = writeErrorAction;
            _writeAction = writeAction;
        }

        /// <inheritdoc/>
        public void WriteLine(string line = null) => _writeAction(line);

        /// <inheritdoc/>
        public void WriteError(string error = null) => _writeErrorAction(error);
    }
}
