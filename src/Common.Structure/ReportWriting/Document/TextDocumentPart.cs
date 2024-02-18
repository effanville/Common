namespace Effanville.Common.Structure.ReportWriting.Document
{
    /// <summary>
    /// A part of a document that contains text only.
    /// </summary>
    public sealed class TextDocumentPart : DocumentPart
    {
        /// <summary>
        /// The text for this part of the document.
        /// </summary>
        public string Text
        {
            get;
            set;
        }

        /// <summary>
        /// Construct an instance.
        /// </summary>
        public TextDocumentPart(DocumentType docType, DocumentElement element, string constituentString)
            : base(docType, element, constituentString)
        {
            Text = GetTextFromTotalString(docType, element, constituentString);
        }

        private static string GetTextFromTotalString(DocumentType docType, DocumentElement element, string constituentString)
        {
            switch (docType)
            {
                case DocumentType.Html:
                {
                    string stringForm = docType.StringForm(element);
                    int startIndex = constituentString.IndexOf(stringForm);
                    int endIndex = constituentString.IndexOf($"</{element}>");
                    return constituentString.Substring(startIndex + stringForm.Length, endIndex - startIndex - stringForm.Length);
                }
                case DocumentType.Md:
                {
                    if (element.IsHeader())
                    {
                        string elementString = docType.StringForm(element);
                        return constituentString.Substring(elementString.Length).Trim();
                    }

                    return constituentString;
                }
                default:
                {
                    return null;
                }
            }
        }
    }
}