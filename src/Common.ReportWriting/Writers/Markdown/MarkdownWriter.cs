using System.Text;

using Effanville.Common.ReportWriting.Documents;

namespace Effanville.Common.ReportWriting.Writers.Markdown
{
    /// <summary>
    /// Contains helper methods for writing markdown docs.
    /// </summary>
    internal sealed class MarkdownWriter : ITextWriter
    {
        /// <inheritdoc/>
        public void WriteParagraph(StringBuilder sb, IReadOnlyList<string> sentences, DocumentElement tag = DocumentElement.p)
        {
            if (tag == DocumentElement.br)
            {
                _ = sb.AppendLine();
                return;
            }

            if (tag == DocumentElement.p)
            {
                for (int index = 0; index < sentences.Count; index++)
                {
                    string line = sentences[index];
                    _ = sb.Append(line);
                    if (index != sentences.Count - 1)
                    {
                        _ = sb.Append(' ');
                    }
                }

                _ = sb.AppendLine();
                return;
            }

            _ = sb.AppendLine($"{HtmlTagToMdTitle(tag)} title");
        }

        /// <inheritdoc/>
        public void WriteTitle(StringBuilder sb, string title, DocumentElement tag = DocumentElement.h1)
            => _ = sb.AppendLine($"{HtmlTagToMdTitle(tag)} {title}");

        public static string HtmlTagToMdTitle(DocumentElement tag)
        {
            return tag switch
            {
                DocumentElement.h1 => "#",
                DocumentElement.h2 => "##",
                DocumentElement.h3 => "###",
                _ => "",
            };
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
