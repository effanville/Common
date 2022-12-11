using System.Collections.Generic;

using Common.Structure.ReportWriting;
using Common.Structure.ReportWriting.Document;

using NUnit.Framework;

namespace Common.Structure.Tests.ReportWriting
{
    [TestFixture]
    internal sealed class ReportSplitterTests
    {
        public static IEnumerable<TestCaseData> Tests()
        {
            yield return new TestCaseData(DocumentType.Md, "", null).SetName("Empty Md String");
            yield return new TestCaseData(DocumentType.Md, (string)null, null).SetName("Null Md String");
            yield return new TestCaseData(DocumentType.Html, "", null).SetName("Empty Html String");

            var mdDdocument = new DocumentBuilder(DocumentType.Md)
                .WriteTitle("Some Title", DocumentElement.h1)
                .WriteParagraph(new[] { "Some sentence here" }, DocumentElement.p)
                .GetDocument();
            yield return new TestCaseData(DocumentType.Md, "# Some Title\r\nSome sentence here\r\n", mdDdocument).SetName("Title and Paragraph Md");

            var tableMdDocument = new DocumentBuilder(DocumentType.Md)
                .WriteTitle("Some Title", DocumentElement.h1)
                .WriteTable<string>(new List<string> { "Header 1", "Header 2" },
                    new List<List<string>> { new List<string> { "Value-1-1", "Value-1-2" }, new List<string> { "Values-2-1", "Value-2-2" } },
                    headerFirstColumn: true);
            yield return new TestCaseData(
                DocumentType.Md,
                "# Some Title\r\n| Header 1       | Header 2  |\r\n| -------------- | --------- |\r\n| __Value-1-1__  | Value-1-2 |\r\n| __Values-2-1__ | Value-2-2 |\r\n",
                tableMdDocument.GetDocument())
                .SetName("Title and Header Md");
            var twotableMdDocument = new DocumentBuilder(DocumentType.Md)
             .WriteTitle("Some Title", DocumentElement.h1)
             .WriteTable<string>(new List<string> { "Header 1", "Header 2" },
                 new List<List<string>> { new List<string> { "Value-1-1", "Value-1-2" }, new List<string> { "Values-2-1", "Value-2-2" } },
                 headerFirstColumn: true)
             .WriteTable<string>(new List<string> { "Header A", "Header B", "Header C" },
                            new List<List<string>> { new List<string> { "Value-1-A", "Value-1-B", "Value-1-C" }, new List<string> { "Values-2-A", "Value-2-B", "Value-2-C" } },
                            headerFirstColumn: true);
            yield return new TestCaseData(
                DocumentType.Md,
                "# Some Title\r\n| Header 1       | Header 2  |\r\n| -------------- | --------- |\r\n| __Value-1-1__  | Value-1-2 |\r\n| __Values-2-1__ | Value-2-2 |\r\n| Header A       | Header B  | Header C  |\r\n| -------------- | --------- | --------- |\r\n| __Value-1-A__  | Value-1-B | Value-1-C |\r\n| __Values-2-A__ | Value-2-B | Value-2-C |\r\n",
                twotableMdDocument.GetDocument())
                .SetName("Title and Two Table Md");

            var twotableSentenceMdDocument = new DocumentBuilder(DocumentType.Md)
                 .WriteTitle("Some Title", DocumentElement.h1)
                 .WriteTable<string>(new List<string> { "Header 1", "Header 2" },
                     new List<List<string>> { new List<string> { "Value-1-1", "Value-1-2" }, new List<string> { "Values-2-1", "Value-2-2" } },
                     headerFirstColumn: true)
                 .WriteTable<string>(new List<string> { "Header A", "Header B", "Header C" },
                                new List<List<string>> { new List<string> { "Value-1-A", "Value-1-B", "Value-1-C" }, new List<string> { "Values-2-A", "Value-2-B", "Value-2-C" } },
                                headerFirstColumn: true)
                 .WriteParagraph(new[] { "Then I went to the shops.", "And I did some stuff(sic)." });
            yield return new TestCaseData(
                DocumentType.Md,
                "# Some Title\r\n| Header 1       | Header 2  |\r\n| -------------- | --------- |\r\n| __Value-1-1__  | Value-1-2 |\r\n| __Values-2-1__ | Value-2-2 |\r\n| Header A       | Header B  | Header C  |\r\n| -------------- | --------- | --------- |\r\n| __Value-1-A__  | Value-1-B | Value-1-C |\r\n| __Values-2-A__ | Value-2-B | Value-2-C |\r\nThen I went to the shops. And I did some stuff(sic).\r\n",
                twotableSentenceMdDocument.GetDocument())
                .SetName("Title and Two Table and sentence Md");

            var htmldocument = new DocumentBuilder(DocumentType.Html)
                .WriteTitle("Some Title", DocumentElement.h1)
                .WriteParagraph(new[] { "Some sentence here" }, DocumentElement.p)
                .GetDocument();
            yield return new TestCaseData(DocumentType.Html, "<h1>Some Title</h1>\r\n<p>Some sentence here</p>\r\n", htmldocument).SetName("Title and Paragraph html");

            var otherHtmlDocument = new DocumentBuilder(DocumentType.Html)
                .WriteTitle("Some Title", DocumentElement.h1)
                .WriteTitle("Smaller Title", DocumentElement.h2)
                .GetDocument();
            yield return new TestCaseData(DocumentType.Html, "<h1>Some Title</h1>\r\n<h2>Smaller Title</h2>\r\n", otherHtmlDocument).SetName("Title and Smaller Title html");

            var tableHtmlDocument = new DocumentBuilder(DocumentType.Html)
                .WriteTitle("Some Title", DocumentElement.h1)
                .WriteTable<string>(new List<string> { "Header 1", "Header 2" },
                    new List<List<string>> { new List<string> { "Value-1-1", "Value-1-2" }, new List<string> { "Values-2-1", "Value-2-2" } },
                    headerFirstColumn: true)
                .GetDocument();
            yield return new TestCaseData(
                DocumentType.Html,
                "<h1>Some Title</h1>\r\n<table>\r\n<thead><tr>\r\n<th scope=\"col\">Header 1</th><th>Header 2</th>\r\n</tr></thead>\r\n<tbody>\r\n<tr>\r\n<th scope=\"row\">Value-1-1</th><td>Value-1-2</td>\r\n</tr>\r\n<tr>\r\n<th scope=\"row\">Values-2-1</th><td>Value-2-2</td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n",
                tableHtmlDocument)
                .SetName("Title and Header Html");

            var twoTableHtmlDocument = new DocumentBuilder(DocumentType.Html)
             .WriteTitle("Some Title", DocumentElement.h1)
             .WriteTable<string>(new List<string> { "Header 1", "Header 2" },
                 new List<List<string>> { new List<string> { "Value-1-1", "Value-1-2" }, new List<string> { "Values-2-1", "Value-2-2" } },
                 headerFirstColumn: true)
             .WriteTable<string>(new List<string> { "Header A", "Header B", "header C" },
                            new List<List<string>> { new List<string> { "Value-1-A", "Value-1-B", "Value-1-C" }, new List<string> { "Values-2-A", "Value-2-B", "Value-2-C" } },
                            headerFirstColumn: true)
             .GetDocument();
            yield return new TestCaseData(
                DocumentType.Html,
                "<h1>Some Title</h1>\r\n<table>\r\n<thead><tr>\r\n<th scope=\"col\">Header 1</th><th>Header 2</th>\r\n</tr></thead>\r\n<tbody>\r\n<tr>\r\n<th scope=\"row\">Value-1-1</th><td>Value-1-2</td>\r\n</tr>\r\n<tr>\r\n<th scope=\"row\">Values-2-1</th><td>Value-2-2</td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n<table>\r\n<thead><tr>\r\n<th scope=\"col\">Header A</th><th>Header B</th><th>header C</th>\r\n</tr></thead>\r\n<tbody>\r\n<tr>\r\n<th scope=\"row\">Value-1-A</th><td>Value-1-B</td><td>Value-1-C</td>\r\n</tr>\r\n<tr>\r\n<th scope=\"row\">Values-2-A</th><td>Value-2-B</td><td>Value-2-C</td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n",
                twoTableHtmlDocument)
                .SetName("Title and Two table Html");
        }

        [TestCaseSource(nameof(Tests))]
        public void RunTest(DocumentType docType, string reportString, Document expectedDocument)
        {
            Document actualDocument = ReportSplitter.SplitReportString(docType, reportString);
            Assert.AreEqual(expectedDocument, actualDocument);
        }
    }
}
