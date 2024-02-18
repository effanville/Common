using System;

namespace Common.Structure.ReportWriting.Document
{
    /// <summary>
    /// A part of a document.
    /// </summary>
    public class DocumentPart : IEquatable<DocumentPart>
    {
        /// <summary>
        /// The type of the document.
        /// </summary>
        public DocumentType DocType
        {
            get;
        }

        /// <summary>
        /// The type of this part.
        /// </summary>
        public DocumentElement Element
        {
            get;
        }

        /// <summary>
        /// The string representing this part.
        /// </summary>
        public string ConstituentString
        {
            get;
        }

        /// <summary>
        /// Construct an instance.
        /// </summary>
        public DocumentPart(DocumentType docType, DocumentElement element, string constituentString)
        {
            DocType = docType;
            Element = element;
            ConstituentString = constituentString;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return ConstituentString;
        }

        /// <inheritdoc/>
        public bool Equals(DocumentPart other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(other, this))
            {
                return true;
            }

            return DocType.Equals(other.DocType)
                && Element.Equals(other.Element)
                && ConstituentString.Equals(other.ConstituentString, StringComparison.InvariantCulture);
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
        {
            return HashCode.Combine(DocType.GetHashCode(), Element.GetHashCode(), ConstituentString.GetHashCode());
        }
    }
}