
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
        public void WriteEmptyRow(StringBuilder sb, int numberColumns)
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
        public int WriteTableHeader(StringBuilder sb, IReadOnlyList<string> valuesToWrite)
        {
            _ = sb.Append('|');
            StringBuilder headerRowBuilder = new StringBuilder();
            _ = headerRowBuilder.Append('|');
            foreach (string property in valuesToWrite)
            {
                _ = sb.Append($" {property} ")
                    .Append('|');
                _ = headerRowBuilder.Append(" - ")
                    .Append('|');
            }

            _ = sb.AppendLine()
                .AppendLine(headerRowBuilder.ToString());
            return valuesToWrite.Count;
        }

        /// <inheritdoc/>
        public int WriteTableHeader(StringBuilder sb, IReadOnlyList<string> valuesToWrite, IReadOnlyList<int> columnWidths)
        {
            _ = sb.Append('|');
            StringBuilder headerRowBuilder = new StringBuilder();
            _ = headerRowBuilder.Append('|');
            for (int columnIndex = 0; columnIndex < valuesToWrite.Count; columnIndex++)
            {
                _ = sb.Append($" {valuesToWrite[columnIndex].PadRight(columnWidths[columnIndex])} ")
                    .Append('|');
                _ = headerRowBuilder.Append($" {"-".PadRight(columnWidths[columnIndex], '-')} ")
                    .Append('|');
            }

            _ = sb.AppendLine()
                .AppendLine(headerRowBuilder.ToString());
            return valuesToWrite.Count;
        }

        /// <inheritdoc/>
        public void WriteTableRow(StringBuilder sb, IReadOnlyList<string> valuesToWrite, bool headerFirstColumn)
        {
            _ = sb.Append('|');
            int i = 0;
            foreach (string property in valuesToWrite)
            {
                if (i == 0 && headerFirstColumn)
                {
                    _ = sb.Append($" __{property}__ ")
                        .Append('|');
                    continue;
                }
                _ = sb.Append($" {property} ")
                    .Append('|');
                i++;
            }

            _ = sb.AppendLine();
        }

        /// <inheritdoc/>
        public void WriteTableRow(StringBuilder sb, IReadOnlyList<string> valuesToWrite, bool headerFirstColumn, IReadOnlyList<int> columnWidths)
        {
            if (valuesToWrite == null || valuesToWrite.Count == 0)
            {
                return;
            }

            if (valuesToWrite.Count != columnWidths.Count)
            {
                return;
            }

            _ = sb.Append('|');
            int startIndex = 0;
            if (headerFirstColumn)
            {
                startIndex++;
                string boldValue = $"__{valuesToWrite}__";
                _ = sb.Append($" {boldValue.PadRight(columnWidths[0])} ")
                    .Append('|');
            }

            for (int columnIndex = startIndex; columnIndex < valuesToWrite.Count; columnIndex++)
            {
                _ = sb.Append($" {valuesToWrite[columnIndex].PadRight(columnWidths[columnIndex])} ")
                    .Append('|');
            }

            _ = sb.AppendLine();
        }
    }
}
