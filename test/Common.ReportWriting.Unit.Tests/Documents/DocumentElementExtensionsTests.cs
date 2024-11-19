using NUnit.Framework;
using Effanville.Common.ReportWriting.Documents;

namespace Effanville.Common.ReportWriting.Unit.Tests.Documents
{
    internal sealed class DocumentElementExtensionsTests
    {
        [TestCase(DocumentElement.h1, DocumentElement.h2)]
        [TestCase(DocumentElement.h2, DocumentElement.h3)]
        [TestCase(DocumentElement.h3, DocumentElement.h4)]
        [TestCase(DocumentElement.h4, DocumentElement.h5)]
        [TestCase(DocumentElement.h5, DocumentElement.h6)]
        [TestCase(DocumentElement.h6, DocumentElement.h6)]
        [TestCase(DocumentElement.p, DocumentElement.p)]
        [TestCase(DocumentElement.None, DocumentElement.p)]
        [TestCase(DocumentElement.br, DocumentElement.p)]
        [TestCase(DocumentElement.table, DocumentElement.p)]
        [TestCase(DocumentElement.chart, DocumentElement.p)]
        public void GetNextTests(DocumentElement element, DocumentElement expectedElement)
        {
            var actualElement = element.GetNext();
            Assert.That(actualElement, Is.EqualTo(expectedElement));
        }

        [TestCase(DocumentElement.h1, true)]
        [TestCase(DocumentElement.h2, true)]
        [TestCase(DocumentElement.h3, true)]
        [TestCase(DocumentElement.h4, true)]
        [TestCase(DocumentElement.h5, true)]
        [TestCase(DocumentElement.h6, true)]
        [TestCase(DocumentElement.p, false)]
        [TestCase(DocumentElement.None, false)]
        [TestCase(DocumentElement.br, false)]
        [TestCase(DocumentElement.table, false)]
        [TestCase(DocumentElement.chart, false)]
        public void IsHeaderTests(DocumentElement element, bool expectedResult)
        {
            bool actualResult = element.IsHeader();
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }
    }
}
