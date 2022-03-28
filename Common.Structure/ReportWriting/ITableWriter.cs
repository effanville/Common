using System.Collections.Generic;
using System.Text;

namespace Common.Structure.ReportWriting
{
    /// <summary>
    /// Contains methods for writing tables.
    /// </summary>
    public interface ITableWriter
    {
        /// <summary>
        /// Write any beginning part for the table.
        /// </summary>
        /// <param name="sb">The StringBuilder to use</param>
        void WriteTableStart(StringBuilder sb);

        /// <summary>
        /// Write any ending part for the table.
        /// </summary>
        /// <param name="sb">The StringBuilder to use</param>
        void WriteTableEnd(StringBuilder sb);

        /// <summary>
        /// Writes the header of a table
        /// </summary>
        /// <param name="sb">The StringBuilder to use</param>
        /// <param name="headerValues">The values to use for the header names.</param>
        /// <returns>The number of columns for the table.</returns>
        int WriteTableHeader(StringBuilder sb, IReadOnlyList<string> headerValues);

        /// <summary>
        /// Writes the header of a table
        /// </summary>
        /// <param name="sb">The StringBuilder to use</param>
        /// <param name="headerValues">The values to use for the header names.</param>
        /// <param name="columnWidths">Specifies the specific widths for the columns.</param>
        /// <returns>The number of columns for the table.</returns>
        int WriteTableHeader(StringBuilder sb, IReadOnlyList<string> headerValues, IReadOnlyList<int> columnWidths);

        /// <summary>
        /// Writes a row of a table from an enumerable of string values.
        /// </summary>
        /// <param name="sb">The StringBuilder to use</param>
        /// <param name="rowValues">The values to use for the header names.</param>
        /// <param name="headerFirstColumn">Whether first column should be header style or not.</param>
        void WriteTableRow(StringBuilder sb, IReadOnlyList<string> rowValues, bool headerFirstColumn);

        /// <summary>
        /// Write an empty row into the table.
        /// </summary>
        /// <param name="sb">The StringBuilder to use</param>
        /// <param name="numberColumns"></param>
        void WriteEmptyRow(StringBuilder sb, int numberColumns);

        /// <summary>
        /// Writes a row of a table from an enumerable of string values.
        /// </summary>
        /// <param name="sb">The StringBuilder to use</param>
        /// <param name="rowValues">The values to use for the header names.</param>
        /// <param name="headerFirstColumn">Whether first column should be header style or not.</param>
        /// <param name="columnWidths">Specifies the specific widths for the columns.</param>
        void WriteTableRow(StringBuilder sb, IReadOnlyList<string> rowValues, bool headerFirstColumn, IReadOnlyList<int> columnWidths);
    }
}
