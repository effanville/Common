using System.Collections.Generic;

using Effanville.Common.ReportWriting.Documents;
using Effanville.Common.ReportWriting.Writers;

using NUnit.Framework;

namespace Effanville.Common.ReportWriting.Unit.Tests;

[TestFixture]
public class DocumentBuilderRoundTripTests
{
    private static string enl = TestConstants.EnvNewLine;

    public static IEnumerable<TestCaseData> CanRoundTripStringTests()
    {
        yield return new TestCaseData(DocumentType.Md, "").SetName($"{nameof(CanRoundTripStringToDocument)}-Empty Md String");
        yield return new TestCaseData(DocumentType.Md, null).SetName($"{nameof(CanRoundTripStringToDocument)}-Null Md String");
        yield return new TestCaseData(DocumentType.Html, null).SetName($"{nameof(CanRoundTripStringToDocument)}-Empty Html String");

        yield return new TestCaseData(
            DocumentType.Md,
            $"# Some Title{enl}Some sentence here{enl}").SetName($"{nameof(CanRoundTripStringToDocument)}-Title and Paragraph Md");

        yield return new TestCaseData(
            DocumentType.Md,
            $"# Some Title{enl}| Header 1       | Header 2  |{enl}| -------------- | --------- |{enl}| __Value-1-1__  | Value-1-2 |{enl}| __Values-2-1__ | Value-2-2 |{enl}")
            .SetName($"{nameof(CanRoundTripStringToDocument)}-Title and Header Md");
        yield return new TestCaseData(
            DocumentType.Md,
            $"# Some Title{enl}| Header 1       | Header 2  |{enl}| -------------- | --------- |{enl}| __Value-1-1__  | Value-1-2 |{enl}| __Values-2-1__ | Value-2-2 |{enl}| Header A       | Header B  | Header C  |{enl}| -------------- | --------- | --------- |{enl}| __Value-1-A__  | Value-1-B | Value-1-C |{enl}| __Values-2-A__ | Value-2-B | Value-2-C |{enl}")
            .SetName($"{nameof(CanRoundTripStringToDocument)}-Title and Two Table Md");

        yield return new TestCaseData(
            DocumentType.Md,
            $"# Some Title{enl}| Header 1       | Header 2  |{enl}| -------------- | --------- |{enl}| __Value-1-1__  | Value-1-2 |{enl}| __Values-2-1__ | Value-2-2 |{enl}| Header A       | Header B  | Header C  |{enl}| -------------- | --------- | --------- |{enl}| __Value-1-A__  | Value-1-B | Value-1-C |{enl}| __Values-2-A__ | Value-2-B | Value-2-C |{enl}Then I went to the shops. And I did some stuff(sic).{enl}")
            .SetName($"{nameof(CanRoundTripStringToDocument)}-Title and Two Table and sentence Md");

        yield return new TestCaseData(
            DocumentType.Html,
            $"<h1>Some Title</h1>{enl}<p>Some sentence here</p>{enl}").SetName($"{nameof(CanRoundTripStringToDocument)}-Title and Paragraph html");

        yield return new TestCaseData(
            DocumentType.Html,
            $"<h1>Some Title</h1>{enl}<h2>Smaller Title</h2>{enl}").SetName($"{nameof(CanRoundTripStringToDocument)}-Title and Smaller Title html");

        yield return new TestCaseData(
            DocumentType.Html,
            $"<h1>Some Title</h1>{enl}<table>{enl}<thead><tr>{enl}<th scope=\"col\">Header 1</th><th>Header 2</th>{enl}</tr></thead>{enl}<tbody>{enl}<tr>{enl}<th scope=\"row\">Value-1-1</th><td>Value-1-2</td>{enl}</tr>{enl}<tr>{enl}<th scope=\"row\">Values-2-1</th><td>Value-2-2</td>{enl}</tr>{enl}</tbody>{enl}</table>{enl}")
            .SetName($"{nameof(CanRoundTripStringToDocument)}-Title and Header Html");
        yield return new TestCaseData(
            DocumentType.Html,
            $"<h1>Some Title</h1>{enl}<table>{enl}<thead><tr>{enl}<th scope=\"col\">Header 1</th><th>Header 2</th>{enl}</tr></thead>{enl}<tbody>{enl}<tr>{enl}<th scope=\"row\">Value-1-1</th><td>Value-1-2</td>{enl}</tr>{enl}<tr>{enl}<th scope=\"row\">Values-2-1</th><td>Value-2-2</td>{enl}</tr>{enl}</tbody>{enl}</table>{enl}<table>{enl}<thead><tr>{enl}<th scope=\"col\">Header A</th><th>Header B</th><th>header C</th>{enl}</tr></thead>{enl}<tbody>{enl}<tr>{enl}<th scope=\"row\">Value-1-A</th><td>Value-1-B</td><td>Value-1-C</td>{enl}</tr>{enl}<tr>{enl}<th scope=\"row\">Values-2-A</th><td>Value-2-B</td><td>Value-2-C</td>{enl}</tr>{enl}</tbody>{enl}</table>{enl}")
            .SetName($"{nameof(CanRoundTripStringToDocument)}-Title and Two table Html");
    }

    [TestCaseSource(nameof(CanRoundTripStringTests))]
    public void CanRoundTripStringToDocument(DocumentType docType, string reportString)
    {
        Document? actualDocument = ReportSplitter.SplitReportString(docType, reportString);

        var settings = DocumentWriterSettings.Default();
        ITableWriter tablew = new TableWriterFactory().Create(docType);
        ITextWriter tw = new TextWriterFactory().Create(docType, settings);
        IChartWriter cw = new ChartWriterFactory().Create(docType);
        DocumentWriter documentWriter = new DocumentWriter(settings, tablew, tw, cw);
        string? output = documentWriter.Write(actualDocument)?.ToString();
        if (string.IsNullOrEmpty(reportString))
        {
            Assert.True(string.IsNullOrEmpty(output));
            return;
        }
        Assert.AreEqual(reportString, output);
    }

    public static IEnumerable<TestCaseData> CanRoundTripDocumentTests()
    {
        yield return new TestCaseData(DocumentType.Md, null).SetName($"{nameof(CanRoundTripDocumentTest)}-null Md String");
        yield return new TestCaseData(DocumentType.Html, null).SetName($"{nameof(CanRoundTripDocumentTest)}-null Html String");

        yield return new TestCaseData(DocumentType.Md, new Document(DocumentType.Md, "")).SetName($"{nameof(CanRoundTripDocumentTest)}-Empty Md String");
        yield return new TestCaseData(DocumentType.Html, new Document(DocumentType.Html, "")).SetName($"{nameof(CanRoundTripDocumentTest)}-Empty Html String");

        var mdDdocument = new DocumentBuilder(DocumentType.Md, "")
            .WriteTitle("Some Title", DocumentElement.h1)
            .WriteParagraph(new[] { "Some sentence here" }, DocumentElement.p)
            .GetDocument();
        yield return new TestCaseData(
            DocumentType.Md,
            mdDdocument).SetName($"{nameof(CanRoundTripDocumentTest)}-Title and Paragraph Md");

        var tableMdDocument = new DocumentBuilder(DocumentType.Md, "")
            .WriteTitle("Some Title", DocumentElement.h1)
            .WriteTable(new List<string> { "Header 1", "Header 2" },
                new List<List<string>> { new List<string> { "Value-1-1", "Value-1-2" }, new List<string> { "Values-2-1", "Value-2-2" } },
                headerFirstColumn: true);
        yield return new TestCaseData(
            DocumentType.Md,
            tableMdDocument.GetDocument())
            .SetName($"{nameof(CanRoundTripDocumentTest)}-Title and Header Md");
        var twotableMdDocument = new DocumentBuilder(DocumentType.Md, "")
         .WriteTitle("Some Title", DocumentElement.h1)
         .WriteTable(new List<string> { "Header 1", "Header 2" },
             new List<List<string>> { new List<string> { "Value-1-1", "Value-1-2" }, new List<string> { "Values-2-1", "Value-2-2" } },
             headerFirstColumn: true)
         .WriteTable(new List<string> { "Header A", "Header B", "Header C" },
                        new List<List<string>> { new List<string> { "Value-1-A", "Value-1-B", "Value-1-C" }, new List<string> { "Values-2-A", "Value-2-B", "Value-2-C" } },
                        headerFirstColumn: true);
        yield return new TestCaseData(
            DocumentType.Md,
            twotableMdDocument.GetDocument())
            .SetName($"{nameof(CanRoundTripDocumentTest)}-Title and Two Table Md");

        var twotableSentenceMdDocument = new DocumentBuilder(DocumentType.Md, "Some Title")
             .WriteTitle("Some Title", DocumentElement.h1)
             .WriteTable(new List<string> { "Header 1", "Header 2" },
                 new List<List<string>> { new List<string> { "Value-1-1", "Value-1-2" }, new List<string> { "Values-2-1", "Value-2-2" } },
                 headerFirstColumn: true)
             .WriteTable(new List<string> { "Header A", "Header B", "Header C" },
                            new List<List<string>> { new List<string> { "Value-1-A", "Value-1-B", "Value-1-C" }, new List<string> { "Values-2-A", "Value-2-B", "Value-2-C" } },
                            headerFirstColumn: true)
             .WriteParagraph(new[] { "Then I went to the shops.", "And I did some stuff(sic)." });
        yield return new TestCaseData(
            DocumentType.Md,
            twotableSentenceMdDocument.GetDocument())
            .SetName($"{nameof(CanRoundTripDocumentTest)}-Title and Two Table and sentence Md");

        var htmldocument = new DocumentBuilder(DocumentType.Html, "Some Title")
            .WriteTitle("Some Title", DocumentElement.h1)
            .WriteParagraph(new[] { "Some sentence here" }, DocumentElement.p)
            .GetDocument();
        yield return new TestCaseData(
            DocumentType.Html,
            htmldocument).SetName($"{nameof(CanRoundTripDocumentTest)}-Title and Paragraph html");

        var otherHtmlDocument = new DocumentBuilder(DocumentType.Html, "Some Title")
            .WriteTitle("Some Title", DocumentElement.h1)
            .WriteTitle("Smaller Title", DocumentElement.h2)
            .GetDocument();
        yield return new TestCaseData(
            DocumentType.Html,
            otherHtmlDocument).SetName($"{nameof(CanRoundTripDocumentTest)}-Title and Smaller Title html");

        var tableHtmlDocument = new DocumentBuilder(DocumentType.Html, "Some Title")
            .WriteTitle("Some Title", DocumentElement.h1)
            .WriteTable(new List<string> { "Header 1", "Header 2" },
                new List<List<string>> { new List<string> { "Value-1-1", "Value-1-2" }, new List<string> { "Values-2-1", "Value-2-2" } },
                headerFirstColumn: true)
            .GetDocument();
        yield return new TestCaseData(
            DocumentType.Html,
            tableHtmlDocument)
            .SetName($"{nameof(CanRoundTripDocumentTest)}-Title and Header Html");

        var twoTableHtmlDocument = new DocumentBuilder(DocumentType.Html, "Some Title")
         .WriteTitle("Some Title", DocumentElement.h1)
         .WriteTable(new List<string> { "Header 1", "Header 2" },
             new List<List<string>> { new List<string> { "Value-1-1", "Value-1-2" }, new List<string> { "Values-2-1", "Value-2-2" } },
             headerFirstColumn: true)
         .WriteTable(new List<string> { "Header A", "Header B", "header C" },
                        new List<List<string>> { new List<string> { "Value-1-A", "Value-1-B", "Value-1-C" }, new List<string> { "Values-2-A", "Value-2-B", "Value-2-C" } },
                        headerFirstColumn: true)
         .GetDocument();
        yield return new TestCaseData(
            DocumentType.Html,
            twoTableHtmlDocument)
            .SetName($"{nameof(CanRoundTripDocumentTest)}-Title and Two table Html");
    }

    [TestCaseSource(nameof(CanRoundTripDocumentTests))]
    public void CanRoundTripDocumentTest(DocumentType docType, Document expectedDocument)
    {
        var settings = DocumentWriterSettings.Default();
        ITableWriter tablew = new TableWriterFactory().Create(docType);
        ITextWriter tw = new TextWriterFactory().Create(docType, settings);
        IChartWriter cw = new ChartWriterFactory().Create(docType);
        DocumentWriter documentWriter = new DocumentWriter(settings, tablew, tw, cw);
        string? output = documentWriter.Write(expectedDocument)?.ToString();

        Document? actualDocument = ReportSplitter.SplitReportString(docType, output);

        Assert.AreEqual(expectedDocument, actualDocument);
    }
}
