using System.Text;

namespace Effanville.Common.Structure.ReportWriting.Markdown
{
    /// <summary>
    /// Contains helper methods for writing markdown docs.
    /// </summary>
    internal sealed class MarkdownWriter : ITextWriter
    {
        /// <inheritdoc/>
        public void WriteParagraph(StringBuilder sb, string[] sentence, DocumentElement tag = DocumentElement.p)
        {
            if (tag != DocumentElement.p)
            {
                _ = sb.AppendLine($"{HtmlTagToMdTitle(tag)} title");
            }

            for (int index = 0; index < sentence.Length; index++)
            {
                string line = sentence[index];
                _ = sb.Append(line);
                if (index != sentence.Length - 1)
                {
                    _ = sb.Append(' ');
                }
            }

            _ = sb.AppendLine();
        }

        /// <inheritdoc/>
        public void WriteTitle(StringBuilder sb, string title, DocumentElement tag = DocumentElement.h1)
        {
            _ = sb.AppendLine($"{HtmlTagToMdTitle(tag)} {title}");
        }

        public static string HtmlTagToMdTitle(DocumentElement tag)
        {
            switch (tag)
            {
                case DocumentElement.h1:
                    return "#";
                case DocumentElement.h2:
                    return "##";
                case DocumentElement.h3:
                    return "###";
                case DocumentElement.p:
                default:
                    return "";
            }
        }

        /// <inheritdoc/>
        public void WriteHeader(StringBuilder sb, string pageTitle, bool useColours)
        {
        }

        /// <inheritdoc/>
        public void WriteFooter(StringBuilder sb)
        {
        }
    }
}
