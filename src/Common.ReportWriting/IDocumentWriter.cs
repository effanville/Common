using System.Text;

using Effanville.Common.ReportWriting.Documents;

namespace Effanville.Common.ReportWriting
{
    /// <summary>
    /// Writes a document in the given format
    /// </summary>
    public interface IDocumentWriter
    {
        /// <summary>
        /// Write the document into a stringbuilder.
        /// </summary>
        StringBuilder Write(Document document);
    }
}
