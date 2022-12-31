using System.Collections.Generic;
using System.Text;

namespace Common.Structure.ReportWriting.Document
{
    public sealed class DocumentBuilder
    {
        private readonly ITextWriter fTextWriter;
        private readonly ITableWriter fTableWriter;
        private readonly IChartWriter fChartWriter;
        private readonly ReportSettings fSettings;

        private Document fDocument;
        public DocumentBuilder(DocumentType docType)
        {
            var settings = ReportSettings.Default();
            fTableWriter = TableWriterFactory.Create(docType);
            fTextWriter = TextWriterFactory.Create(docType, settings);
            fChartWriter = ChartWriterFactory.Create(docType);
            fSettings = settings;
            fDocument = new Document(docType);
        }

        public DocumentBuilder WriteTitle(string title, DocumentElement tag)
        {
            var sb = new StringBuilder();
            fTextWriter.WriteTitle(sb, title, tag);
            var part = DocumentPartFactory.Generate(fDocument.DocType, tag, sb.ToString());
            fDocument.Add(part);
            return this;
        }

        public DocumentBuilder WriteParagraph(string[] sentences, DocumentElement tag = DocumentElement.p)
        {
            var sb = new StringBuilder();
            fTextWriter.WriteParagraph(sb, sentences, tag);
            var part = DocumentPartFactory.Generate(fDocument.DocType, tag, sb.ToString());
            fDocument.Add(part);
            return this;
        }

        public DocumentBuilder WriteTable<T>(IEnumerable<IEnumerable<T>> values, TableSettings settings)
        {
            var sb = new StringBuilder();
            fTableWriter.WriteTable(sb, values, settings);
            var part = DocumentPartFactory.Generate(fDocument.DocType, DocumentElement.table, sb.ToString());
            fDocument.Add(part);
            return this;
        }

        public DocumentBuilder WriteTable<T>(IEnumerable<T> values, TableSettings settings)
        {
            var sb = new StringBuilder();
            fTableWriter.WriteTable(sb, values, settings);
            var part = DocumentPartFactory.Generate(fDocument.DocType, DocumentElement.table, sb.ToString());
            fDocument.Add(part);
            return this;
        }

        public DocumentBuilder WriteTable<T>(IEnumerable<string> headerValues, IEnumerable<IEnumerable<T>> rowValues, TableSettings settings)
        {
            var sb = new StringBuilder();
            fTableWriter.WriteTable(sb, headerValues, rowValues, settings);
            var part = DocumentPartFactory.Generate(fDocument.DocType, DocumentElement.table, sb.ToString());
            fDocument.Add(part);
            return this;
        }

        public DocumentBuilder WriteTable<T>(IEnumerable<string> headerValues, IEnumerable<T> rowValues, TableSettings settings)
        {
            var sb = new StringBuilder();
            fTableWriter.WriteTable(sb, headerValues, rowValues, settings);
            var part = DocumentPartFactory.Generate(fDocument.DocType, DocumentElement.table, sb.ToString());
            fDocument.Add(part);
            return this;
        }

        public void SetDocumentType(DocumentType docType)
        {
            fDocument.SetDocumentType(docType);
        }

        public Document GetDocument()
        {
            return fDocument;
        }

        public DocumentBuilder Clear()
        {
            fDocument.Clear();
            return this;
        }
    }
}
