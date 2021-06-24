using System;

namespace Common.Structure.Reporting
{
    /// <summary>
    /// A report structure containing information about a possible problem (or just info) happening in the program.
    /// </summary>
    public class ErrorReport : IComparable<ErrorReport>
    {
        /// <summary>
        /// The time the report was logged.
        /// </summary>
        public DateTime TimeStamp;


        /// <summary>
        /// How serious the report is, enabling a grading of the reports based on seriousness.
        /// </summary>
        public ReportSeverity ErrorSeverity
        {
            get;
            set;
        }

        /// <summary>
        /// The type of the report (is this an error or a warning etc).
        /// </summary>
        public ReportType ErrorType
        {
            get;
            set;
        }

        /// <summary>
        /// Where is this a report from.
        /// </summary>
        public ReportLocation ErrorLocation
        {
            get;
            set;
        }

        /// <summary>
        /// Any extra text needed to aid info to the report.
        /// </summary>
        public string Message
        {
            get;
            set;
        }

        /// <summary>
        /// Default constructor with no arguments.
        /// </summary>
        public ErrorReport()
        {
            TimeStamp = DateTime.Now;
        }

        /// <summary>
        /// Constructs an error report with default <see cref="ReportSeverity"/>.
        /// </summary>
        public ErrorReport(ReportType type, ReportLocation errorLocation, string message)
        {
            TimeStamp = DateTime.Now;
            ErrorType = type;
            ErrorLocation = errorLocation;
            Message = message;
        }

        /// <summary>
        /// Constructs a full report, setting all properties.
        /// </summary>
        public ErrorReport(ReportSeverity severity, ReportType type, ReportLocation errorLocation, string message)
        {
            TimeStamp = DateTime.Now;
            ErrorSeverity = severity;
            ErrorType = type;
            ErrorLocation = errorLocation;
            Message = message;
        }

        /// <summary>
        /// Output of error as a string. This does not include the severity of the report.
        /// </summary>
        public override string ToString()
        {
            return $"[{TimeStamp}]{ErrorType} - {ErrorLocation} - {Message}";

        }

        /// <summary>
        /// Method of comparison
        /// </summary>
        public int CompareTo(object obj)
        {
            if (obj is ErrorReport other)
            {
                return CompareTo(other);
            }

            return 0;
        }

        public int CompareTo(ErrorReport other)
        {
            if (other.ErrorType.Equals(ErrorType))
            {
                if (other.ErrorLocation.Equals(ErrorLocation))
                {
                    return string.Compare(Message, other.Message);
                }

                return other.ErrorLocation.CompareTo(ErrorLocation);
            }

            return ErrorType.CompareTo(other.ErrorType);
        }
    }
}
