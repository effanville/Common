using System.Text;

namespace Effanville.Common.Structure.ReportWriting.Csv
{
    internal sealed class CsvTextWriter : ITextWriter
    {
        /// <inheritdoc/>
        public void WriteFooter(StringBuilder sb)
        {
        }

        /// <inheritdoc/>
        public void WriteHeader(StringBuilder sb, string pageTitle, bool useColours)
        {
        }

        /// <inheritdoc/>
        public void WriteParagraph(StringBuilder sb, string[] sentence, DocumentElement tag = DocumentElement.p)
        {
            _ = sb.AppendLine(string.Join(",", sentence));
        }

        /// <inheritdoc/>
        public void WriteTitle(StringBuilder sb, string title, DocumentElement tag = DocumentElement.h1)
        {
            _ = sb.AppendLine("");
            _ = sb.AppendLine(title);
            _ = sb.AppendLine("");
        }
    }
}
