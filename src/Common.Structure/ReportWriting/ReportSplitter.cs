using System;
using System.Text;

using Effanville.Common.Structure.ReportWriting.Document;

namespace Effanville.Common.Structure.ReportWriting
{
    /// <summary>
    /// Contains methods for splitting reports into their constituent parts.
    /// </summary>
    public static class ReportSplitter
    {
        /// <summary>
        /// Separates out a report string into constituent parts.
        /// </summary>
        public static Document.Document SplitReportString(DocumentType docType, string reportString)
        {
            if (string.IsNullOrEmpty(reportString))
            {
                return null;
            }

            if (docType == DocumentType.Csv || docType == DocumentType.Pdf || docType == DocumentType.Doc)
            {
                return null;
            }

            string remainingReportString = RemoveHeader(docType, reportString);
            Document.Document document = new Document.Document(docType);
            while (remainingReportString.Length > 0)
            {
                DocumentElement docElement = MatchDocumentStartElement(docType, remainingReportString, newElement: true);
                if (docElement != DocumentElement.None)
                {
                    DocumentPart nextPart = GetPart(docType, docElement, remainingReportString);
                    document.Add(nextPart);
                    remainingReportString = remainingReportString.Substring(Math.Min(nextPart.ConstituentString.Length, remainingReportString.Length));
                }
                else
                {
                    remainingReportString = remainingReportString.Substring(1);
                }
            }

            document.MergeTableParts();
            return document;
        }

        private static DocumentPart GetPart(DocumentType docType, DocumentElement docElement, string remainingReportString)
        {
            StringBuilder sb = new StringBuilder();
            int index = docType.StringForm(docElement).Length;
            _ = sb.Append(remainingReportString.AsSpan(0, index - 1));
            DocumentElement nextElement = DocumentElement.None;

            // continue until we reach the start of the next element type, or the end of the file.
            while (
                index <= remainingReportString.Length
                && (nextElement == DocumentElement.None))
            {
                _ = sb.Append(remainingReportString[index - 1]);
                nextElement = MatchDocumentStartElement(docType, remainingReportString.Substring(index), newElement: false);
                index++;
            }

            if (docType.StringForm(nextElement) == Constants.EnvNewLine)
            {
                _ = sb.AppendLine();
            }

            string elementString = sb.ToString();
            return DocumentPartFactory.Generate(docType, docElement, elementString);
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

        private static string RemoveHeader(DocumentType docType, string reportString)
        {
            switch (docType)
            {
                case DocumentType.Html:
                {
                    string endHeaderString = "</head>";
                    int endHeaderIndex = reportString.IndexOf(endHeaderString);
                    if (endHeaderIndex == -1)
                    {
                        return reportString;
                    }

                    return reportString.Substring(endHeaderIndex + endHeaderString.Length);
                }
                case DocumentType.Md:
                case DocumentType.Csv:
                case DocumentType.Doc:
                case DocumentType.Pdf:
                default:
                {
                    return reportString;
                }
            }
        }
    }
}
