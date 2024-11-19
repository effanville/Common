using System.Collections.Generic;

using Effanville.Common.ReportWriting.Documents;
using Effanville.Common.ReportWriting.Writers;

using NUnit.Framework;

namespace Effanville.Common.ReportWriting.Unit.Tests
{
    [TestFixture]
    internal class ReportBuilderTests
    {
        private static string enl = TestConstants.EnvNewLine;

        public static IEnumerable<TestCaseData> WriteParagraphTestCases()
        {
            yield return new TestCaseData(DocumentType.Md, "", $"{enl}").SetName("ReportBuilderWriteParagraphTests-Empty Md String");
            yield return new TestCaseData(DocumentType.Html, "", $"<p></p>{enl}").SetName("ReportBuilderWriteParagraphTests-Empty html String");
            yield return new TestCaseData(DocumentType.Md, "Something", $"Something{enl}").SetName("ReportBuilderWriteParagraphTests-Md String");
            yield return new TestCaseData(DocumentType.Html, "Something", $"<p>Something</p>{enl}").SetName("ReportBuilderWriteParagraphTests-html String");
        }

        [TestCaseSource(nameof(WriteParagraphTestCases))]
        public void ReportBuilderWriteParagraphTests(DocumentType docType, string reportString, string expectedDocument)
        {
            var settings = DocumentWriterSettings.Default();
            ITableWriter tablew = new TableWriterFactory().Create(docType);
            ITextWriter tw = new TextWriterFactory().Create(docType, settings);
            IChartWriter cw = new ChartWriterFactory().Create(docType);
            var rb = new ReportBuilder(settings, tablew, tw, cw);

            _ = rb.WriteParagraph(new[] { reportString });

            string actualDocument = rb.ToString();
            Assert.That(actualDocument, Is.EqualTo(expectedDocument));
        }

        public static IEnumerable<TestCaseData> WriteHeaderTestCases()
        {
            yield return new TestCaseData(DocumentType.Md, "", $"# {enl}").SetName("ReportBuilderWriteTitleTests-Empty Md String");
            yield return new TestCaseData(DocumentType.Html, "", $"<h1></h1>{enl}").SetName("ReportBuilderWriteTitleTests-Empty html String");
            yield return new TestCaseData(DocumentType.Md, "Something", $"# Something{enl}").SetName("ReportBuilderWriteTitleTests-Md String");
            yield return new TestCaseData(DocumentType.Html, "Something", $"<h1>Something</h1>{enl}").SetName("ReportBuilderWriteTitleTests-html String");
        }

        [TestCaseSource(nameof(WriteHeaderTestCases))]
        public void ReportBuilderWriteTitleTests(DocumentType docType, string reportString, string expectedDocument)
        {
            var settings = DocumentWriterSettings.Default();
            ITableWriter tablew = new TableWriterFactory().Create(docType);
            ITextWriter tw = new TextWriterFactory().Create(docType, settings);
            IChartWriter cw = new ChartWriterFactory().Create(docType);
            var rb = new ReportBuilder(settings, tablew, tw, cw);

            _ = rb.WriteTitle(reportString, DocumentElement.h1);

            string actualDocument = rb.ToString();
            Assert.That(actualDocument, Is.EqualTo(expectedDocument));
        }
    }
}
