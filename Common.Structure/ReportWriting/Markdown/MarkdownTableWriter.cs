
using System.Collections.Generic;
using System.Text;

namespace Common.Structure.ReportWriting.Markdown
{
    /// <summary>
    /// A table writer for markdown documents.
    /// </summary>
    public sealed class MarkdownTableWriter : ITableWriter
    {
        /// <inheritdoc/>
        public void WriteEmptyRow(StringBuilder sb, int numberColumns, TableSettings settings)
        {
            _ = sb.Append('|');
            for (int columnIndex = 0; columnIndex < numberColumns; columnIndex++)
            {
                _ = sb.Append(' ')
                    .Append('|');
            }

            _ = sb.AppendLine();
        }

        /// <inheritdoc/>
        public void WriteTableEnd(StringBuilder sb)
        {
        }

        /// <inheritdoc/>
        public void WriteTableStart(StringBuilder sb)
        {
        }

        /// <inheritdoc/>
        public int WriteTableHeader(StringBuilder sb, IReadOnlyList<string> valuesToWrite, TableSettings settings)
        {
            if (valuesToWrite == null || valuesToWrite.Count == 0)
            {
                return -1;
            }

            _ = sb.Append('|');
            StringBuilder headerRowBuilder = new StringBuilder();
            _ = headerRowBuilder.Append('|');
            for (int columnIndex = 0; columnIndex < valuesToWrite.Count; columnIndex++)
            {
                int paddingValue = settings.GetColumnWidth(columnIndex);
                _ = sb.Append($" {valuesToWrite[columnIndex].PadRight(paddingValue)} ")
                    .Append('|');
                _ = headerRowBuilder.Append($" {"-".PadRight(paddingValue, '-')} ")
                    .Append('|');
            }

            _ = sb.AppendLine()
                .AppendLine(headerRowBuilder.ToString());
            return valuesToWrite.Count;
        }

        /// <inheritdoc/>
        public void WriteTableRow(StringBuilder sb, IReadOnlyList<string> valuesToWrite, TableSettings settings)
        {
            if (valuesToWrite == null || valuesToWrite.Count == 0)
            {
                return;
            }

            _ = sb.Append('|');
            int startIndex = 0;
            if (settings.FirstColumnAsHeader)
            {
                startIndex++;
                string boldValue = $"__{valuesToWrite[0]}__";
                int paddingValue = settings.GetColumnWidth(0);
                _ = sb.Append($" {boldValue.PadRight(paddingValue)} ")
                    .Append('|');
            }

            for (int columnIndex = startIndex; columnIndex < valuesToWrite.Count; columnIndex++)
            {
                int paddingValue = settings.GetColumnWidth(columnIndex);
                var value = valuesToWrite[columnIndex] ?? " ";
                _ = sb.Append($" {value.PadRight(paddingValue)} ")
                    .Append('|');
            }

            _ = sb.AppendLine();
        }
    }
}
