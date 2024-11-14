namespace Effanville.Common.ReportWriting.Documents
{
    /// <summary>
    /// The type of file that is being exported.
    /// </summary>
    public enum DocumentType
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
        Pdf,

        /// <summary>
        /// A Markdown file.
        /// </summary>
        Md
    }
}