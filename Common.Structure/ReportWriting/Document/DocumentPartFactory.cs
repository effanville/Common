namespace Common.Structure.ReportWriting.Document
{
    public static class DocumentPartFactory
    {
        public static DocumentPart Generate(DocumentType docType, DocumentElement element, string constituentString)
        {
            switch (element)
            {
                case DocumentElement.table:
                    return new TableDocumentPart(docType, element, constituentString);
                default:
                    return new DocumentPart(docType, element, constituentString);
            }
        }
    }
}