using System;
using System.IO;
using System.IO.Abstractions;

namespace Common.Structure.Reporting
{
    /// <summary>
    /// Collection of standard reporting mechanisms.
    /// </summary>
    public class LogReporter : IReportLogger
    {
        private readonly Action<ReportSeverity, ReportType, string, string> fLoggingAction;

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
            fLoggingAction = addReport;
        }

        private void AddReport(ReportSeverity severity, ReportType type, string location, string message)
        {
            if (SaveInternally)
            {
                Reports.AddErrorReport(severity, type, location, message);
            }
        }

        /// <inheritdoc />
        public void Log(ReportType type, string location, string message)
        {
            fLoggingAction?.Invoke(ReportSeverity.Useful, type, location, message);
            AddReport(ReportSeverity.Useful, type, location, message);
        }

        /// <inheritdoc />
        public void Log(ReportSeverity severity, ReportType type, string location, string message)
        {
            fLoggingAction?.Invoke(severity, type, location, message);
            AddReport(severity, type, location, message);
        }

        /// <inheritdoc />
        public bool Log(ReportSeverity severity, ReportType type, ReportLocation location, string message)
        {
            if (fLoggingAction != null)
            {
                fLoggingAction(severity, type, location.ToString(), message);
                AddReport(severity, type, location.ToString(), message);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Log an arbitrary message.
        /// </summary>
        public bool LogUseful(ReportType type, ReportLocation location, string message)
        {
            if (fLoggingAction != null)
            {
                fLoggingAction(ReportSeverity.Useful, type, location.ToString(), message);
                AddReport(ReportSeverity.Useful, type, location.ToString(), message);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Log an arbitrary message.
        /// </summary>
        public bool LogUsefulError(ReportLocation location, string message)
        {
            if (fLoggingAction != null)
            {
                fLoggingAction(ReportSeverity.Useful, ReportType.Error, location.ToString(), message);
                AddReport(ReportSeverity.Useful, ReportType.Error, location.ToString(), message);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public void Error(string location, string message)
        {
            fLoggingAction?.Invoke(ReportSeverity.Useful, ReportType.Error, location, message);
            AddReport(ReportSeverity.Useful, ReportType.Error, location, message);
        }

        /// <inheritdoc/>
        public void Warning(string location, string message)
        {
            fLoggingAction?.Invoke(ReportSeverity.Useful, ReportType.Warning, location, message);
            AddReport(ReportSeverity.Useful, ReportType.Error, location, message);
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
