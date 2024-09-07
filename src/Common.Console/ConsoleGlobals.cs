using System.IO.Abstractions;

using Microsoft.Extensions.Logging;

namespace Effanville.Common.Console
{
    /// <summary>
    /// Contains data required at a global scope for a console app.
    /// </summary>
    public sealed class ConsoleGlobals
    {
        /// <summary>
        /// The current working directory for the application.
        /// </summary>
        public string CurrentWorkingDirectory { get; set; }

        /// <summary>
        /// The current filesystem for the application.
        /// </summary>
        public IFileSystem CurrentFileSystem { get; }

        /// <summary>
        /// A logger to log with.
        /// </summary>
        public ILogger ReportLogger { get; set; }

        /// <summary>
        /// Standard constructor.
        /// </summary>
        public ConsoleGlobals(string currentWorkingDirectory, IFileSystem currentFileSystem, ILogger reportLogger)
        {
            CurrentWorkingDirectory = currentWorkingDirectory;
            CurrentFileSystem = currentFileSystem;
            ReportLogger = reportLogger;
        }

        /// <summary>
        /// Standard constructor using the file system.
        /// </summary>
        public ConsoleGlobals(IFileSystem currentFileSystem, ILogger reportLogger)
            : this(currentFileSystem.Directory.GetCurrentDirectory(), currentFileSystem, reportLogger)
        {
        }
    }
}
