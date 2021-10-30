namespace Common.Structure.ReportWriting
{
    /// <summary>
    /// The type of file that is being exported.
    /// </summary>
    public enum ReportType
    {
        /// <summary>
        /// An Html file type
        /// </summary>
        Html,

        /// <summary>
        /// A Csv file type.
        /// </summary>
        Csv,

        /// <summary>
        /// A Word document type.
        /// </summary>
        Doc,

        /// <summary>
        /// Portable document format.
        /// </summary>
        Pdf
    }
}