using System.Collections.Generic;
using System.Text;

using Common.Structure.ReportWriting.Markdown;

namespace Common.Structure.ReportWriting
{
    /// <summary>
    /// Contains routines for writing tables.
    /// </summary>
    public static class TableWriting
    {
        /// <summary>
        /// Writes an enumerable to a table, where the columns are all the properties of the type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of object to write out.</typeparam>
        /// <param name="sb">The writer to write with.</param>
        /// <param name="reportType">The type of file to export to.</param>
        /// <param name="values">The values to write into the table. The property names for <typeparamref name="T"/> give the header values.</param>
        /// <param name="headerFirstColumn">Whether first column should be header style or not.</param>
        public static void WriteTableFromEnumerable<T>(StringBuilder sb, DocumentType reportType, IEnumerable<IEnumerable<T>> values, bool headerFirstColumn)
        {
            var tableWriter = TableWriterFactory.Create(reportType);
            tableWriter.WriteTable(sb, values, headerFirstColumn);
        }

        /// <summary>
        /// Writes an enumerable to a table, where the columns are all the properties of the type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of object to write out.</typeparam>
        /// <param name="sb">The writer to write with.</param>
        /// <param name="reportType">The type of file to export to.</param>
        /// <param name="values">The values to write into the table. The property names for <typeparamref name="T"/> give the header values.</param>
        /// <param name="headerFirstColumn">Whether first column should be header style or not.</param>
        public static void WriteTable<T>(StringBuilder sb, DocumentType reportType, IEnumerable<T> values, bool headerFirstColumn)
        {
            var tableWriter = TableWriterFactory.Create(reportType);
            tableWriter.WriteTable(sb, values, headerFirstColumn);
        }

        /// <summary>
        /// Writes an enumerable to a table, where the column headers are specified./>.
        /// </summary>
        /// <typeparam name="T">The type of object to write out.</typeparam>
        /// <param name="sb">The writer to write with.</param>
        /// <param name="reportType">The type of file to export to.</param>
        /// <param name="headerValues">The values to write into the table header.</param>
        /// <param name="rowValues">The values to write for the table data. Each item is a row, and each row is a list of values. The values use the ToString() method to output the value.</param>
        /// <param name="headerFirstColumn">Whether first column should be header style or not.</param>
        public static void WriteTableFromEnumerable<T>(StringBuilder sb, DocumentType reportType, IEnumerable<string> headerValues, IEnumerable<IEnumerable<T>> rowValues, bool headerFirstColumn)
        {
            var tableWriter = TableWriterFactory.Create(reportType);
            tableWriter.WriteTable(sb, headerValues, rowValues, headerFirstColumn);
        }

        /// <summary>
        /// Writes a table to the file, given header values which are a subset of the property names of the type <typeparamref name="T"/>.
        /// <para> If the value in rowValues is null, then this writes a break line into the table.</para>
        /// </summary>
        /// <typeparam name="T">The object to write the properties out of.</typeparam>
        /// <param name="sb">The writer to write with.</param>
        /// <param name="reportType">The type of file to write to.</param>
        /// <param name="headerValues">The values to write out.</param>
        /// <param name="rowValues">The values to place in successive rows.</param>
        /// <param name="headerFirstColumn">Whether first column should be header style or not.</param>
        public static void WriteTable<T>(StringBuilder sb, DocumentType reportType, IEnumerable<string> headerValues, IEnumerable<T> rowValues, bool headerFirstColumn)
        {
            var tableWriter = TableWriterFactory.Create(reportType);
            tableWriter.WriteTable(sb, headerValues, rowValues, headerFirstColumn);
        }
    }
}
