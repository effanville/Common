using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Structure.ReportWriting
{
    /// <summary>
    /// Contains extension methods for an <see cref="ITableWriter"/>.
    /// </summary>
    public static class TableWriterExtensions
    {
        /// <summary>
        /// Writes an enumerable to a table, where the columns are all the properties of the type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of object to write out.</typeparam>
        /// <param name="tableWriter">The table writer for writing the table</param>
        /// <param name="sb">The writer to write with.</param>
        /// <param name="values">The values to write into the table. The property names for <typeparamref name="T"/> give the header values.</param>
        /// <param name="settings">The settings for how the table should be written.</param>
        public static void WriteTable<T>(
            this ITableWriter tableWriter,
            StringBuilder sb,
            IEnumerable<IEnumerable<T>> values,
            TableSettings settings)
        {
            T instanceForHeaderValues = default(T);
            foreach (T value in values)
            {
                if (value != null)
                {
                    instanceForHeaderValues = value;
                    break;
                }
            }

            if (instanceForHeaderValues == null)
            {
                return;
            }

            tableWriter.WriteTable(
                sb,
                instanceForHeaderValues.GetType().GetProperties().Select(type => type.Name),
                values,
                settings);
        }

        /// <summary>
        /// Writes an enumerable to a table, where the columns are all the properties of the type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of object to write out.</typeparam>
        /// <param name="tableWriter">The table writer for writing the table</param>
        /// <param name="sb">The writer to write with.</param>
        /// <param name="values">The values to write into the table. The property names for <typeparamref name="T"/> give the header values.</param>
        /// <param name="headerFirstColumn">Whether first column should be header style or not.</param>
        public static void WriteTable<T>(
            this ITableWriter tableWriter,
            StringBuilder sb,
            IEnumerable<T> values,
            TableSettings settings)
        {
            T instanceForHeaderValues = default(T);
            foreach (T value in values)
            {
                if (value != null)
                {
                    instanceForHeaderValues = value;
                    break;
                }
            }

            if (instanceForHeaderValues == null)
            {
                return;
            }

            tableWriter.WriteTable(
                sb,
                instanceForHeaderValues.GetType().GetProperties().Select(type => type.Name),
                values,
                settings);
        }

        /// <summary>
        /// Writes an enumerable to a table, where the column headers are specified./>.
        /// </summary>
        /// <typeparam name="T">The type of object to write out.</typeparam>
        /// <param name="tableWriter">The table writer for writing the table</param>
        /// <param name="sb">The writer to write with.</param>
        /// <param name="headerValues">The values to write into the table header.</param>
        /// <param name="rowValues">The values to write for the table data. Each item is a row, and each row is a list of values. The values use the ToString() method to output the value.</param>
        /// <param name="headerFirstColumn">Whether first column should be header style or not.</param>
        public static void WriteTable<T>(
            this ITableWriter tableWriter,
            StringBuilder sb,
            IEnumerable<string> headerValues,
            IEnumerable<IEnumerable<T>> rowValues,
            TableSettings settings)
        {
            tableWriter.WriteTable(
                sb, headerValues.ToList(),
                rowValues.Select(row => selectValues(row)).ToList(),
                settings);

            IReadOnlyList<string> selectValues(IEnumerable<T> row)
            {
                if (row == null)
                {
                    return null;
                }

                return row.Select(val => val.ToString()).ToList();
            }
        }

        /// <summary>
        /// Writes a table to the file, given header values which are a subset of the property names of the type <typeparamref name="T"/>.
        /// <para> If the value in rowValues is null, then this writes a break line into the table.</para>
        /// </summary>
        /// <typeparam name="T">The object to write the properties out of.</typeparam>
        /// <param name="tableWriter">The table writer for writing the table</param>
        /// <param name="sb">The writer to write with.</param>
        /// <param name="headerValues">The values to write out.</param>
        /// <param name="rowValues">The values to place in successive rows.</param>
        /// <param name="headerFirstColumn">Whether first column should be header style or not.</param>
        public static void WriteTable<T>(
            this ITableWriter tableWriter,
            StringBuilder sb,
            IEnumerable<string> headerValues,
            IEnumerable<T> rowValues,
            TableSettings settings)
        {
            tableWriter.WriteTable(
                sb,
                headerValues.ToList(),
                rowValues.Select(row => selectValues(row)).ToList(),
                settings);

            IReadOnlyList<string> selectValues(T row)
            {
                if (row == null)
                {
                    return null;
                }
                return row.GetType().GetProperties().Where(info => headerValues.Contains(info.Name)).Select(ro => ro.GetValue(row)?.ToString()).ToList();
            }
        }

        /// <summary>
        /// Writes a table to the file.
        /// </summary>
        /// <param name="sb">The writer to write with.</param>
        /// <param name="tableWriter">The table writer for writing the table</param>
        /// <param name="headerValues">The values to write out.</param>
        /// <param name="rowValues">The values to place in successive rows.</param>
        /// <param name="headerFirstColumn">Whether first column should be header style or not.</param>
        public static void WriteTable(
            this ITableWriter tableWriter,
            StringBuilder sb,
            IEnumerable<string> headerValues,
            IEnumerable<IEnumerable<string>> rowValues,
            TableSettings settings)
        {
            tableWriter.WriteTableStart(sb);
            int numberColumns = tableWriter.WriteTableHeader(sb, headerValues.ToList(), settings);
            foreach (var row in rowValues)
            {
                if (row != null)
                {
                    tableWriter.WriteTableRow(sb, row.ToList(), settings);
                }
                if (row == null)
                {
                    tableWriter.WriteEmptyRow(sb, numberColumns, settings);
                }
            }

            tableWriter.WriteTableEnd(sb);
        }

        /// <summary>
        /// Writes a table to the file.
        /// </summary>
        /// <param name="sb">The writer to write with.</param>
        /// <param name="tableWriter">The table writer for writing the table</param>
        /// <param name="headerValues">The values to write out.</param>
        /// <param name="rowValues">The values to place in successive rows.</param>
        /// <param name="headerFirstColumn">Whether first column should be header style or not.</param>
        public static void WriteTable(
            this ITableWriter tableWriter,
            StringBuilder sb,
            IReadOnlyList<string> headerValues,
            IReadOnlyList<IReadOnlyList<string>> rowValues,
            TableSettings settings)
        {
            TableSettings actualSettings = TableSettings.FromSettings(settings);

            // First calculate max lengths of all the values in the table.
            List<int> columnWidths = new List<int>();
            int numberHeaders = headerValues.Count;
            for (int i = 0; i < numberHeaders; i++)
            {
                columnWidths.Add(headerValues[i]?.Length ?? 0);
            }

            for (int j = 0; j < rowValues.Count; j++)
            {
                if (rowValues[j] == null)
                {
                    continue;
                }
                for (int i = 0; i < Math.Min(rowValues[j].Count, numberHeaders); i++)
                {
                    int headerFirstColumnAddition = i == 0 && settings.FirstColumnAsHeader ? 4 : 0;
                    int rowColumnLength = rowValues[j][i]?.Length + headerFirstColumnAddition ?? 0;
                    if (rowColumnLength > columnWidths[i])
                    {
                        columnWidths[i] = rowColumnLength;
                    }
                }
            }

            actualSettings.ColumnWidths = columnWidths;
            tableWriter.WriteTableStart(sb);
            int numberColumns = tableWriter.WriteTableHeader(sb, headerValues, actualSettings);
            foreach (var row in rowValues)
            {
                if (row != null)
                {
                    tableWriter.WriteTableRow(sb, row, actualSettings);
                }
                if (row == null)
                {
                    tableWriter.WriteEmptyRow(sb, numberColumns, actualSettings);
                }
            }

            tableWriter.WriteTableEnd(sb);
        }
    }
}
