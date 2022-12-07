using System.Collections.Generic;

namespace Common.Structure.ReportWriting.Document
{
    /// <summary>
    /// A part of a document detailing a table.
    /// </summary>
    public sealed class TableDocumentPart : DocumentPart
    {
        /// <summary>
        /// The headers of the table.
        /// </summary>
        public List<string> TableHeaders
        {
            get;
        }

        /// <summary>
        /// The rows of the table.
        /// </summary>
        public List<List<string>> TableRows
        {
            get;
        }

        /// <summary>
        /// Construct an instance of a <see cref="TableDocumentPart"/>
        /// </summary>
        public TableDocumentPart(DocumentType docType, DocumentElement element, string constituentString)
            : base(docType, element, constituentString)
        {
            var inverted = TableInverter.InvertTable(DocType, constituentString);
            TableHeaders = inverted.TableHeaders;
            TableRows = inverted.TableRows;
        }

        /// <summary>
        /// Construct an instance of a <see cref="TableDocumentPart"/>
        /// </summary>
        public TableDocumentPart(DocumentType docType, List<string> tableHeaders, List<List<string>> tableRows, string constituentString)
            : base(docType, DocumentElement.table, constituentString)
        {
            TableHeaders = tableHeaders;
            TableRows = tableRows;
        }
    }
}