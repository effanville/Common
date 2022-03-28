using Common.Structure.ReportWriting.Csv;
using Common.Structure.ReportWriting.Html;
using Common.Structure.ReportWriting.Markdown;

namespace Common.Structure.ReportWriting
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
