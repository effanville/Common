namespace Effanville.Common.ReportWriting.Documents;

/// <summary>
/// A part of a document detailing a table.
/// </summary>
public sealed class TableDocumentPart : DocumentPart
{
    public bool FirstColumnAsHeader { get; set; } = true;

    /// <summary>
    /// The headers of the table.
    /// </summary>
    public IReadOnlyList<string> TableHeaders { get; }

    /// <summary>
    /// The rows of the table.
    /// </summary>
    public IReadOnlyList<IReadOnlyList<string>> TableRows { get; }

    /// <summary>
    /// Construct an instance of a <see cref="TableDocumentPart"/>
    /// </summary>
    public TableDocumentPart(
        DocumentType docType,
        IReadOnlyList<string> headers,
        IReadOnlyList<IReadOnlyList<string>> rows,
        bool headerFirstColumn)
        : base(docType, DocumentElement.table, "")
    {
        TableHeaders = headers;
        TableRows = rows;
        FirstColumnAsHeader = headerFirstColumn;
    }

    /// <summary>
    /// Construct an instance of a <see cref="TableDocumentPart"/>
    /// </summary>
    public TableDocumentPart(DocumentType docType, DocumentElement element, string constituentString)
        : base(docType, element, constituentString)
    {
        TableDocumentPart inverted = TableInverter.InvertTable(DocType, constituentString);
        TableHeaders = inverted.TableHeaders;
        TableRows = inverted.TableRows;
    }

    /// <inheritdoc/>
    public override bool Equals(DocumentPart other)
    {
        if (other == null)
        {
            return false;
        }

        if (ReferenceEquals(other, this))
        {
            return true;
        }

        if (other is TableDocumentPart otherTablePart)
        {
            if (TableRows.Count != otherTablePart.TableRows.Count)
            {
                return false;
            }
            for (int rowIndex = 0; rowIndex < TableRows.Count; rowIndex++)
            {
                if (!TableRows[rowIndex].SequenceEqual(otherTablePart.TableRows[rowIndex]))
                {
                    return false;
                }
            }

            bool headersEqual = TableHeaders.SequenceEqual(otherTablePart.TableHeaders);
            return base.Equals(other)
                && headersEqual;

        }

        return false;
    }

    /// <inheritdoc/>
    public override bool Equals(object obj)
    {
        if (obj == null)
        {
            return false;
        }

        if (ReferenceEquals(obj, this))
        {
            return true;
        }

        return Equals(obj as DocumentPart);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
        => HashCode.Combine(base.GetHashCode(), TableRows.GetHashCode(), TableHeaders.GetHashCode());

}