using Effanville.Common.ReportWriting.Documents;

using HtmlAgilityPack;

namespace Effanville.Common.ReportWriting
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

            bool firstRowHeaders = true;
            foreach (List<string> row in rowFields)
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

            if (firstRowHeaders)
            {
                foreach (List<string> row in rowFields)
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

            return new TableDocumentPart(DocumentType.Md, headerFields, rowFields, firstRowHeaders);

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
            return new TableDocumentPart(DocumentType.Html, tableHeaderFields, tableRowFields, true);

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
