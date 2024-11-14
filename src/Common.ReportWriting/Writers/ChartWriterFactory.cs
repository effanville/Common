using Effanville.Common.ReportWriting.Documents;
using Effanville.Common.ReportWriting.Writers.Html;

namespace Effanville.Common.ReportWriting.Writers
{
    /// <summary>
    /// Contains factory methods for Chart writers.
    /// </summary>
    public class ChartWriterFactory
    {
        /// <summary>
        /// Create an <see cref="IChartWriter"/>
        /// </summary>
        /// <param name="documentType">The type of chart writer to create.</param>
        public IChartWriter Create(DocumentType documentType)
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
