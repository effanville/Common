using Effanville.Common.Structure.ReportWriting.Csv;
using Effanville.Common.Structure.ReportWriting.Html;
using Effanville.Common.Structure.ReportWriting.Markdown;

namespace Effanville.Common.Structure.ReportWriting
{
    /// <summary>
    /// Factory for creating an <see cref="ITableWriter"/>.
    /// </summary>
    public static class TableWriterFactory
    {
        /// <summary>
        /// Create an instance of a TableWriter.
        /// </summary>
        /// <param name="exportType"></param>
        /// <returns></returns>
        public static ITableWriter Create(DocumentType exportType)
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
}
