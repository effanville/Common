using System;

namespace ConsoleCommon
{
    /// <summary>
    /// An instance of an <see cref="IConsole"/> allowing for
    /// writing lines and errors.
    /// </summary>
    public sealed class ConsoleInstance : IConsole
    {
        private readonly Action<string> fWriteAction;
        private readonly Action<string> fWriteErrorAction;

        /// <summary>
        /// Construct an instance.
        /// </summary>
        /// <param name="writeErrorAction">The action for writing an error.</param>
        /// <param name="writeAction">The action for writing a normal line.</param>
        public ConsoleInstance(Action<string> writeErrorAction, Action<string> writeAction)
        {
            fWriteErrorAction = writeErrorAction;
            fWriteAction = writeAction;
        }

        /// <inheritdoc/>
        public void WriteLine(string line = null)
        {
            fWriteAction(line);
        }

        /// <inheritdoc/>
        public void WriteError(string error = null)
        {
            fWriteErrorAction(error);
        }
    }
}
