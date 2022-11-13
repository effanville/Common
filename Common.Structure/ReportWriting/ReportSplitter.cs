using System;
using System.Text;

using Common.Structure.ReportWriting.Document;

namespace Common.Structure.ReportWriting
{
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
            var document = new Document.Document(docType);
            while (remainingReportString.Length > 0)
            {
                var docElement = MatchDocumentStartElement(docType, remainingReportString, newElement: true);
                if (docElement != DocumentElement.None)
                {
                    var nextPart = GetPart(docType, docElement, remainingReportString);
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
            int index = DocumentElementString(docType, docElement).Length;
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

            if (DocumentElementString(docType, nextElement) == "\r\n")
            {
                _ = sb.AppendLine();
            }

            string elementString = sb.ToString();
            return DocumentPartFactory.Generate(docType, docElement, elementString);
        }

        private static DocumentElement MatchDocumentStartElement(DocumentType docType, string reportString, bool newElement = false)
        {
            if (reportString.StartsWith(DocumentElementString(docType, DocumentElement.table)))
            {
                return DocumentElement.table;
            }
            else if (reportString.StartsWith(DocumentElementString(docType, DocumentElement.p)))
            {
                return DocumentElement.p;
            }
            else if (reportString.StartsWith(DocumentElementString(docType, DocumentElement.h6)))
            {
                return DocumentElement.h6;
            }
            else if (reportString.StartsWith(DocumentElementString(docType, DocumentElement.h5)))
            {
                return DocumentElement.h5;
            }
            else if (reportString.StartsWith(DocumentElementString(docType, DocumentElement.h4)))
            {
                return DocumentElement.h4;
            }
            else if (reportString.StartsWith(DocumentElementString(docType, DocumentElement.h3)))
            {
                return DocumentElement.h3;
            }
            else if (reportString.StartsWith(DocumentElementString(docType, DocumentElement.h2)))
            {
                return DocumentElement.h2;
            }
            else if (reportString.StartsWith(DocumentElementString(docType, DocumentElement.h1)))
            {
                return DocumentElement.h1;
            }
            else if (reportString.StartsWith(DocumentElementString(docType, DocumentElement.chart)))
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

        public static string StringForm(this DocumentType docType, DocumentElement docElement)
        {
            return DocumentElementString(docType, docElement);
        }

        private static string DocumentElementString(DocumentType docType, DocumentElement docElement)
        {
            switch (docType)
            {
                case DocumentType.Html:
                {
                    if (docElement == DocumentElement.chart)
                    {
                        return "<div>";
                    }
                    return $"<{docElement.ToString()}>";
                }
                case DocumentType.Md:
                {
                    switch (docElement)
                    {
                        case DocumentElement.h1:
                        {
                            return "#";
                        }
                        case DocumentElement.h2:
                        {
                            return "##";
                        }
                        case DocumentElement.h3:
                        case DocumentElement.h4:
                        case DocumentElement.h5:
                        case DocumentElement.h6:
                        {
                            return "###";
                        }
                        case DocumentElement.table:
                        {
                            return "|";
                        }
                        case DocumentElement.chart:
                        {
                            return "ChartChart";
                        }
                        case DocumentElement.None:
                        case DocumentElement.p:
                        default:
                        {
                            return "\r\n";
                        }
                    }
                }
                case DocumentType.Doc:
                case DocumentType.Pdf:
                case DocumentType.Csv:
                default:
                    return null;
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
