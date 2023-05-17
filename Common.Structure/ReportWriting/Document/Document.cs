using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Structure.ReportWriting.Document
{
    /// <summary>
    /// A representation of a document that contains various parts.
    /// </summary>
    public sealed class Document : IEquatable<Document>
    {
        private readonly List<DocumentPart> fParts;

        /// <summary>
        /// The type of the document.
        /// </summary>
        public DocumentType DocType
        {
            get;
            private set;
        }

        /// <summary>
        /// The title of the document.
        /// </summary>
        public string Title
        {
            get;
            set;
        }

        /// <summary>
        /// The list of parts that make up this document.
        /// </summary>
        public IReadOnlyList<DocumentPart> Parts => fParts;

        /// <summary>
        /// Construct an instance.
        /// </summary>
        public Document(DocumentType docType)
        {
            DocType = docType;
            fParts = new List<DocumentPart>();
        }

        private Document(DocumentType docType, List<DocumentPart> parts)
        {
            DocType = docType;
            fParts = parts;
        }

        /// <summary>
        /// Set the type of the document.
        /// </summary>
        public void SetDocumentType(DocumentType docType)
        {
            DocType = docType;
        }

        /// <summary>
        /// An a part onto the end of the document.
        /// </summary>
        /// <param name="part"></param>
        public void Add(DocumentPart part)
        {
            fParts.Add(part);
        }

        /// <summary>
        /// Retrieve the last part of the document.
        /// </summary>
        public DocumentPart GetLastPart()
        {
            return Parts[Parts.Count - 1];
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

            for (int index = 1; index < fParts.Count; index++)
            {
                if (fParts[index - 1].Element == DocumentElement.table
                    && fParts[index].Element == DocumentElement.table)
                {
                    TableDocumentPart mergedTable = new TableDocumentPart(
                        DocType,
                        DocumentElement.table,
                        fParts[index - 1].ConstituentString + fParts[index].ConstituentString);
                    fParts[index - 1] = mergedTable;
                    fParts.RemoveAt(index);
                    index--;
                }
            }

            for (int index = 0; index < fParts.Count; index++)
            {
                if (fParts[index].Element == DocumentElement.table)
                {
                    string tableString = fParts[index].ConstituentString;
                    string[] lines = tableString.Split(Constants.EnvNewLine).Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();
                    bool[] headerValues = lines.Select(l => IsMdHeaderSpecifier(l)).ToArray();
                    IEnumerable<bool> trues = headerValues.Where(val => val == true);

                    // table is actually more than one table. must now split.
                    if (trues.Count() > 1)
                    {
                        fParts.RemoveAt(index);
                        List<List<string>> tempParts = new List<List<string>>();
                        List<string> tempList = new List<string> { lines[0], lines[1] };
                        for (int lineIndex = 2; lineIndex < lines.Length; lineIndex++)
                        {
                            if (lineIndex + 1 >= lines.Length)
                            {
                                tempList.Add(lines[lineIndex]);
                                tempParts.Add(tempList);
                                tempList = new List<string>();
                            }
                            else if (headerValues[lineIndex + 1])
                            {
                                tempParts.Add(tempList);
                                tempList = new List<string> { lines[lineIndex], lines[lineIndex + 1] };
                                lineIndex++;
                            }
                            else if (!headerValues[lineIndex])
                            {
                                tempList.Add(lines[lineIndex]);
                            }

                        }

                        int indexValue = index;
                        foreach (List<string> tempTable in tempParts)
                        {
                            StringBuilder sb = new StringBuilder();
                            foreach (string string1 in tempTable)
                            {
                                _ = sb.AppendLine(string1);
                            }

                            TableDocumentPart mergedTable = new TableDocumentPart(
                                DocType,
                                DocumentElement.table,
                                sb.ToString());
                            fParts.Insert(indexValue, mergedTable);
                            indexValue++;
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

        /// <summary>
        /// Find the index of the first part matching the predicate.
        /// </summary>
        public int FindIndex(Predicate<DocumentPart> match)
        {
            return fParts.FindIndex(match);
        }

        /// <summary>
        /// Find the index of the first part matching the predicate.
        /// </summary>
        public int FindIndex(int index, Predicate<DocumentPart> match)
        {
            return fParts.FindIndex(index, match);
        }

        /// <summary>
        /// Returns the first part of the document that is a table.
        /// </summary>
        public TableDocumentPart FirstTablePart()
        {
            return fParts.First(part => part.Element == DocumentElement.table) as TableDocumentPart;
        }

        /// <summary>
        /// Returns the first part of the document that is a text element.
        /// </summary>
        public TextDocumentPart FirstTextPart(DocumentElement element)
        {
            return fParts.First(part => part.Element == element) as TextDocumentPart;
        }

        /// <summary>
        /// Retrieve the parts of the document after the index satisfying the conditions.
        /// </summary>
        public Document GetSubDocumentFrom(int index, Predicate<DocumentPart> predicate)
        {
            return GetSubDocument(FindIndex(index, predicate));
        }

        /// <summary>
        /// Get the parts of the document after the value expected.
        /// </summary>
        public Document GetSubDocumentFrom(Predicate<DocumentPart> predicate)
        {
            return GetSubDocument(FindIndex(predicate));
        }

        /// <summary>
        /// Return all elements in the document under the value specified at the index.
        /// </summary>
        public Document GetSubDocument(int startIndex)
        {
            if (startIndex < 0 || startIndex >= fParts.Count)
            {
                return null;
            }

            List<DocumentPart> parts = new List<DocumentPart>
                {
                    fParts[startIndex]
                };
            DocumentElement startIndexElementType = fParts[startIndex].Element;
            int index = startIndex + 1;
            while (fParts[index].Element != startIndexElementType)
            {
                parts.Add(fParts[index]);
                index++;
            }

            return new Document(DocType, parts);
        }

        /// <summary>
        /// Returns all the string representations of the parts.
        /// </summary>
        public string SerializeToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (DocumentPart part in Parts)
            {
                _ = sb.Append(part.ToString());
            }

            return sb.ToString();
        }

        /// <summary>
        /// Returns a string of the document including any header and footer.
        /// </summary>
        public string DocumentString()
        {
            ReportBuilder rb = new ReportBuilder(DocType, ReportSettings.Default());
            _ = rb.WriteHeader(Title);
            foreach (DocumentPart part in Parts)
            {
                _ = rb.Append(part.ToString());
            }

            _ = rb.WriteFooter();
            return rb.ToString();
        }

        /// <inheritdoc/>
        public bool Equals(Document other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(other, this))
            {
                return true;
            }

            return Enumerable.SequenceEqual(Parts, other.Parts);
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

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals(obj as Document);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return Parts.GetHashCode();
        }

        /// <summary>
        /// Removes all parts in the document.
        /// </summary>
        public void Clear()
        {
            fParts.Clear();
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            string manyParts = Parts.Count > 4 ? "..." : "";
            return $"Document. Title:{Title}, Parts:{string.Join(',', Parts.Take(4))}{manyParts}";
        }
    }
}