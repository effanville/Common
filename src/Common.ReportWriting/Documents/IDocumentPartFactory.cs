namespace Effanville.Common.ReportWriting.Documents
{
    /// <summary>
    /// Provides methods for generating parts of documents.
    /// </summary>
    public interface IDocumentPartFactory
    {
        /// <summary>
        /// Generates different document parts.
        /// </summary>
        DocumentPart Generate(DocumentType docType, DocumentElement element, string constituentString);

        DocumentPart GenerateText(DocumentType docType, DocumentElement element, IReadOnlyList<string> constituentStrings);


        DocumentPart GenerateTable(DocumentType docType,
            IReadOnlyList<string> headers,
            IReadOnlyList<IReadOnlyList<string>> rows,
            bool headerFirstColumn);
    }
}