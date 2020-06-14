namespace StructureCommon.Reporting
{
    /// <summary>
    /// The type of the report.
    /// </summary>
    public enum ReportType
    {
        /// <summary>
        /// The default value, and the most serious, an error occurred.
        /// </summary>
        Error = 0,

        /// <summary>
        /// Less serious, something could be wrong so warning.
        /// </summary>
        Warning = 1,

        /// <summary>
        /// Just some information that could be useful or needed.
        /// </summary>
        Report = 2
    }
}
