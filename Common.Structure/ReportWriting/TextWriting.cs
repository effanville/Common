using System.Text;

namespace Common.Structure.ReportWriting
{
    /// <summary>
    /// Class containing default routines to write to any file type in <see cref="DocumentType"/>.
    /// </summary>
    public static class TextWriting
    {
        /// <summary>
        /// Writes a paragraph to the file.
        /// </summary>
        /// <param name="sb">The writer to use</param>
        /// <param name="documentType">The type of file to export to.</param>
        /// <param name="sentence">The sentence to export.</param>
        /// <param name="tag">The <see cref="DocumentElement"/> to use.</param>
        public static void WriteParagraph(StringBuilder sb, DocumentType documentType, string[] sentence, DocumentElement tag = DocumentElement.p)
        {
            var textWriter = TextWriterFactory.Create(documentType);
            textWriter.WriteParagraph(sb, sentence, tag);
        }

        /// <summary>
        /// Writes a title line to the file.
        /// </summary>
        /// <param name="sb">The writer to use.</param>
        /// <param name="documentType">The type of file to export to.</param>
        /// <param name="title">The title string to write.</param>
        /// <param name="tag">The specific <see cref="DocumentElement"/> to use in this title.</param>
        public static void WriteTitle(StringBuilder sb, DocumentType documentType, string title, DocumentElement tag = DocumentElement.h1)
        {
            var textWriter = TextWriterFactory.Create(documentType);
            textWriter.WriteTitle(sb, title, tag);
        }

        /// <summary>
        /// Creates a generic header with default styles for a html page.
        /// </summary>
        /// <param name="sb">The stringbuilder to write the page with</param>
        /// <param name="documentType">The type of document to write</param>
        /// <param name="pageTitle">A title to give the page</param>
        /// <param name="useColours">Whether to use colour styling or not.</param>
        public static void WriteHeader(StringBuilder sb, DocumentType documentType, string pageTitle, bool useColours)
        {
            var textWriter = TextWriterFactory.Create(documentType);
            textWriter.WriteHeader(sb, pageTitle, useColours);
        }

        /// <summary>
        /// Creates a generic footer for a html page.
        /// </summary>
        /// <param name="sb">The stringbuilder to write the page with</param>
        /// <param name="documentType">The type of document to write</param>
        public static void WriteFooter(StringBuilder sb, DocumentType documentType)
        {
            var textWriter = TextWriterFactory.Create(documentType);
            textWriter.WriteFooter(sb);
        }
    }
}
