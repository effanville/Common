using System.Text;

namespace Common.Structure.ReportWriting
{
    /// <summary>
    /// Contains methods for writing text.
    /// </summary>
    public interface ITextWriter
    {
        /// <summary>
        /// Write a header for the text document.
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="pageTitle"></param>
        /// <param name="useColours"></param>
        void WriteHeader(StringBuilder sb, string pageTitle, bool useColours);

        /// <summary>
        /// Writes a title line to the file.
        /// </summary>
        /// <param name="sb">The writer to use.</param>
        /// <param name="title">The title string to write.</param>
        /// <param name="tag">The specific <see cref="DocumentElement"/> to use in this title.</param>
        void WriteTitle(StringBuilder sb, string title, DocumentElement tag = DocumentElement.h1);
        /// <summary>
        /// Writes a paragraph to the file.
        /// </summary>
        /// <param name="sb">The writer to use</param>
        /// <param name="sentence">The sentence to export.</param>
        /// <param name="tag">The <see cref="DocumentElement"/> to use.</param>
        void WriteParagraph(StringBuilder sb, string[] sentence, DocumentElement tag = DocumentElement.p);

        /// <summary>
        /// Creates a generic footer for the document page.
        /// </summary>
        void WriteFooter(StringBuilder sb);
    }
}
