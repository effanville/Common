using System.Collections.Generic;
using System.Text;

namespace Common.Structure.ReportWriting.Csv
{
    /// <summary>
    /// A writer of tables in csv format.
    /// </summary>
    public sealed class CsvTableWriter : ITableWriter
    {
        /// <inheritdoc/>
        public void WriteEmptyRow(StringBuilder sb, int numberColumns, TableSettings settings)
        {
        }

        /// <inheritdoc/>
        public void WriteTableEnd(StringBuilder sb)
        {
        }

        /// <inheritdoc/>
        public int WriteTableHeader(StringBuilder sb, IReadOnlyList<string> headerValues, TableSettings settings)
        {
            _ = sb.AppendLine(string.Join(",", headerValues));
            return headerValues.Count;
        }

        /// <inheritdoc/>
        public void WriteTableRow(StringBuilder sb, IReadOnlyList<string> rowValues, TableSettings settings)
        {
            _ = sb.AppendLine(string.Join(",", rowValues));
        }

        /// <inheritdoc/>
        public void WriteTableStart(StringBuilder sb)
        {
        }
    }
}
