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
        /// <param name="settings">The settings for how the table should be written.</param>
        /// <returns>The number of columns for the table.</returns>
        int WriteTableHeader(StringBuilder sb, IReadOnlyList<string> headerValues, TableSettings settings);

        /// <summary>
        /// Writes a row of a table from an enumerable of string values.
        /// </summary>
        /// <param name="sb">The StringBuilder to use</param>
        /// <param name="rowValues">The values to use for the header names.</param>
        /// <param name="settings">The settings for how the table should be written.</param>
        void WriteTableRow(StringBuilder sb, IReadOnlyList<string> rowValues, TableSettings settings);

        /// <summary>
        /// Write an empty row into the table.
        /// </summary>
        /// <param name="sb">The StringBuilder to use</param>
        /// <param name="numberColumns"></param>
        /// <param name="settings">The settings for how the table should be written.</param>
        void WriteEmptyRow(StringBuilder sb, int numberColumns, TableSettings settings);
    }
}
