using System.IO.Abstractions;
using Common.Structure.Reporting;

namespace Common.Console
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
        public IReportLogger ReportLogger { get; set; }

        /// <summary>
        /// Abstraction for writing to the console.
        /// </summary>
        public IConsole Console { get; }

        /// <summary>
        /// Standard constructor.
        /// </summary>
        public ConsoleGlobals(string currentWorkingDirectory, IFileSystem currentFileSystem, IConsole console, IReportLogger reportLogger)
        {
            CurrentWorkingDirectory = currentWorkingDirectory;
            CurrentFileSystem = currentFileSystem;
            Console = console;
            ReportLogger = reportLogger;
        }

        /// <summary>
        /// Standard constructor using the file system.
        /// </summary>
        public ConsoleGlobals(IFileSystem currentFileSystem, IConsole console, IReportLogger reportLogger)
            : this(currentFileSystem.Directory.GetCurrentDirectory(), currentFileSystem, console, reportLogger)
        {
        }
    }
}
