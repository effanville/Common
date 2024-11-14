using System.Text;

namespace Effanville.Common.ReportWriting.Writers.Csv;

/// <summary>
/// A writer of tables in csv format.
/// </summary>
public sealed class CsvTableWriter : ITableWriter
{
    /// <inheritdoc/>
    public void WriteEmptyRow(StringBuilder sb, int numberColumns)
    {
    }

    /// <inheritdoc/>
    public void WriteTableEnd(StringBuilder sb)
    {
    }

    /// <inheritdoc/>
    public int WriteTableHeader(StringBuilder sb, IReadOnlyList<string> headerValues)
    {
        _ = sb.AppendLine(string.Join(",", headerValues));
        return headerValues.Count;
    }

    /// <inheritdoc/>
    public int WriteTableHeader(StringBuilder sb, IReadOnlyList<string> headerValues, IReadOnlyList<int> columnWidths)
        => WriteTableHeader(sb, headerValues);

    /// <inheritdoc/>
    public void WriteTableRow(StringBuilder sb, IReadOnlyList<string> rowValues, bool headerFirstColumn)
        => _ = sb.AppendLine(string.Join(",", rowValues));

    /// <inheritdoc/>
    public void WriteTableRow(StringBuilder sb, IReadOnlyList<string> rowValues, bool headerFirstColumn, IReadOnlyList<int> columnWidths)
        => WriteTableRow(sb, rowValues, headerFirstColumn);

    /// <inheritdoc/>
    public void WriteTableStart(StringBuilder sb)
    {
    }
}
