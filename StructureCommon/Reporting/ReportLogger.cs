using System;
using System.IO;
using System.IO.Abstractions;
using System.Text;
using StructureCommon.Extensions;

namespace StructureCommon.Reporting
{
    /// <summary>
    /// Collection of standard reporting mechanisms.
    /// </summary>
    public class LogReporter : IReportLogger
    {
        private readonly Action<ReportSeverity, ReportType, ReportLocation, string> fLoggingAction;

        /// <inheritdoc/>
        public ErrorReports Reports
        {
            get;
            set;
        }

        /// <summary>
        /// Log an arbitrary message.
        /// </summary>
        public bool LogWithStrings(string severity, string type, string location, string message)
        {
            if (fLoggingAction != null)
            {
                fLoggingAction(severity.ToEnum<ReportSeverity>(), type.ToEnum<ReportType>(), location.ToEnum<ReportLocation>(), message);
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
        public bool Log(ReportSeverity severity, ReportType type, ReportLocation location, string message)
        {
            if (fLoggingAction != null)
            {
                fLoggingAction(severity, type, location, message);
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
        public bool LogUsefulWithStrings(string type, string location, string message)
        {
            if (fLoggingAction != null)
            {
                fLoggingAction(ReportSeverity.Useful, type.ToEnum<ReportType>(), location.ToEnum<ReportLocation>(), message);
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
                fLoggingAction(ReportSeverity.Useful, type, location, message);
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
                fLoggingAction(ReportSeverity.Useful, ReportType.Error, location, message);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Log an error message.
        /// </summary>
        public bool LogUsefulErrorWithStrings(string location, string message)
        {
            if (fLoggingAction != null)
            {
                fLoggingAction("Useful".ToEnum<ReportSeverity>(), "Error".ToEnum<ReportType>(), location.ToEnum<ReportLocation>(), message);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Constructor for reporting mechanisms. Parameter addReport is the report callback mechanism.
        /// </summary>
        public LogReporter(Action<string, string, string, string> addReport)
        {
            fLoggingAction = (detailLevel, type, location, message) => addReport(detailLevel.ToString(), type.ToString(), location.ToString(), message);
        }

        /// <summary>
        /// Constructor for reporting mechanisms. Parameter addReport is the report callback mechanism.
        /// </summary>
        public LogReporter(Action<ReportSeverity, ReportType, ReportLocation, string> addReport)
        {
            fLoggingAction = addReport;
        }

        /// <summary>
        /// Write the reports to a suitable file.
        /// </summary>
        public void WriteReportsToFile(string filePath)
        {
            WriteReportsToFile(filePath, new FileSystem());
        }

        /// <summary>
        /// Write the reports to a suitable file.
        /// </summary>
		public void WriteReportsToFile(string filePath, IFileSystem fileSystem)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(filePath))
                {
                    return;
                }
                using (var memoryStream = new MemoryStream())
                {

                    foreach (var report in Reports)
                    {
                        string line = DateTime.Now + "(" + report.ErrorType.ToString() + ")" + report.Message;
                        byte[] array = Encoding.UTF8.GetBytes(line + "\n");
                        memoryStream.Write(array, 0, array.Length);
                    }
                    FileStream fileWrite = new FileStream(filePath, FileMode.Create, System.IO.FileAccess.Write);
                    memoryStream.WriteTo(fileWrite);

                    fileWrite.Close();
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
