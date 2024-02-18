using Effanville.Common.Structure.ReportWriting.Html;

namespace Effanville.Common.Structure.ReportWriting
{
    /// <summary>
    /// Contains factory methods for Chart writers.
    /// </summary>
    public static class ChartWriterFactory
    {
        /// <summary>
        /// Create an <see cref="IChartWriter"/>
        /// </summary>
        /// <param name="documentType">The type of chart writer to create.</param>
        public static IChartWriter Create(DocumentType documentType)
        {
            switch (documentType)
            {
                case DocumentType.Html:
                {
                    return new HtmlChartWriter();
                }
                case DocumentType.Csv:
                case DocumentType.Doc:
                case DocumentType.Pdf:
                case DocumentType.Md:
                default:
                {
                    return null;
                }
            }
        }
    }
}
