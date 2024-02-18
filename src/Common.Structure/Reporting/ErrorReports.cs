using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;

namespace Common.Structure.Reporting
{
    /// <summary>
    /// Collection of ErrorReport with added query functionality to tell the user what is happening.
    /// </summary>
    public class ErrorReports : IEnumerable<ErrorReport>
    {
        private readonly object lockObject = new object();

        /// <summary>
        /// List of all reports held.
        /// </summary>
        private readonly List<ErrorReport> _reports;

        /// <summary>
        /// Creates an instance with an empty list of reports.
        /// </summary>
        public ErrorReports()
        {
            _reports = new List<ErrorReport>();
        }

        /// <summary>
        /// Creates an instance with an list of reports from an
        /// existing list.
        /// </summary>
        public ErrorReports(List<ErrorReport> errorReports)
        {
            _reports = errorReports;
        }

        /// <inheritdoc/>
        public override string ToString()
            =>
                $"These reports contain {Count()} entries: {GetReports(ReportType.Error)} errors, {GetReports(ReportType.Warning)} warnings and {GetReports(ReportType.Information)} reports.";

        /// <summary>
        /// Number of reports in total.
        /// </summary>
        public int Count()
        {
            lock (lockObject)
            {
                return _reports.Count;
            }
        }

        /// <summary>
        /// Determines if there are any reports in the list.
        /// </summary>
        /// <returns></returns>
        public bool Any()
        {
            lock (lockObject)
            {
                return _reports.Any();
            }
        }

        /// <summary>
        /// Adds the reports from another repository of reports to this one.
        /// </summary>
        /// <param name="reports"></param>
        public void AddReports(ErrorReports reports)
        {
            lock (lockObject)
            {
                _reports.AddRange(reports.GetReports());
            }
        }

        /// <summary>
        /// Adds a report to the existing list
        /// </summary>
        public void AddReportFromStrings(string severity, string type, string location, string message)
        {
            _ = ReportSeverity.TryParse(severity, out ReportSeverity reportSeverity);
            _ = ReportType.TryParse(type, out ReportType typeOfReport);
            _ = ReportLocation.TryParse(location, out ReportLocation locationType);

            AddErrorReport(reportSeverity, typeOfReport, locationType, message);
        }

        /// <summary>
        /// Adds a report of any type to the existing list
        /// </summary>
        public void AddErrorReport(ReportSeverity severity, ReportType type, ReportLocation location, string newReport)
        {
            lock (lockObject)
            {
                _reports.Add(new ErrorReport(severity, type, location, newReport));
            }
        }

        /// <summary>
        /// Adds a report of any type to the existing list
        /// </summary>
        public void AddErrorReport(ReportSeverity severity, ReportType type, string location, string newReport)
        {
            lock (lockObject)
            {
                _reports.Add(new ErrorReport(severity, type, location, newReport));
            }
        }

        /// <summary>
        /// Returns a copy of all reports held in the structure.
        /// </summary>
        public List<ErrorReport> GetReports()
        {
            lock (lockObject)
            {
                return new List<ErrorReport>(_reports);
            }
        }

        /// <summary>
        /// Returns all reports of a certain severity from the system.
        /// </summary>
        public List<ErrorReport> GetReports(ReportSeverity severity)
        {
            switch (severity)
            {
                case ReportSeverity.Critical:
                    return GetReports().Where(report => report.ErrorSeverity == severity).ToList();
                default:
                case ReportSeverity.Useful:
                    return GetReports().Where(report =>
                        report.ErrorSeverity == severity || report.ErrorSeverity == ReportSeverity.Critical).ToList();
                case ReportSeverity.Detailed:
                    return GetReports();
            }
        }

        /// <summary>
        /// Returns a list of reports with location the same as the specified location.
        /// </summary>
        public List<ErrorReport> GetReports(ReportLocation location)
            => GetReports(location.ToString());

        /// <summary>
        /// Returns a list of reports with location the same as the specified location.
        /// </summary>
        public List<ErrorReport> GetReports(string location)
            => GetReports().Where(report => report.ErrorLocation == location).ToList();

        /// <summary>
        /// Returns a list of reports with ReportType matching the desired type.
        /// </summary>
        public List<ErrorReport> GetReports(ReportType reportType)
            => GetReports().Where(report => report.ErrorType == reportType).ToList();

        /// <summary>
        /// Removes element at index <param name="i"/>
        /// </summary>
        public void RemoveReport(int i)
        {
            lock (lockObject)
            {
                if (i >= 0 && i < _reports.Count)
                {
                    _reports.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Removes the selected element
        /// </summary>
        public int RemoveReports(IList<ErrorReport> reports)
        {
            lock (lockObject)
            {
                int numberRemovals = 0;
                foreach (var report in reports)
                {
                    numberRemovals += _reports.Remove(report) ? 1 : 0;
                }

                return numberRemovals;
            }
        }
        
        /// <summary>
        /// Removes the selected element
        /// </summary>
        public bool RemoveReport(ErrorReport report)
        {
            lock (lockObject)
            {
                return _reports.Remove(report);
            }
        }

        /// <summary>
        /// Removes all reports held in the list.
        /// </summary>
        public void Clear()
        {
            lock (lockObject)
            {
                _reports.Clear();
            }
        }

        /// <summary>
        /// Return the error report at the index specified.
        /// </summary>
        public ErrorReport this[int index]
        {
            get
            {
                lock (lockObject)
                {
                    return _reports[index];
                }
            }
        }

        /// <inheritdoc/>
        public IEnumerator<ErrorReport> GetEnumerator() => _reports.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Saves the reports to file.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileSystem"></param>
        public void Save(string filePath, IFileSystem fileSystem)
        {
            using (Stream stream = fileSystem.FileStream.Create(filePath, FileMode.Create))
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.WriteLine(ErrorReport.ToCsvHeader());
                foreach (ErrorReport report in GetReports())
                {
                    writer.WriteLine(report.ToCsvString());
                }
            }
        }
    }
}