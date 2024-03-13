namespace Effanville.Common.Structure.Reporting
{
    /// <summary>
    /// Methods for setting data in a report.
    /// </summary>
    public interface IReport
    {
        /// <summary>
        /// Set the type of the Report.
        /// </summary>
        IReport Type(ReportType type);

        /// <summary>
        /// Set the location of the report.
        /// </summary>
        IReport Location(string location);

        /// <summary>
        /// Set the message of the report.
        /// </summary>
        void WithMessage(string message);
    }
}
