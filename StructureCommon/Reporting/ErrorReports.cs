using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace StructureCommon.Reporting
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
        private readonly List<ErrorReport> fReports;

        /// <summary>
        /// Creates an instance with an empty list of reports.
        /// </summary>
        public ErrorReports()
        {
            fReports = new List<ErrorReport>();
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"These reports contain {Count()} entries: {GetReports(ReportType.Error)} errors, {GetReports(ReportType.Warning)} warnings and {GetReports(ReportType.Information)} reports.";
        }

        /// <summary>
        /// Number of reports in total.
        /// </summary>
        public int Count()
        {
            lock (lockObject)
            {
                return fReports.Count;
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
                return fReports.Any();
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
                fReports.AddRange(reports.GetReports());
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
                fReports.Add(new ErrorReport(severity, type, location, newReport));
            }
        }

        /// <summary>
        /// Adds a report to the existing list
        /// </summary>
        public void AddReport(string newReport, ReportLocation location = ReportLocation.Unknown)
        {
            AddGeneralReport(ReportType.Information, location, newReport);
        }

        /// <summary>
        /// Adds an Error report to the existing list
        /// </summary>
        public void AddError(string newReport, ReportLocation location = ReportLocation.Unknown)
        {
            AddGeneralReport(ReportType.Error, location, newReport);
        }

        /// <summary>
        /// Adds a Warning report to the existing list
        /// </summary>
        public void AddWarning(string newReport, ReportLocation location = ReportLocation.Unknown)
        {
            AddGeneralReport(ReportType.Warning, location, newReport);
        }

        /// <summary>
        /// Adds a report of any type to the existing list
        /// </summary>
        private void AddGeneralReport(ReportType type, ReportLocation location, string newReport)
        {
            lock (lockObject)
            {
                fReports.Add(new ErrorReport(type, location, newReport));
            }
        }

        /// <summary>
        /// Returns a copy of all reports held in the structure.
        /// </summary>
        public List<ErrorReport> GetReports()
        {
            lock (lockObject)
            {
                return new List<ErrorReport>(fReports);
            }
        }

        /// <summary>
        /// Returns all reports of a certain severity from the system.
        /// </summary>
        public List<ErrorReport> GetReports(ReportSeverity severity = ReportSeverity.Useful)
        {
            switch (severity)
            {
                case ReportSeverity.Critical:
                    return GetReports().Where(report => report.ErrorSeverity == severity).ToList();
                default:
                case ReportSeverity.Useful:
                    return GetReports().Where(report => report.ErrorSeverity == severity || report.ErrorSeverity == ReportSeverity.Critical).ToList();
                case ReportSeverity.Detailed:
                    return GetReports();
            }
        }

        /// <summary>
        /// Returns a list of reports with location the same as the specified location.
        /// </summary>
        public List<ErrorReport> GetReports(ReportLocation location)
        {
            return GetReports().Where(report => report.ErrorLocation == location).ToList();
        }

        /// <summary>
        /// Returns a list of reports with ReportType matching the desired type.
        /// </summary>
        public List<ErrorReport> GetReports(ReportType reportType)
        {
            return GetReports().Where(report => report.ErrorType == reportType).ToList();
        }

        /// <summary>
        /// Removes element at index <param name="i"/>
        /// </summary>
        public void RemoveReport(int i)
        {
            lock (lockObject)
            {
                if (i >= 0 && i < fReports.Count)
                {
                    fReports.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Removes all reports held in the list.
        /// </summary>
        public void Clear()
        {
            lock (lockObject)
            {
                fReports.Clear();
            }
        }

        public ErrorReport this[int index]
        {
            get
            {
                lock (lockObject)
                {
                    return fReports[index];
                }
            }
        }

        public IEnumerator<ErrorReport> GetEnumerator()
        {
            return fReports.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
