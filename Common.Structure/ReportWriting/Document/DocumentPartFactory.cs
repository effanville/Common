namespace Common.Structure.ReportWriting.Document
{
    public static class DocumentPartFactory
    {
        public static DocumentPart Generate(DocumentType docType, DocumentElement element, string constituentString)
        {
            switch (element)
            {
                case DocumentElement.table:
                {
                    return new TableDocumentPart(docType, element, constituentString);
                }
                case DocumentElement.h1:
                case DocumentElement.h2:
                case DocumentElement.h3:
                case DocumentElement.h4:
                case DocumentElement.h5:
                case DocumentElement.h6:
                case DocumentElement.p:
                {
                    return new TextDocumentPart(docType, element, constituentString);
                }
                case DocumentElement.chart:
                case DocumentElement.None:
                default:
                    return new DocumentPart(docType, element, constituentString);
            }
        }
    }
}