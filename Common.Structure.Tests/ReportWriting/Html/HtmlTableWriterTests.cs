using System.Collections.Generic;
using System.Text;

using Common.Structure.ReportWriting;
using Common.Structure.ReportWriting.Markdown;

using NUnit.Framework;

namespace Common.Structure.Tests.ReportWriting.Html
{
    [TestFixture]
    public sealed class HtmlTableWriterTests
    {
        private static IEnumerable<TestCaseData> TableHeaderData()
        {
            yield return new TestCaseData(new List<string>() { "Row1", "Info", "More Stuff" }, "<thead><tr>\r\n<th scope=\"col\">Row1</th><th>Info</th><th>More Stuff</th>\r\n</tr></thead>\r\n<tbody>\r\n");
        }

        [TestCaseSource(nameof(TableHeaderData))]
        public void CanWriteTableHeader(List<string> headerValues, string expectedMarkdown)
        {
            TableSettings settings = TableSettings.Default();
            StringBuilder sb = new StringBuilder();
            var tableWriter = TableWriterFactory.Create(DocumentType.Html);
            _ = tableWriter.WriteTableHeader(sb, headerValues, settings);

            string markdownSnippet = sb.ToString();
            Assert.AreEqual(expectedMarkdown, markdownSnippet);
        }

        private static IEnumerable<TestCaseData> TableRowData()
        {
            yield return new TestCaseData(new List<string>() { "Row1", "Info", "More Stuff" }, "<tr>\r\n<td>Row1</td><td>Info</td><td>More Stuff</td>\r\n</tr>\r\n");
        }

        [TestCaseSource(nameof(TableRowData))]
        public void CanWriteTableRow(List<string> headerValues, string expectedMarkdown)
        {
            TableSettings settings = TableSettings.Default();
            StringBuilder sb = new StringBuilder();
            var tableWriter = TableWriterFactory.Create(DocumentType.Html);
            tableWriter.WriteTableRow(sb, headerValues, settings);

            string markdownSnippet = sb.ToString();
            Assert.AreEqual(expectedMarkdown, markdownSnippet);
        }

        private static IEnumerable<TestCaseData> TableData()
        {
            yield return new TestCaseData(
                new List<string>() { "Row1", "Info", "More Stuff" },
                new List<List<string>> { new List<string>() { "Row1", "Info", "More Stuff" } },
                "<table>\r\n<thead><tr>\r\n<th scope=\"col\">Row1</th><th>Info</th><th>More Stuff</th>\r\n</tr></thead>\r\n<tbody>\r\n<tr>\r\n<td>Row1</td><td>Info</td><td>More Stuff</td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n");
        }

        [TestCaseSource(nameof(TableData))]
        public void CanWriteTable(List<string> headerValues, List<List<string>> rowValues, string expectedMarkdown)
        {
            TableSettings settings = TableSettings.Default();
            StringBuilder sb = new StringBuilder();
            var tableWriter = TableWriterFactory.Create(DocumentType.Html);
            tableWriter.WriteTable(sb, headerValues, rowValues, settings);

            string markdownSnippet = sb.ToString();
            Assert.AreEqual(expectedMarkdown, markdownSnippet);
        }
    }
}
