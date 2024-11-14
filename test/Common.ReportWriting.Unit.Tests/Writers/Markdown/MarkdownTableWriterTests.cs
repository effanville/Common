using System.Collections.Generic;
using System.Text;

using Effanville.Common.ReportWriting.Documents;
using Effanville.Common.ReportWriting.Writers;

using NUnit.Framework;

namespace Effanville.Common.ReportWriting.Unit.Tests.Writers.Markdown
{
    [TestFixture]
    public sealed class MarkdownTableWriterTests
    {
        private static string enl = TestConstants.EnvNewLine;

        private static IEnumerable<TestCaseData> TableHeaderData()
        {
            yield return new TestCaseData(
                new List<string>() { "Row1", "Info", "More Stuff" },
                $"| Row1 | Info | More Stuff |{enl}| - | - | - |{enl}");
        }

        [TestCaseSource(nameof(TableHeaderData))]
        public void CanWriteTableHeader(List<string> headerValues, string expectedMarkdown)
        {
            StringBuilder sb = new StringBuilder();
            var tableWriter = new TableWriterFactory().Create(DocumentType.Md);
            _ = tableWriter.WriteTableHeader(sb, headerValues);

            string markdownSnippet = sb.ToString();
            Assert.AreEqual(expectedMarkdown, markdownSnippet);
        }

        private static IEnumerable<TestCaseData> TableRowData()
        {
            yield return new TestCaseData(
                new List<string>() { "Row1", "Info", "More Stuff" },
                $"| Row1 | Info | More Stuff |{enl}");
        }

        [TestCaseSource(nameof(TableRowData))]
        public void CanWriteTableRow(List<string> headerValues, string expectedMarkdown)
        {
            StringBuilder sb = new StringBuilder();
            var tableWriter = new TableWriterFactory().Create(DocumentType.Md);
            tableWriter.WriteTableRow(sb, headerValues, headerFirstColumn: false);

            string markdownSnippet = sb.ToString();
            Assert.AreEqual(expectedMarkdown, markdownSnippet);
        }

        private static IEnumerable<TestCaseData> TableData()
        {
            yield return new TestCaseData(
                new List<string>() { "Row1", "Info", "More Stuff" },
                new List<List<string>> { new List<string>() { "Row1", "Info", "More Stuff" } },
                false,
                $"| Row1 | Info | More Stuff |{enl}| ---- | ---- | ---------- |{enl}| Row1 | Info | More Stuff |{enl}");
            yield return new TestCaseData(
                 new List<string>() { "", "" },
                 new List<List<string>> { new List<string>() { "Byes", "4" }, new List<string>() { "Leg Byes", "3" } },
                 true,
                $"|              |   |{enl}| ------------ | - |{enl}| __Byes__     | 4 |{enl}| __Leg Byes__ | 3 |{enl}");

        }

        [TestCaseSource(nameof(TableData))]
        public void CanWriteTable(List<string> headerValues, List<List<string>> rowValues, bool header, string expectedMarkdown)
        {
            StringBuilder sb = new StringBuilder();
            var tableWriter = new TableWriterFactory().Create(DocumentType.Md);
            tableWriter.WriteTable(sb, headerValues, rowValues, headerFirstColumn: header);

            string markdownSnippet = sb.ToString();
            Assert.AreEqual(expectedMarkdown, markdownSnippet);
        }

        [TestCaseSource(nameof(TableData))]
        public void CanWriteTableWriting(List<string> headerValues, List<List<string>> rowValues, bool header, string expectedMarkdown)
        {
            StringBuilder sb = new StringBuilder();
            var tableWriter = new TableWriterFactory().Create(DocumentType.Md);
            tableWriter.WriteTable(sb, headerValues, rowValues, headerFirstColumn: header);

            string markdownSnippet = sb.ToString();
            Assert.AreEqual(expectedMarkdown, markdownSnippet);
        }
    }
}
