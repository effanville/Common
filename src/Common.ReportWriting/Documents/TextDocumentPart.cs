namespace Effanville.Common.ReportWriting.Documents
{
    /// <summary>
    /// A part of a document that contains text only.
    /// </summary>
    public sealed class TextDocumentPart : DocumentPart
    {
        /// <summary>
        /// The text for this part of the document.
        /// </summary>
        public IReadOnlyList<string> Text { get; }

        /// <summary>
        /// Construct an instance.
        /// </summary>
        public TextDocumentPart(DocumentType docType, DocumentElement element, string sentence)
            : base(docType, element, sentence)
        {
            Text = new List<string> { sentence };
        }

        /// <summary>
        /// Construct an instance.
        /// </summary>
        public TextDocumentPart(DocumentType docType, DocumentElement element, IReadOnlyList<string> sentences)
            : base(docType, element, string.Join(" ", sentences))
        {
            Text = sentences;
        }
    }
}