﻿using System.Collections.Generic;
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
            StringBuilder sb = new StringBuilder();
            var tableWriter = TableWriterFactory.Create(DocumentType.Md);
            _ = tableWriter.WriteTableHeader(sb, headerValues);

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
            StringBuilder sb = new StringBuilder();
            var tableWriter = TableWriterFactory.Create(DocumentType.Md);
            tableWriter.WriteTableRow(sb, headerValues, headerFirstColumn: false);

            string markdownSnippet = sb.ToString();
            Assert.AreEqual(expectedMarkdown, markdownSnippet);
        }

        private static IEnumerable<TestCaseData> TableData()
        {
            yield return new TestCaseData(
                new List<string>() { "Row1", "Info", "More Stuff" },
                new List<List<string>> { new List<string>() { "Row1", "Info", "More Stuff" } },
                "| Row1 | Info | More Stuff |\r\n| ---- | ---- | ---------- |\r\n| Row1 | Info | More Stuff |\r\n");

        }

        [TestCaseSource(nameof(TableData))]
        public void CanWriteTable(List<string> headerValues, List<List<string>> rowValues, string expectedMarkdown)
        {
            StringBuilder sb = new StringBuilder();
            var tableWriter = TableWriterFactory.Create(DocumentType.Md);
            tableWriter.WriteTable(sb, headerValues, rowValues, headerFirstColumn: false);

            string markdownSnippet = sb.ToString();
            Assert.AreEqual(expectedMarkdown, markdownSnippet);
        }
    }
}