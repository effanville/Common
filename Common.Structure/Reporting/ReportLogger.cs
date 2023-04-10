using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Threading.Tasks;

using Common.Structure.DataStructures;

namespace Common.Structure.Reporting
{
    /// <summary>
    /// Collection of standard reporting mechanisms.
    /// </summary>
    public class LogReporter : IReportLogger
    {
        private readonly Action<ReportSeverity, ReportType, string, string> _loggingAction;

        private TaskQueue _loggingQueue;

        /// <inheritdoc/>
        public ErrorReports Reports
        {
            get;
            set;
        } = new ErrorReports();

        /// <inheritdoc/>
        public bool SaveInternally
        {
            get; set;
        }

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
        public LogReporter(Action<ReportSeverity, ReportType, string, string> addReport, bool saveInternally)
            : this(addReport)
        {
            SaveInternally = saveInternally;
        }

        private void AddReport(ReportSeverity severity, ReportType type, string location, string message)
        {
            if (SaveInternally)
            {
                Reports.AddErrorReport(severity, type, location, message);
            }
        }

        /// <inheritdoc/>
        public IReport Critical()
        {
            return new ReportBuilder(this, ReportSeverity.Critical);
        }

        public IReport Useful()
        {
            return new ReportBuilder(this, ReportSeverity.Useful);
        }

        /// <inheritdoc />
        public void Log(ReportType type, string location, string message)
        {
            Log(ReportSeverity.Useful, type, location, message);
        }

        /// <inheritdoc />
        public void Log(ReportSeverity severity, ReportType type, string location, string message)
        {          
            AddReport(severity, type, location, message);
            if(_loggingAction != null)
            {
                _loggingQueue.Enqueue(() => _loggingAction?.Invoke(severity, type, location, message));
            }
        }

        /// <inheritdoc />
        public bool Log(ReportSeverity severity, ReportType type, ReportLocation location, string message)
        {            
            AddReport(severity, type, location.ToString(), message);
            if (_loggingAction == null)
            {
                return false;
            }

            _loggingQueue.Enqueue(() => _loggingAction(severity, type, location.ToString(), message));
            return true;
        }

        /// <summary>
        /// Log an arbitrary message.
        /// </summary>
        public bool LogUseful(ReportType type, ReportLocation location, string message)
        {
            return Log(ReportSeverity.Useful, type, location, message);
        }

        /// <summary>
        /// Log an arbitrary message.
        /// </summary>
        public bool LogUsefulError(ReportLocation location, string message)
        {
            return Log(ReportSeverity.Useful, ReportType.Error, location, message);
        }

        /// <inheritdoc/>
        public void Error(string location, string message)
        {
            Log(ReportSeverity.Useful, ReportType.Error, location, message);
        }

        /// <inheritdoc/>
        public void Warning(string location, string message)
        {
            Log(ReportSeverity.Useful, ReportType.Warning, location, message);
        }

        /// <inheritdoc/>
        public void WriteReportsToFile(string filePath)
        {
            WriteReportsToFile(filePath, new FileSystem());
        }

        /// <inheritdoc/>
        public void WriteReportsToFile(string filePath, out string message)
        {
            WriteReportsToFile(filePath, new FileSystem(), out message);
        }

        /// <inheritdoc/>
        public void WriteReportsToFile(string filePath, IFileSystem fileSystem)
        {
            WriteReportsToFile(filePath, fileSystem, out _);
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
                using (Stream stream = fileSystem.FileStream.Create(filePath, FileMode.Create))
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
