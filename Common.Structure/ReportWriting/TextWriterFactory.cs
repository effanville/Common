using Common.Structure.ReportWriting.Csv;
using Common.Structure.ReportWriting.Html;
using Common.Structure.ReportWriting.Markdown;

namespace Common.Structure.ReportWriting
{
    /// <summary>
    /// Contains factory methods for creating <see cref="ITextWriter"/>s.
    /// </summary>
    public static class TextWriterFactory
    {
        /// <summary>
        /// Create an instance of a <see cref="ITextWriter"/>.
        /// </summary>
        public static ITextWriter Create(DocumentType exportType, ReportSettings settings)
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
}
