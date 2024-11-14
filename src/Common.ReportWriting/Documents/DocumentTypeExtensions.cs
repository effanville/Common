namespace Effanville.Common.ReportWriting.Documents
{
    /// <summary>
    /// Extension methods for <see cref="DocumentType"/>s.
    /// </summary>
    public static class DocumentTypeExtensions
    {
        /// <summary>
        /// Generate the string form for the <see cref="DocumentType"/> and the <see cref="DocumentElement"/>
        /// </summary>
        public static string StringForm(this DocumentType docType, DocumentElement docElement)
            => DocumentElementString(docType, docElement);

        private static string DocumentElementString(DocumentType docType, DocumentElement docElement)
        {
            switch (docType)
            {
                case DocumentType.Html:
                {
                    if (docElement == DocumentElement.chart)
                    {
                        return "<div>";
                    }
                    return $"<{docElement}>";
                }
                case DocumentType.Md:
                {
                    switch (docElement)
                    {
                        case DocumentElement.h1:
                        {
                            return "#";
                        }
                        case DocumentElement.h2:
                        {
                            return "##";
                        }
                        case DocumentElement.h3:
                        case DocumentElement.h4:
                        case DocumentElement.h5:
                        case DocumentElement.h6:
                        {
                            return "###";
                        }
                        case DocumentElement.table:
                        {
                            return "|";
                        }
                        case DocumentElement.chart:
                        {
                            return "ChartChart";
                        }
                        case DocumentElement.None:
                        case DocumentElement.p:
                        default:
                        {
                            return Constants.EnvNewLine;
                        }
                    }
                }
                case DocumentType.Doc:
                case DocumentType.Pdf:
                case DocumentType.Csv:
                default:
                    return null;
            }
        }
    }
}
