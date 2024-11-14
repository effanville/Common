using Effanville.Common.ReportWriting.Documents;
using Effanville.Common.ReportWriting.Writers.Csv;
using Effanville.Common.ReportWriting.Writers.Html;
using Effanville.Common.ReportWriting.Writers.Markdown;

namespace Effanville.Common.ReportWriting.Writers;

/// <summary>
/// Contains factory methods for creating <see cref="ITextWriter"/>s.
/// </summary>
public class TextWriterFactory
{
    /// <summary>
    /// Create an instance of a <see cref="ITextWriter"/>.
    /// </summary>
    public ITextWriter Create(DocumentType exportType, DocumentWriterSettings settings)
    {
        switch (exportType)
        {
            case DocumentType.Html:
            {
                return new HtmlWriter(!settings.UseDefaultStyle, settings.UseScripts);
            }
            case DocumentType.Csv:
            {
                return new CsvTextWriter();
            }
            case DocumentType.Md:
            {
                return new MarkdownWriter();
            }
            case DocumentType.Doc:
            case DocumentType.Pdf:
            default:
            {
                return null;
            }
        }
    }
}
