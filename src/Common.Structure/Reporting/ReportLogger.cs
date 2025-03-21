using System;
using System.IO;
using System.IO.Abstractions;

using Effanville.Common.Structure.DataStructures;

namespace Effanville.Common.Structure.Reporting
{
    /// <summary>
    /// Collection of standard reporting mechanisms.
    /// </summary>
    public class LogReporter : IReportLogger
    {
        private readonly Action<ReportSeverity, ReportType, string, string> _loggingAction;

        private readonly ITaskQueue _loggingQueue;

        /// <inheritdoc/>
        public ErrorReports Reports { get; set; } = new ErrorReports();

        /// <inheritdoc/>
        public bool SaveInternally { get; set; }

        /// <summary>
        /// Constructor for reporting mechanisms. Parameter addReport is the report callback mechanism.
        /// </summary>
        public LogReporter(Action<ReportSeverity, ReportType, string, string> addReport)
        {
            _loggingAction = addReport;
            _loggingQueue = new TaskQueue();
        }

        /// <summary>
        /// Constructor for reporting mechanisms. Parameter addReport is the report callback mechanism.
        /// </summary>
        public LogReporter(Action<ReportSeverity, ReportType, string, string> addReport, ITaskQueue taskQueue, bool saveInternally)
        {
            _loggingAction = addReport;
            _loggingQueue = taskQueue;
            SaveInternally = saveInternally;
        }

        /// <summary>
        /// Constructor for reporting mechanisms. Parameter addReport is the report callback mechanism.
        /// </summary>
        public LogReporter(Action<ReportSeverity, ReportType, string, string> addReport, bool saveInternally)
            : this(addReport)
        {
            SaveInternally = saveInternally;
        }

        /// <inheritdoc />
        public void Log(ReportType type, string location, string message)
            => LogInternal(ReportSeverity.Useful, type, location, message);

        private bool LogInternal(ReportSeverity severity, ReportType type, string location, string message)
        {
            if (SaveInternally)
            {
                Reports.AddErrorReport(severity, type, location, message);
            }
            if (_loggingAction == null)
            {
                return false;
            }

            _loggingQueue.Enqueue(() => _loggingAction(severity, type, location.ToString(), message));
            return true;
        }

        /// <inheritdoc/>
        public void WriteReportsToFile(string filePath, IFileSystem fileSystem, out string error)
        {
            error = null;
            if (string.IsNullOrWhiteSpace(filePath))
            {
                error = $"Filepath was empty.";
                return;
            }
            try
            {
                using (Stream stream = fileSystem.FileStream.New(filePath, FileMode.Create))
                using (TextWriter writer = new StreamWriter(stream))
                {
                    foreach (ErrorReport report in Reports)
                    {
                        writer.WriteLine(report.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
        }
    }
}
