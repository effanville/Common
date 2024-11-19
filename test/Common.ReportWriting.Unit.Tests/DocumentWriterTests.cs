using System.Collections.Generic;

using Effanville.Common.ReportWriting.Documents;
using Effanville.Common.ReportWriting.Writers;

using NUnit.Framework;

namespace Effanville.Common.ReportWriting.Unit.Tests;

[TestFixture]
internal class DocumentWriterTests
{
    private static string enl = TestConstants.EnvNewLine;

    public static IEnumerable<TestCaseData> CanWriteDocumentTestCases()
    {
        yield return new TestCaseData(DocumentType.Md, new Document(DocumentType.Md, ""), "")
            .SetName($"{nameof(CanWriteDocument)}-Empty Md String");

        yield return new TestCaseData(DocumentType.Html, new Document(DocumentType.Html, ""), $"")
            .SetName($"{nameof(CanWriteDocument)}-Empty html String");

        yield return new TestCaseData(DocumentType.Html, new Document(DocumentType.Html, "") { IncludesHeader = true }, $"<!DOCTYPE html>{enl}<html lang=\"en\">{enl}<head>{enl}<meta charset=\"utf-8\" http-equiv=\"x-ua-compatible\" content=\"IE=11\"/>{enl}<title></title>{enl}<script src=\"https://cdnjs.cloudflare.com/ajax/libs/Chart.js/2.9.4/Chart.min.js\"></script>{enl}<script src=\"https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.29.1/moment.min.js\"></script>{enl}<script src=\"https://cdn.jsdelivr.net/npm/chart.js@2.9.4/dist/Chart.min.js\"></script>{enl}</head>{enl}<body>{enl}</body>{enl}</html>{enl}")
            .SetName($"{nameof(CanWriteDocument)}-Empty html String with header");

        var mdDocumentBuilder = new DocumentBuilder(DocumentType.Md, "")
            .WriteParagraph("Something");

        yield return new TestCaseData(DocumentType.Md, mdDocumentBuilder.GetDocument(), $"Something{enl}")
            .SetName($"{nameof(CanWriteDocument)}-Md String");

        var htmlDocumentBuilder = new DocumentBuilder(DocumentType.Html, "")
            .WriteParagraph("Something");

        yield return new TestCaseData(DocumentType.Html, htmlDocumentBuilder.GetDocument(), $"<p>Something</p>{enl}")
            .SetName($"{nameof(CanWriteDocument)}-html String");

        _ = mdDocumentBuilder.Clear()
            .WriteTitle("Some Title", DocumentElement.h1)
            .WriteParagraph(new[] { "Some sentence here" }, DocumentElement.p);
        yield return new TestCaseData(DocumentType.Md, mdDocumentBuilder.GetDocument(), $"# Some Title{enl}Some sentence here{enl}")
            .SetName($"{nameof(CanWriteDocument)}-Md-Title and sentence");

        _ = htmlDocumentBuilder.Clear()
            .WriteTitle("Some Title", DocumentElement.h1)
            .WriteParagraph(new[] { "Some sentence here" }, DocumentElement.p);
        yield return new TestCaseData(DocumentType.Html, htmlDocumentBuilder.GetDocument(), $"<h1>Some Title</h1>{enl}<p>Some sentence here</p>{enl}")
            .SetName($"{nameof(CanWriteDocument)}-Html-Title and sentence");

        _ = mdDocumentBuilder.Clear()
            .WriteTitle("Some Title", DocumentElement.h1)
            .WriteTable(new List<string> { "Header 1", "Header 2" },
                new List<List<string>> { new List<string> { "Value-1-1", "Value-1-2" }, new List<string> { "Values-2-1", "Value-2-2" } },
                headerFirstColumn: true)
            .WriteTable(new List<string> { "Header A", "Header B", "Header C" },
                new List<List<string>> { new List<string> { "Value-1-A", "Value-1-B", "Value-1-C" }, new List<string> { "Values-2-A", "Value-2-B", "Value-2-C" } },
                headerFirstColumn: true);
        yield return new TestCaseData(DocumentType.Md, mdDocumentBuilder.GetDocument(), $"# Some Title{enl}| Header 1       | Header 2  |{enl}| -------------- | --------- |{enl}| __Value-1-1__  | Value-1-2 |{enl}| __Values-2-1__ | Value-2-2 |{enl}| Header A       | Header B  | Header C  |{enl}| -------------- | --------- | --------- |{enl}| __Value-1-A__  | Value-1-B | Value-1-C |{enl}| __Values-2-A__ | Value-2-B | Value-2-C |{enl}")
            .SetName($"{nameof(CanWriteDocument)}-Md-Title and twotable");

        _ = htmlDocumentBuilder.Clear()
            .WriteTitle("Some Title", DocumentElement.h1)
            .WriteTable(new List<string> { "Header 1", "Header 2" },
                new List<List<string>> { new List<string> { "Value-1-1", "Value-1-2" }, new List<string> { "Values-2-1", "Value-2-2" } },
                headerFirstColumn: true)
            .WriteTable(new List<string> { "Header A", "Header B", "Header C" },
                new List<List<string>> { new List<string> { "Value-1-A", "Value-1-B", "Value-1-C" }, new List<string> { "Values-2-A", "Value-2-B", "Value-2-C" } },
                headerFirstColumn: true);
        yield return new TestCaseData(DocumentType.Html, htmlDocumentBuilder.GetDocument(), $"<h1>Some Title</h1>{enl}<table>{enl}<thead><tr>{enl}<th scope=\"col\">Header 1</th><th>Header 2</th>{enl}</tr></thead>{enl}<tbody>{enl}<tr>{enl}<th scope=\"row\">Value-1-1</th><td>Value-1-2</td>{enl}</tr>{enl}<tr>{enl}<th scope=\"row\">Values-2-1</th><td>Value-2-2</td>{enl}</tr>{enl}</tbody>{enl}</table>{enl}<table>{enl}<thead><tr>{enl}<th scope=\"col\">Header A</th><th>Header B</th><th>Header C</th>{enl}</tr></thead>{enl}<tbody>{enl}<tr>{enl}<th scope=\"row\">Value-1-A</th><td>Value-1-B</td><td>Value-1-C</td>{enl}</tr>{enl}<tr>{enl}<th scope=\"row\">Values-2-A</th><td>Value-2-B</td><td>Value-2-C</td>{enl}</tr>{enl}</tbody>{enl}</table>{enl}")
            .SetName($"{nameof(CanWriteDocument)}-Html-Title and two table");

        _ = mdDocumentBuilder.Clear()
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
            mdDocumentBuilder.GetDocument(),
            $"# Some Title{enl}| Header 1       | Header 2  |{enl}| -------------- | --------- |{enl}| __Value-1-1__  | Value-1-2 |{enl}| __Values-2-1__ | Value-2-2 |{enl}| Header A       | Header B  | Header C  |{enl}| -------------- | --------- | --------- |{enl}| __Value-1-A__  | Value-1-B | Value-1-C |{enl}| __Values-2-A__ | Value-2-B | Value-2-C |{enl}Then I went to the shops. And I did some stuff(sic).{enl}")
            .SetName($"{nameof(CanWriteDocument)}-Md-Title, table and paragraph");

        _ = htmlDocumentBuilder.Clear()
             .WriteTitle("Some Title", DocumentElement.h1)
             .WriteTable(new List<string> { "Header 1", "Header 2" },
                 new List<List<string>> { new List<string> { "Value-1-1", "Value-1-2" }, new List<string> { "Values-2-1", "Value-2-2" } },
                 headerFirstColumn: true)
             .WriteTable(new List<string> { "Header A", "Header B", "Header C" },
                            new List<List<string>> { new List<string> { "Value-1-A", "Value-1-B", "Value-1-C" }, new List<string> { "Values-2-A", "Value-2-B", "Value-2-C" } },
                            headerFirstColumn: true)
             .WriteParagraph(new[] { "Then I went to the shops.", "And I did some stuff(sic)." });
        yield return new TestCaseData(
            DocumentType.Html,
            htmlDocumentBuilder.GetDocument(),
            $"<h1>Some Title</h1>{enl}<table>{enl}<thead><tr>{enl}<th scope=\"col\">Header 1</th><th>Header 2</th>{enl}</tr></thead>{enl}<tbody>{enl}<tr>{enl}<th scope=\"row\">Value-1-1</th><td>Value-1-2</td>{enl}</tr>{enl}<tr>{enl}<th scope=\"row\">Values-2-1</th><td>Value-2-2</td>{enl}</tr>{enl}</tbody>{enl}</table>{enl}<table>{enl}<thead><tr>{enl}<th scope=\"col\">Header A</th><th>Header B</th><th>Header C</th>{enl}</tr></thead>{enl}<tbody>{enl}<tr>{enl}<th scope=\"row\">Value-1-A</th><td>Value-1-B</td><td>Value-1-C</td>{enl}</tr>{enl}<tr>{enl}<th scope=\"row\">Values-2-A</th><td>Value-2-B</td><td>Value-2-C</td>{enl}</tr>{enl}</tbody>{enl}</table>{enl}<p>Then I went to the shops. And I did some stuff(sic).</p>{enl}")
            .SetName($"{nameof(CanWriteDocument)}-Html-Title, table and paragraph");
    }

    [TestCaseSource(nameof(CanWriteDocumentTestCases))]
    public void CanWriteDocument(DocumentType docType, Document document, string expectedDocument)
    {
        var settings = DocumentWriterSettings.Default();
        ITableWriter tablew = new TableWriterFactory().Create(docType);
        ITextWriter tw = new TextWriterFactory().Create(docType, settings);
        IChartWriter cw = new ChartWriterFactory().Create(docType);
        var documentWriter = new DocumentWriter(settings, tablew, tw, cw);

        string? actualDocument = documentWriter.Write(document)?.ToString();
        Assert.That(actualDocument, Is.EqualTo(expectedDocument));
    }
}
