using System.Collections.Generic;
using System.Text;

using Effanville.Common.Structure.ReportWriting;

using NUnit.Framework;

namespace Effanville.Common.Structure.Tests.ReportWriting.Html
{
    [TestFixture]
    public sealed class HtmlTableWriterTests
    {
        private static string enl = TestConstants.EnvNewLine;
        
        private static IEnumerable<TestCaseData> TableHeaderData()
        {
            yield return new TestCaseData(
                new List<string>() { "Row1", "Info", "More Stuff" }, 
                $"<thead><tr>{enl}<th scope=\"col\">Row1</th><th>Info</th><th>More Stuff</th>{enl}</tr></thead>{enl}<tbody>{enl}");
        }

        [TestCaseSource(nameof(TableHeaderData))]
        public void CanWriteTableHeader(List<string> headerValues, string expectedMarkdown)
        {
            StringBuilder sb = new StringBuilder();
            var tableWriter = TableWriterFactory.Create(DocumentType.Html);
            _ = tableWriter.WriteTableHeader(sb, headerValues);

            string markdownSnippet = sb.ToString();
            Assert.AreEqual(expectedMarkdown, markdownSnippet);
        }

        private static IEnumerable<TestCaseData> TableRowData()
        {
            yield return new TestCaseData(
                new List<string>() { "Row1", "Info", "More Stuff" }, 
                $"<tr>{enl}<td>Row1</td><td>Info</td><td>More Stuff</td>{enl}</tr>{enl}");
        }

        [TestCaseSource(nameof(TableRowData))]
        public void CanWriteTableRow(List<string> headerValues, string expectedMarkdown)
        {
            StringBuilder sb = new StringBuilder();
            var tableWriter = TableWriterFactory.Create(DocumentType.Html);
            tableWriter.WriteTableRow(sb, headerValues, headerFirstColumn: false);

            string markdownSnippet = sb.ToString();
            Assert.AreEqual(expectedMarkdown, markdownSnippet);
        }

        private static IEnumerable<TestCaseData> TableData()
        {
            yield return new TestCaseData(
                new List<string>() { "Row1", "Info", "More Stuff" },
                new List<List<string>> { new List<string>() { "Row1", "Info", "More Stuff" } },
                $"<table>{enl}<thead><tr>{enl}<th scope=\"col\">Row1</th><th>Info</th><th>More Stuff</th>{enl}</tr></thead>{enl}<tbody>{enl}<tr>{enl}<td>Row1</td><td>Info</td><td>More Stuff</td>{enl}</tr>{enl}</tbody>{enl}</table>{enl}");
        }

        [TestCaseSource(nameof(TableData))]
        public void CanWriteTable(List<string> headerValues, List<List<string>> rowValues, string expectedMarkdown)
        {
            StringBuilder sb = new StringBuilder();
            var tableWriter = TableWriterFactory.Create(DocumentType.Html);
            tableWriter.WriteTable(sb, headerValues, rowValues, headerFirstColumn: false);

            string markdownSnippet = sb.ToString();
            Assert.AreEqual(expectedMarkdown, markdownSnippet);
        }
    }
}
