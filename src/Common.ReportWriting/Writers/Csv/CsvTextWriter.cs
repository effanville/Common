using System.Text;

using Effanville.Common.ReportWriting.Documents;

namespace Effanville.Common.ReportWriting.Writers.Csv;

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
    public void WriteParagraph(StringBuilder sb, IReadOnlyList<string> sentences, DocumentElement tag = DocumentElement.p)
        => _ = sb.AppendLine(string.Join(",", sentences));

    /// <inheritdoc/>
    public void WriteTitle(StringBuilder sb, string title, DocumentElement tag = DocumentElement.h1)
    {
        _ = sb.AppendLine("");
        _ = sb.AppendLine(title);
        _ = sb.AppendLine("");
    }
}
