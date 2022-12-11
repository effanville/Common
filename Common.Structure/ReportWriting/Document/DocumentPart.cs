using System;

namespace Common.Structure.ReportWriting.Document
{
    public class DocumentPart : IEquatable<DocumentPart>
    {
        public DocumentType DocType
        {
            get;
        }

        public DocumentElement Element
        {
            get;
        }

        public string ConstituentString
        {
            get;
        }

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