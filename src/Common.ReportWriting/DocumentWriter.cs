using System.Text;

using Effanville.Common.ReportWriting.Documents;

namespace Effanville.Common.ReportWriting
{
    /// <inheritdoc cref="IDocumentWriter"/>
    public sealed class DocumentWriter : IDocumentWriter
    {
        private readonly DocumentWriterSettings _settings;
        private readonly ITextWriter _textWriter;
        private readonly ITableWriter _tableWriter;
        private readonly IChartWriter _chartWriter;

        /// <summary>
        /// Construct an instance.
        /// </summary>
        public DocumentWriter(
            DocumentWriterSettings settings,
            ITableWriter tableWriter,
            ITextWriter textWriter,
            IChartWriter chartWriter)
        {
            _settings = settings;
            _tableWriter = tableWriter;
            _textWriter = textWriter;
            _chartWriter = chartWriter;
        }

        /// <inheritdoc />
        public StringBuilder? Write(Document document)
        {
            if (document == null)
            {
                return null;
            }

            StringBuilder sb = new StringBuilder();

            if (document.IncludesHeader)
            {
                _textWriter.WriteHeader(sb, document.Title, _settings.UseColours);
            }

            foreach (DocumentPart part in document.Parts)
            {
                switch (part.Element)
                {
                    case DocumentElement.h1:
                    case DocumentElement.h2:
                    case DocumentElement.h3:
                    case DocumentElement.h4:
                    case DocumentElement.h5:
                    case DocumentElement.h6:
                        _textWriter.WriteTitle(sb, part.ConstituentString, part.Element);
                        break;
                    case DocumentElement.p:
                    case DocumentElement.br:
                        if (part is TextDocumentPart textPart)
                        {
                            _textWriter.WriteParagraph(sb, textPart.Text, textPart.Element);
                        }
                        else
                        {
                            _textWriter.WriteParagraph(sb, new[] { part.ConstituentString }, part.Element);
                        }
                        break;
                    case DocumentElement.table:
                        if (part is TableDocumentPart tablePart)
                        {
                            _tableWriter.WriteTable(sb, tablePart.TableHeaders, tablePart.TableRows, tablePart.FirstColumnAsHeader);
                        }
                        break;
                    case DocumentElement.chart:
                        break;
                    case DocumentElement.None:
                    default:
                        break;
                }
            }
            if (document.IncludesHeader)
            {
                _textWriter.WriteFooter(sb);
            }
            return sb;
        }
    }
}
