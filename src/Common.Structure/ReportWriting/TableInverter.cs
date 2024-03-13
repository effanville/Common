using System.Collections.Generic;
using System.Linq;

using Effanville.Common.Structure.ReportWriting.Document;

using HtmlAgilityPack;

namespace Effanville.Common.Structure.ReportWriting
{
    /// <summary>
    /// Class to invert a Table.
    /// </summary>
    public static class TableInverter
    {
        /// <summary>
        /// Invert the table.
        /// </summary>
        public static TableDocumentPart InvertTable(DocumentType reportType, string tableString)
        {
            switch (reportType)
            {
                case DocumentType.Md:
                    return InvertTableMd(tableString);
                case DocumentType.Html:
                    return InvertTableHtml(tableString);
                case DocumentType.Csv:
                case DocumentType.Doc:
                case DocumentType.Pdf:
                default:
                    return null;
            }
        }

        private static TableDocumentPart InvertTableMd(string tableString)
        {
            string[] tableRows = tableString.Split('\n');
            string headerString = tableRows[0];
            List<string> headerFields = ConvertRowFromString(headerString);

            string[] rowStrings = tableRows.Skip(2).Where(match => match != "").ToArray();
            List<List<string>> rowFields = rowStrings.Select(row => ConvertRowFromString(row)).ToList();
            return new TableDocumentPart(DocumentType.Md, headerFields, rowFields, tableString);

            List<string> ConvertRowFromString(string row)
            {
                return row
                    .Split('|')
                    .ToList()
                    .Where(match => match != "" && match != "\r")
                    .Select(field => field.Trim())
                    .ToList();
            }
        }

        private static TableDocumentPart InvertTableHtml(string tableString)
        {
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(tableString);
            var table = htmlDocument.DocumentNode.ChildNodes.First(node => node.Name == "table");
            var tableHeader = table.ChildNodes.First(node => node.Name == "thead");
            var tableHeaderRow = tableHeader.ChildNodes.First(node => node.Name == "tr");
            List<string> tableHeaderFields = ConvertRow(tableHeaderRow, new[] { "th" });
            var tableRows = table.ChildNodes
                .First(node => node.Name == "tbody")
                .ChildNodes
                .Where(node => node.Name == "tr")
                .ToList();
            List<List<string>> tableRowFields = tableRows
                .Select(node => ConvertRow(node, new[] { "td", "th" }))
                .ToList();
            return new TableDocumentPart(DocumentType.Html, tableHeaderFields, tableRowFields, tableString);

            List<string> ConvertRow(HtmlNode rowNode, string[] rowTag)
            {
                return rowNode.ChildNodes
                   .Where(node => rowTag.Contains(node.Name))
                   .Select(node => node.InnerHtml.Trim())
                   .ToList();
            }
        }
    }
}
