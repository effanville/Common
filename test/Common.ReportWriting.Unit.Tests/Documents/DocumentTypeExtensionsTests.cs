using NUnit.Framework;
using Effanville.Common.ReportWriting.Documents;

namespace Effanville.Common.ReportWriting.Unit.Tests.Documents
{
    internal sealed class DocumentTypeExtensionsTests
    {
        [TestCase(DocumentType.Html, DocumentElement.h1, "<h1>")]
        [TestCase(DocumentType.Html, DocumentElement.h2, "<h2>")]
        [TestCase(DocumentType.Html, DocumentElement.h3, "<h3>")]
        [TestCase(DocumentType.Html, DocumentElement.h4, "<h4>")]
        [TestCase(DocumentType.Html, DocumentElement.h5, "<h5>")]
        [TestCase(DocumentType.Html, DocumentElement.h6, "<h6>")]
        [TestCase(DocumentType.Html, DocumentElement.p, "<p>")]
        [TestCase(DocumentType.Html, DocumentElement.br, "<br>")]
        [TestCase(DocumentType.Html, DocumentElement.chart, "<div>")]
        [TestCase(DocumentType.Html, DocumentElement.table, "<table>")]
        [TestCase(DocumentType.Html, DocumentElement.None, "<None>")]
        [TestCase(DocumentType.Md, DocumentElement.h1, "#")]
        [TestCase(DocumentType.Md, DocumentElement.h2, "##")]
        [TestCase(DocumentType.Md, DocumentElement.h3, "###")]
        [TestCase(DocumentType.Md, DocumentElement.h4, "###")]
        [TestCase(DocumentType.Md, DocumentElement.h5, "###")]
        [TestCase(DocumentType.Md, DocumentElement.h6, "###")]
        [TestCase(DocumentType.Md, DocumentElement.p, "\r\n")]
        [TestCase(DocumentType.Md, DocumentElement.br, "\r\n")]
        [TestCase(DocumentType.Md, DocumentElement.chart, "ChartChart")]
        [TestCase(DocumentType.Md, DocumentElement.table, "|")]
        [TestCase(DocumentType.Md, DocumentElement.None, "\r\n")]
        [TestCase(DocumentType.Csv, DocumentElement.p, null)]
        [TestCase(DocumentType.Csv, DocumentElement.None, null)]
        public void StringFormTests(DocumentType docType, DocumentElement element, string expectedForm)
        {
            string actualElement = docType.StringForm(element);
            Assert.AreEqual(expectedForm, actualElement);
        }
    }
}
