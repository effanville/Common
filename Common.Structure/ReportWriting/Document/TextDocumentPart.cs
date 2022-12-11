namespace Common.Structure.ReportWriting.Document
{
    public sealed class TextDocumentPart : DocumentPart
    {
        public string Text
        {
            get;
            set;
        }

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