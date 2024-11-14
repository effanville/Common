namespace Effanville.Common.ReportWriting.Documents
{
    /// <summary>
    /// Provides a builder for creating documents.
    /// </summary>
    public sealed class DocumentBuilder
    {
        private readonly IDocumentPartFactory _documentPartFactory = new DocumentPartFactory();

        private Document _document;

        /// <summary>
        /// Construct an instance.
        /// </summary>
        public DocumentBuilder(DocumentType docType, string title)
        {
            _document = new Document(docType, title);
        }

        /// <summary>
        /// Write a title in the document.
        /// </summary>
        public DocumentBuilder WriteTitle(string title, DocumentElement tag)
        {
            DocumentPart part = _documentPartFactory.Generate(_document.DocType, tag, title);
            _document.Add(part);
            return this;
        }

        /// <summary>
        /// Write a paragraph in the document
        /// </summary>
        public DocumentBuilder WriteParagraph(string sentence, DocumentElement tag = DocumentElement.p)
        {
            DocumentPart part = _documentPartFactory.Generate(_document.DocType, tag, sentence);
            _document.Add(part);
            return this;
        }

        /// <summary>
        /// Write a paragraph in the document
        /// </summary>
        public DocumentBuilder WriteParagraph(IReadOnlyList<string> sentences, DocumentElement tag = DocumentElement.p)
        {
            DocumentPart part = _documentPartFactory.GenerateText(_document.DocType, tag, sentences);
            _document.Add(part);
            return this;
        }

        /// <summary>
        /// Write a table in the document.
        /// </summary>
        public DocumentBuilder WriteTable(
            IReadOnlyList<string> headerValues,
            IReadOnlyList<IReadOnlyList<string>> rowValues, bool headerFirstColumn)
        {
            DocumentPart part = _documentPartFactory.GenerateTable(
                _document.DocType,
                headerValues,
                rowValues,
                headerFirstColumn);
            _document.Add(part);
            return this;
        }

        /// <summary>
        /// Writes a number of newlines into the document.
        /// </summary>
        public DocumentBuilder WriteSpace(int numLines)
        {
            int numberAdded = 0;
            while (numberAdded < numLines)
            {
                _document.Add(_documentPartFactory.Generate(_document.DocType, DocumentElement.br, string.Empty));
                numberAdded++;
            }

            return this;
        }

        /// <summary>
        /// Return the document that has been created so far.
        /// </summary>
        public Document GetDocument() => _document;

        /// <summary>
        /// Clears the enclosed document.
        /// </summary>
        public DocumentBuilder Clear()
        {
            _document = new Document(_document.DocType, _document.Title);
            return this;
        }
    }
}
