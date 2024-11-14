namespace Effanville.Common.ReportWriting.Documents;

/// <summary>
/// A representation of a document that contains various parts.
/// </summary>
public sealed class Document : IEquatable<Document>
{
    private readonly List<DocumentPart> _parts = new List<DocumentPart>();

    /// <summary>
    /// The type of the document.
    /// </summary>
    public DocumentType DocType { get; private set; }

    /// <summary>
    /// The title of the document.
    /// </summary>
    public string Title { get; set; }

    public bool IncludesHeader { get; set; }

    /// <summary>
    /// The list of parts that make up this document.
    /// </summary>
    public IReadOnlyList<DocumentPart> Parts => _parts;

    /// <summary>
    /// Construct an instance.
    /// </summary>
    public Document(DocumentType docType, string title)
    {
        DocType = docType;
        Title = title;
    }

    /// <summary>
    /// An a part onto the end of the document.
    /// </summary>
    /// <param name="part"></param>
    public void Add(DocumentPart part) => _parts.Add(part);


    private static bool IsRowHeaderSpecifier(IReadOnlyList<string> lines)
    {
        bool[] headerValues = lines.Select(l => IsMdHeaderSpecifier(l)).ToArray();
        return headerValues.All(x => x);
    }

    /// <summary>
    /// Merge separate table parts into one single part.
    /// </summary>
    public void MergeTableParts()
    {
        if (DocType != DocumentType.Md)
        {
            return;
        }

        int thisTablePartIndex = 0;
        bool tableContainsRowHeader = false;
        for (int index = 0; index < _parts.Count; index++)
        {
            if (_parts[index].Element != DocumentElement.table)
            {
                thisTablePartIndex = 0;
                continue;
            }

            if (thisTablePartIndex == 0)
            {
                thisTablePartIndex++;
                continue;
            }

            bool merge = true;
            var previousTablePart = (_parts[index - 1] as TableDocumentPart);
            TableDocumentPart? thisTablePart = (_parts[index] as TableDocumentPart);
            List<IReadOnlyList<string>> newRows = previousTablePart.TableRows.ToList();
            var headers = previousTablePart.TableHeaders;
            if (!IsRowHeaderSpecifier(thisTablePart.TableHeaders))
            {
                newRows.Add(thisTablePart.TableHeaders);
            }
            else
            {
                if (tableContainsRowHeader)
                {
                    merge = false;
                    var previousUpdatedRows = previousTablePart.TableRows.Take(previousTablePart.TableRows.Count - 1).ToList();
                    _parts[index - 1] = new TableDocumentPart(
                        DocType,
                        previousTablePart.TableHeaders,
                        previousUpdatedRows,
                        previousTablePart.FirstColumnAsHeader);
                    headers = previousTablePart.TableRows[previousTablePart.TableRows.Count - 1];
                    newRows = new List<IReadOnlyList<string>>();
                }
                tableContainsRowHeader = true;
            }

            TableDocumentPart mergedTable = new TableDocumentPart(
                DocType,
                headers,
                newRows,
                previousTablePart.FirstColumnAsHeader);

            if (merge)
            {
                _parts[index - 1] = mergedTable;
                _parts.RemoveAt(index);
                index--;
            }
            else
            {
                _parts[index] = mergedTable;
            }

            thisTablePartIndex++;
        }

        for (int partIndex = 0; partIndex < _parts.Count; partIndex++)
        {
            if (_parts[partIndex].Element == DocumentElement.table && _parts[partIndex] is TableDocumentPart tablePart)
            {
                bool firstRowHeaders = true;
                foreach (List<string> row in tablePart.TableRows)
                {
                    if (row == null || !row.Any())
                    {
                        continue;
                    }

                    if (!row[0].StartsWith(Constants.MarkdownBoldSpecifier) || !row[0].EndsWith(Constants.MarkdownBoldSpecifier))
                    {
                        firstRowHeaders = false;
                        break;
                    }
                }

                tablePart.FirstColumnAsHeader = firstRowHeaders;
                if (firstRowHeaders)
                {
                    foreach (List<string> row in tablePart.TableRows)
                    {
                        if (row == null || !row.Any())
                        {
                            continue;
                        }

                        int index = row[0].IndexOf(Constants.MarkdownBoldSpecifier);
                        string cleanPath = (index < 0)
                            ? row[0]
                            : row[0].Remove(index, Constants.MarkdownBoldSpecifier.Length);

                        int lastIndex = cleanPath.LastIndexOf(Constants.MarkdownBoldSpecifier);
                        row[0] = (lastIndex < 0)
                            ? cleanPath
                            : cleanPath.Remove(lastIndex, Constants.MarkdownBoldSpecifier.Length);
                    }
                }
            }
        }
    }

    private static bool IsMdHeaderSpecifier(string str)
    {
        foreach (char c in str)
        {
            if (c != ' ' && c != '-' && c != '|')
            {
                return false;
            }
        }

        return true;
    }

    /// <inheritdoc/>
    public bool Equals(Document? other)
    {
        if (other == null)
        {
            return false;
        }

        if (ReferenceEquals(other, this))
        {
            return true;
        }

        return Parts.SequenceEqual(other.Parts);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj == null)
        {
            return false;
        }

        if (ReferenceEquals(obj, this))
        {
            return true;
        }

        if (obj.GetType() != GetType())
        {
            return false;
        }

        return Equals(obj as Document);
    }

    /// <inheritdoc/>
    public override int GetHashCode() => Parts.GetHashCode();

    /// <inheritdoc/>
    public override string ToString()
    {
        string manyParts = Parts.Count > 4 ? "..." : "";
        return $"Document. Title:{Title}, Parts:{string.Join(',', Parts.Take(4))}{manyParts}";
    }

    /// <summary>
    /// Removes all parts in the document.
    /// </summary>
    public void Clear() => _parts.Clear();
}