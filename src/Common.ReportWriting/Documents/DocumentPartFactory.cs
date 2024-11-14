namespace Effanville.Common.ReportWriting.Documents
{
    /// <inheritdoc cref="IDocumentPartFactory"/>
    public class DocumentPartFactory : IDocumentPartFactory
    {
        /// <inheritdoc />
        public DocumentPart Generate(DocumentType docType, DocumentElement element, string constituentString)
        {
            switch (element)
            {
                case DocumentElement.table:
                {
                    return TableInverter.InvertTable(docType, constituentString);
                }
                case DocumentElement.h1:
                case DocumentElement.h2:
                case DocumentElement.h3:
                case DocumentElement.h4:
                case DocumentElement.h5:
                case DocumentElement.h6:
                case DocumentElement.p:
                case DocumentElement.br:
                {
                    return new TextDocumentPart(docType, element, GetTextFromTotalString(docType, element, constituentString));
                }
                case DocumentElement.chart:
                case DocumentElement.None:
                default:
                    return new DocumentPart(docType, element, constituentString);
            }
        }

        public DocumentPart GenerateText(DocumentType docType, DocumentElement element, IReadOnlyList<string> constituentStrings)
        {
            switch (element)
            {
                case DocumentElement.h1:
                case DocumentElement.h2:
                case DocumentElement.h3:
                case DocumentElement.h4:
                case DocumentElement.h5:
                case DocumentElement.h6:
                case DocumentElement.p:
                case DocumentElement.br:
                {
                    return new TextDocumentPart(docType, element, constituentStrings);
                }
                case DocumentElement.table:
                case DocumentElement.chart:
                case DocumentElement.None:
                default:
                    return null;
            }
        }

        public DocumentPart GenerateTable(
            DocumentType docType,
            IReadOnlyList<string> headers,
            IReadOnlyList<IReadOnlyList<string>> rows,
            bool headerFirstColumn)
            => new TableDocumentPart(docType, headers, rows, headerFirstColumn);

        private static string GetTextFromTotalString(DocumentType docType, DocumentElement element, string constituentString)
        {
            switch (docType)
            {
                case DocumentType.Html:
                {
                    string stringForm = docType.StringForm(element);
                    int startIndex = constituentString.IndexOf(stringForm);
                    int endIndex = constituentString.IndexOf($"</{element}>");
                    if (startIndex == -1 || endIndex == -1)
                    {
                        return constituentString;
                    }
                    return constituentString.Substring(startIndex + stringForm.Length, endIndex - startIndex - stringForm.Length);
                }
                case DocumentType.Md:
                {
                    if (element.IsHeader())
                    {
                        string elementString = docType.StringForm(element);
                        if (constituentString.StartsWith(elementString))
                        {
                            return constituentString.Substring(elementString.Length).Trim();
                        }

                        return constituentString;
                    }

                    return constituentString;
                }
                default:
                {
                    return null;
                }
            }
        }
    }
}