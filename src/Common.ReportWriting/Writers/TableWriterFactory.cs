using Effanville.Common.ReportWriting.Documents;
using Effanville.Common.ReportWriting.Writers.Csv;
using Effanville.Common.ReportWriting.Writers.Html;
using Effanville.Common.ReportWriting.Writers.Markdown;

namespace Effanville.Common.ReportWriting.Writers;

/// <summary>
/// Factory for creating an <see cref="ITableWriter"/>.
/// </summary>
public class TableWriterFactory
{
    /// <summary>
    /// Create an instance of a TableWriter.
    /// </summary>
    /// <param name="exportType"></param>
    /// <returns></returns>
    public ITableWriter Create(DocumentType exportType)
    {
        switch (exportType)
        {
            case DocumentType.Html:
            {
                return new HtmlTableWriter();
            }
            case DocumentType.Csv:
            {
                return new CsvTableWriter();
            }
            case DocumentType.Md:
            {
                return new MarkdownTableWriter();
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
