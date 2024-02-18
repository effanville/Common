using System.Collections.Generic;
using System.Text;

namespace Common.Structure.ReportWriting.Document
{
    /// <summary>
    /// Provides a builder for creating documents.
    /// </summary>
    public sealed class DocumentBuilder
    {
        private readonly ITextWriter fTextWriter;
        private readonly ITableWriter fTableWriter;

        private readonly Document fDocument;

        /// <summary>
        /// Construct an instance.
        /// </summary>
        public DocumentBuilder(DocumentType docType)
        {
            ReportSettings settings = ReportSettings.Default();
            fTableWriter = TableWriterFactory.Create(docType);
            fTextWriter = TextWriterFactory.Create(docType, settings);
            fDocument = new Document(docType);
        }

        /// <summary>
        /// Write a title in the document.
        /// </summary>
        public DocumentBuilder WriteTitle(string title, DocumentElement tag)
        {
            StringBuilder sb = new StringBuilder();
            fTextWriter.WriteTitle(sb, title, tag);
            DocumentPart part = DocumentPartFactory.Generate(fDocument.DocType, tag, sb.ToString());
            fDocument.Add(part);
            return this;
        }

        /// <summary>
        /// Write a paragraph in the document
        /// </summary>
        public DocumentBuilder WriteParagraph(string[] sentences, DocumentElement tag = DocumentElement.p)
        {
            StringBuilder sb = new StringBuilder();
            fTextWriter.WriteParagraph(sb, sentences, tag);
            DocumentPart part = DocumentPartFactory.Generate(fDocument.DocType, tag, sb.ToString());
            fDocument.Add(part);
            return this;
        }

        /// <summary>
        /// Write a table in the document.
        /// </summary>
        public DocumentBuilder WriteTable<T>(IEnumerable<IEnumerable<T>> values, bool headerFirstColumn)
        {
            StringBuilder sb = new StringBuilder();
            fTableWriter.WriteTable(sb, values, headerFirstColumn);
            DocumentPart part = DocumentPartFactory.Generate(fDocument.DocType, DocumentElement.table, sb.ToString());
            fDocument.Add(part);
            return this;
        }

        /// <summary>
        /// Write a table in the document.
        /// </summary>
        public DocumentBuilder WriteTable<T>(IEnumerable<T> values, bool headerFirstColumn)
        {
            StringBuilder sb = new StringBuilder();
            fTableWriter.WriteTable(sb, values, headerFirstColumn);
            DocumentPart part = DocumentPartFactory.Generate(fDocument.DocType, DocumentElement.table, sb.ToString());
            fDocument.Add(part);
            return this;
        }

        /// <summary>
        /// Write a table in the document.
        /// </summary>
        public DocumentBuilder WriteTable<T>(IEnumerable<string> headerValues, IEnumerable<IEnumerable<T>> rowValues, bool headerFirstColumn)
        {
            StringBuilder sb = new StringBuilder();
            fTableWriter.WriteTable(sb, headerValues, rowValues, headerFirstColumn);
            DocumentPart part = DocumentPartFactory.Generate(fDocument.DocType, DocumentElement.table, sb.ToString());
            fDocument.Add(part);
            return this;
        }

        /// <summary>
        /// Write a table in the document.
        /// </summary>
        public DocumentBuilder WriteTable<T>(IEnumerable<string> headerValues, IEnumerable<T> rowValues, bool headerFirstColumn)
        {
            StringBuilder sb = new StringBuilder();
            fTableWriter.WriteTable(sb, headerValues, rowValues, headerFirstColumn);
            DocumentPart part = DocumentPartFactory.Generate(fDocument.DocType, DocumentElement.table, sb.ToString());
            fDocument.Add(part);
            return this;
        }

        /// <summary>
        /// Set the type of the document.
        /// </summary>
        public void SetDocumentType(DocumentType docType)
        {
            fDocument.SetDocumentType(docType);
        }

        /// <summary>
        /// Return the document that has been created so far.
        /// </summary>
        public Document GetDocument()
        {
            return fDocument;
        }

        /// <summary>
        /// Clears the enclosed document.
        /// </summary>
        public DocumentBuilder Clear()
        {
            fDocument.Clear();
            return this;
        }
    }
}
