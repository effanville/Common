using System;

namespace Effanville.Common.Structure.Reporting
{
    /// <summary>
    /// A report structure containing information about a possible problem (or just info) happening in the program.
    /// </summary>
    public class ErrorReport : IComparable<ErrorReport>
    {
        /// <summary>
        /// The time the report was logged.
        /// </summary>
        public DateTime TimeStamp { get; internal set;  }


        /// <summary>
        /// How serious the report is, enabling a grading of the reports based on seriousness.
        /// </summary>
        public ReportSeverity ErrorSeverity { get; set; }

        /// <summary>
        /// The type of the report (is this an error or a warning etc).
        /// </summary>
        public ReportType ErrorType { get; set; }

        /// <summary>
        /// Where is this a report from.
        /// </summary>
        public string ErrorLocation { get; set; }

        /// <summary>
        /// Any extra text needed to aid info to the report.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Default constructor with no arguments.
        /// </summary>
        public ErrorReport()
        {
            TimeStamp = DateTime.Now;
            ErrorLocation = ReportLocation.Unknown.ToString();
        }

        /// <summary>
        /// Constructs an error report with default <see cref="ReportSeverity"/>.
        /// </summary>
        public ErrorReport(ReportType type, ReportLocation errorLocation, string message)
        {
            TimeStamp = DateTime.Now;
            ErrorType = type;
            ErrorLocation = errorLocation.ToString();
            Message = message;
        }

        /// <summary>
        /// Constructs a full report, setting all properties.
        /// </summary>
        public ErrorReport(ReportSeverity severity, ReportType type, ReportLocation errorLocation, string message)
         : this(type, errorLocation, message)
        {
            ErrorSeverity = severity;
        }
        
        /// <summary>
        /// Constructs an error report with default <see cref="ReportSeverity"/>.
        /// </summary>
        public ErrorReport(ReportType type, string errorLocation, string message)
        {
            TimeStamp = DateTime.Now;
            ErrorType = type;
            ErrorLocation = errorLocation;
            Message = message;
        }

        /// <summary>
        /// Constructs a full report, setting all properties.
        /// </summary>
        public ErrorReport(ReportSeverity severity, ReportType type, string errorLocation, string message)
            : this(type, errorLocation, message)
        {
            TimeStamp = DateTime.Now;
            ErrorSeverity = severity;
        }

        /// <summary>
        /// Output of error as a string. This does not include the severity of the report.
        /// </summary>
        public override string ToString() => $"[{TimeStamp}] [{ErrorType.ToLogString()}] [{ErrorLocation}] {Message}";

        /// <summary>
        /// Output the headers for the report in csv format.
        /// </summary>
        public static string ToCsvHeader() => "TimeStamp,Severity,ErrorType,Location,Message";
        
        /// <summary>
        /// Output of the report in csv format.
        /// </summary>
        /// <returns></returns>
        public string ToCsvString() => $"{TimeStamp},{ErrorSeverity},{ErrorType},{ErrorLocation},{Message}";
        
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

        /// <summary>
        /// Compare this error report to another error report.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(ErrorReport other)
        {
            if (!other.ErrorType.Equals(ErrorType))
            {
                return ErrorType.CompareTo(other.ErrorType);
            }

            return string.Equals(other.ErrorLocation, ErrorLocation) 
                ? string.Compare(Message, other.Message, StringComparison.InvariantCulture) 
                : string.Compare(other.ErrorLocation, ErrorLocation, StringComparison.InvariantCulture);
        }
    }
}
