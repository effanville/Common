using Effanville.Common.Structure.ReportWriting.Csv;
using Effanville.Common.Structure.ReportWriting.Html;
using Effanville.Common.Structure.ReportWriting.Markdown;

namespace Effanville.Common.Structure.ReportWriting
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
