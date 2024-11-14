using System.Text;

using Effanville.Common.ReportWriting.Documents;

namespace Effanville.Common.ReportWriting
{
    /// <summary>
    /// Contains methods for splitting reports into their constituent parts.
    /// </summary>
    public static class ReportSplitter
    {
        /// <summary>
        /// Separates out a report string into constituent parts.
        /// </summary>
        public static Document? SplitReportString(DocumentType docType, string reportString)
        {
            if (reportString == null)
            {
                return null;
            }

            if (docType == DocumentType.Csv || docType == DocumentType.Pdf || docType == DocumentType.Doc)
            {
                return null;
            }

            Document document = new Document(docType, "");
            if (RemoveHeader(docType, reportString, out string remainingReportString))
            {
                document.IncludesHeader = true;
            }

            while (remainingReportString.Length > 0)
            {
                DocumentElement docElement = MatchDocumentStartElement(docType, remainingReportString, newElement: true);
                if (docElement != DocumentElement.None)
                {
                    DocumentPart nextPart = GetPart(docType, docElement, remainingReportString, out int endIndex);
                    document.Add(nextPart);
                    remainingReportString = remainingReportString.Substring(Math.Min(endIndex, remainingReportString.Length));
                }
                else
                {
                    remainingReportString = remainingReportString.Substring(1);
                }
            }

            document.MergeTableParts();
            return document;
        }

        private static DocumentPart GetPart(DocumentType docType, DocumentElement docElement, string remainingReportString, out int endIndex)
        {
            endIndex = 0;
            StringBuilder sb = new StringBuilder();
            int index = docType.StringForm(docElement).Length;
            _ = sb.Append(remainingReportString.AsSpan(0, index - 1));
            DocumentElement nextElement = DocumentElement.None;

            // continue until we reach the start of the next element type, or the end of the file.
            while (
                index <= remainingReportString.Length
                && nextElement == DocumentElement.None)
            {
                _ = sb.Append(remainingReportString[index - 1]);
                nextElement = MatchDocumentStartElement(docType, remainingReportString.Substring(index), newElement: false);
                if (docType == DocumentType.Md && (docElement == DocumentElement.table && nextElement == DocumentElement.table))
                {
                    nextElement = DocumentElement.None;
                }
                endIndex = index;
                index++;
            }

            if (docType.StringForm(nextElement) == Constants.EnvNewLine)
            {
                endIndex += Constants.EnvNewLine.Length;
            }

            string elementString = sb.ToString();
            return new DocumentPartFactory().Generate(docType, docElement, elementString);
        }

        private static DocumentElement MatchDocumentStartElement(DocumentType docType, string reportString, bool newElement = false)
        {
            if (reportString.StartsWith(docType.StringForm(DocumentElement.table)))
            {
                return DocumentElement.table;
            }
            else if (reportString.StartsWith(docType.StringForm(DocumentElement.p)))
            {
                return DocumentElement.p;
            }
            else if (reportString.StartsWith(docType.StringForm(DocumentElement.h6)))
            {
                return DocumentElement.h6;
            }
            else if (reportString.StartsWith(docType.StringForm(DocumentElement.h5)))
            {
                return DocumentElement.h5;
            }
            else if (reportString.StartsWith(docType.StringForm(DocumentElement.h4)))
            {
                return DocumentElement.h4;
            }
            else if (reportString.StartsWith(docType.StringForm(DocumentElement.h3)))
            {
                return DocumentElement.h3;
            }
            else if (reportString.StartsWith(docType.StringForm(DocumentElement.h2)))
            {
                return DocumentElement.h2;
            }
            else if (reportString.StartsWith(docType.StringForm(DocumentElement.h1)))
            {
                return DocumentElement.h1;
            }
            else if (reportString.StartsWith(docType.StringForm(DocumentElement.chart)))
            {
                return DocumentElement.chart;
            }
            else
            {
                if (newElement && docType == DocumentType.Md)
                {
                    return DocumentElement.p;
                }

                return DocumentElement.None;
            }
        }

        private static bool RemoveHeader(DocumentType docType, string reportString, out string removedString)
        {
            switch (docType)
            {
                case DocumentType.Html:
                {
                    string endHeaderString = "</head>";
                    int endHeaderIndex = reportString.IndexOf(endHeaderString);
                    if (endHeaderIndex == -1)
                    {
                        removedString = reportString;
                        return false;
                    }

                    removedString = reportString.Substring(endHeaderIndex + endHeaderString.Length);
                    return true;
                }
                case DocumentType.Md:
                case DocumentType.Csv:
                case DocumentType.Doc:
                case DocumentType.Pdf:
                default:
                {
                    removedString = reportString;
                    return false;
                }
            }
        }
    }
}
