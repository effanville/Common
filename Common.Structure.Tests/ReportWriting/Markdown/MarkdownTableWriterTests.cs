using System.Collections.Generic;
using System.Text;

using Common.Structure.ReportWriting;
using Common.Structure.ReportWriting.Markdown;

using NUnit.Framework;

namespace Common.Structure.Tests.ReportWriting.Markdown
{
    [TestFixture]
    public sealed class MarkdownTableWriterTests
    {
        private static IEnumerable<TestCaseData> TableHeaderData()
        {
            yield return new TestCaseData(new List<string>() { "Row1", "Info", "More Stuff" }, "| Row1 | Info | More Stuff |\r\n| - | - | - |\r\n");
        }

        [TestCaseSource(nameof(TableHeaderData))]
        public void CanWriteTableHeader(List<string> headerValues, string expectedMarkdown)
        {
            TableSettings settings = TableSettings.Default();
            StringBuilder sb = new StringBuilder();
            var tableWriter = TableWriterFactory.Create(DocumentType.Md);
            _ = tableWriter.WriteTableHeader(sb, headerValues, settings);

            string markdownSnippet = sb.ToString();
            Assert.AreEqual(expectedMarkdown, markdownSnippet);
        }

        private static IEnumerable<TestCaseData> TableRowData()
        {
            yield return new TestCaseData(new List<string>() { "Row1", "Info", "More Stuff" }, "| Row1 | Info | More Stuff |\r\n");
        }

        [TestCaseSource(nameof(TableRowData))]
        public void CanWriteTableRow(List<string> headerValues, string expectedMarkdown)
        {
            TableSettings settings = TableSettings.Default();
            StringBuilder sb = new StringBuilder();
            var tableWriter = TableWriterFactory.Create(DocumentType.Md);
            tableWriter.WriteTableRow(sb, headerValues, settings);

            string markdownSnippet = sb.ToString();
            Assert.AreEqual(expectedMarkdown, markdownSnippet);
        }

        private static IEnumerable<TestCaseData> TableData()
        {
            yield return new TestCaseData(
                new List<string>() { "Row1", "Info", "More Stuff" },
                new List<List<string>> { new List<string>() { "Row1", "Info", "More Stuff" } },
                false,
                "| Row1 | Info | More Stuff |\r\n| ---- | ---- | ---------- |\r\n| Row1 | Info | More Stuff |\r\n");
            yield return new TestCaseData(
                 new List<string>() { "", "" },
                 new List<List<string>> { new List<string>() { "Byes", "4" }, new List<string>() { "Leg Byes", "3" } },
                 true,
                "|              |   |\r\n| ------------ | - |\r\n| __Byes__     | 4 |\r\n| __Leg Byes__ | 3 |\r\n");

        }

        [TestCaseSource(nameof(TableData))]
        public void CanWriteTable(List<string> headerValues, List<List<string>> rowValues, bool header, string expectedMarkdown)
        {
            TableSettings settings = new TableSettings()
            {
                FirstColumnAsHeader = header
            };
            StringBuilder sb = new StringBuilder();
            var tableWriter = TableWriterFactory.Create(DocumentType.Md);
            tableWriter.WriteTable(sb, headerValues, rowValues, settings);

            string markdownSnippet = sb.ToString();
            Assert.AreEqual(expectedMarkdown, markdownSnippet);
        }

        [TestCaseSource(nameof(TableData))]
        public void CanWriteTableWriting(List<string> headerValues, List<List<string>> rowValues, bool header, string expectedMarkdown)
        {
            TableSettings settings = new TableSettings()
            {
                FirstColumnAsHeader = header
            };
            StringBuilder sb = new StringBuilder();
            var tableWriter = TableWriterFactory.Create(DocumentType.Md);
            tableWriter.WriteTable(sb, headerValues, rowValues, settings);

            string markdownSnippet = sb.ToString();
            Assert.AreEqual(expectedMarkdown, markdownSnippet);
        }
    }
}
