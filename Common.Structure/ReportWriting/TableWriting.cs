using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Common.Structure.FileAccess;

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
        public static void WriteTableFromEnumerable<T>(StringBuilder sb, ExportType reportType, IEnumerable<IEnumerable<T>> values, bool headerFirstColumn)
        {
            T forTypes = default(T);
            foreach (T value in values)
            {
                if (value != null)
                {
                    forTypes = value;
                    break;
                }
            }

            if (forTypes == null)
            {
                return;
            }

            WriteTableFromEnumerable(sb, reportType, forTypes.GetType().GetProperties().Select(type => type.Name), values, headerFirstColumn);
        }

        /// <summary>
        /// Writes an enumerable to a table, where the columns are all the properties of the type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of object to write out.</typeparam>
        /// <param name="sb">The writer to write with.</param>
        /// <param name="reportType">The type of file to export to.</param>
        /// <param name="values">The values to write into the table. The property names for <typeparamref name="T"/> give the header values.</param>
        /// <param name="headerFirstColumn">Whether first column should be header style or not.</param>
        public static void WriteTable<T>(StringBuilder sb, ExportType reportType, IEnumerable<T> values, bool headerFirstColumn)
        {
            T forTypes = default(T);
            foreach (T value in values)
            {
                if (value != null)
                {
                    forTypes = value;
                    break;
                }
            }

            if (forTypes == null)
            {
                return;
            }

            WriteTable(sb, reportType, forTypes.GetType().GetProperties().Select(type => type.Name), values, headerFirstColumn);
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
        public static void WriteTableFromEnumerable<T>(StringBuilder sb, ExportType reportType, IEnumerable<string> headerValues, IEnumerable<IEnumerable<T>> rowValues, bool headerFirstColumn)
        {
            switch (reportType)
            {
                case ExportType.Csv:
                {
                    WriteTableHeader(sb, reportType, headerValues);
                    foreach (var value in rowValues)
                    {
                        WriteTableRow(sb, reportType, value.Select(val => val.ToString()), headerFirstColumn);
                    }

                    return;
                }
                case ExportType.Html:
                {
                    _ = sb.AppendLine("<table>");
                    WriteTableHeader(sb, reportType, headerValues);
                    _ = sb.AppendLine("<tbody>");

                    foreach (var value in rowValues)
                    {
                        if (value != null)
                        {
                            WriteTableRow(sb, reportType, value.Select(val => val.ToString()), headerFirstColumn);
                        }
                        if (value == null)
                        {
                            _ = sb.AppendLine("<tr><td><br/></td></tr>");
                        }
                    }

                    _ = sb.AppendLine("</tbody>");
                    _ = sb.AppendLine("</table>");
                    return;
                }
                default:
                {
                    return;
                }
            }
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
        public static void WriteTable<T>(StringBuilder sb, ExportType reportType, IEnumerable<string> headerValues, IEnumerable<T> rowValues, bool headerFirstColumn)
        {
            switch (reportType)
            {
                case ExportType.Csv:
                {
                    WriteTableHeader(sb, reportType, headerValues);
                    foreach (T value in rowValues)
                    {
                        PropertyInfo[] row = value.GetType().GetProperties();
                        WriteTableRow(sb, reportType, row.Where(info => headerValues.Contains(info.Name)).Select(ro => ro.GetValue(value)?.ToString()), headerFirstColumn);
                    }

                    return;
                }
                case ExportType.Html:
                {
                    _ = sb.AppendLine("<table>");
                    WriteTableHeader(sb, reportType, headerValues);
                    _ = sb.AppendLine("<tbody>");

                    foreach (T value in rowValues)
                    {
                        if (value != null)
                        {
                            PropertyInfo[] row = value.GetType().GetProperties();
                            WriteTableRow(sb, reportType, row.Where(info => headerValues.Contains(info.Name)).Select(ro => ro.GetValue(value)?.ToString()), headerFirstColumn);
                        }
                        else
                        {
                            _ = sb.AppendLine("<tr><td><br/></td></tr>");
                        }
                    }

                    _ = sb.AppendLine("</tbody>");
                    _ = sb.AppendLine("</table>");
                    return;
                }
                default:
                {
                    return;
                }
            }
        }

        /// <summary>
        /// Writes the header of a table
        /// </summary>
        /// <param name="sb">The StreamWriter to use</param>
        /// <param name="reportType">The type of file to export to</param>
        /// <param name="valuesToWrite">The values to use for the header names.</param>
        public static void WriteTableHeader(StringBuilder sb, ExportType reportType, IEnumerable<string> valuesToWrite)
        {
            switch (reportType)
            {
                case ExportType.Csv:
                {
                    _ = sb.AppendLine(string.Join(",", valuesToWrite));
                    return;
                }
                case ExportType.Html:
                {
                    _ = sb.AppendLine("<thead><tr>");
                    int i = 0;
                    foreach (string property in valuesToWrite)
                    {
                        if (i != 0)
                        {
                            _ = sb.Append("<th>");
                        }
                        else
                        {
                            _ = sb.Append("<th scope=\"col\">");
                        }

                        _ = sb.Append(property);
                        _ = sb.Append("</th>");
                        i++;
                    }

                    _ = sb.AppendLine();
                    _ = sb.AppendLine("</tr></thead>");
                    return;
                }
                default:
                {
                    return;
                }
            }
        }

        /// <summary>
        /// Writes a row of a table from an enumerable of string values.
        /// </summary>
        /// <param name="sb">The StreamWriter to use</param>
        /// <param name="exportType">The type of file to export to</param>
        /// <param name="valuesToWrite">The values to use for the header names.</param>
        /// <param name="headerFirstColumn">Whether first column should be header style or not.</param>
        public static void WriteTableRow(StringBuilder sb, ExportType exportType, IEnumerable<string> valuesToWrite, bool headerFirstColumn)
        {
            switch (exportType)
            {
                case ExportType.Csv:
                {
                    _ = sb.AppendLine(string.Join(",", valuesToWrite));
                    return;
                }
                case ExportType.Html:
                {
                    _ = sb.AppendLine("<tr>");
                    int i = 0;

                    if (valuesToWrite.Any())
                    {
                        foreach (string property in valuesToWrite)
                        {
                            bool isDouble = double.TryParse(property, out double value);
                            if (headerFirstColumn)
                            {
                                if (i != 0)
                                {
                                    if (value < 0)
                                    {
                                        _ = sb.Append("<td data-negative>");
                                    }
                                    else
                                    {
                                        _ = sb.Append("<td>");
                                    }
                                }
                                else
                                {
                                    _ = sb.Append("<th scope=\"row\">");
                                }
                            }
                            else
                            {
                                _ = sb.Append("<td>");
                            }

                            _ = sb.Append(property);
                            if (headerFirstColumn)
                            {
                                if (i != 0)
                                {
                                    _ = sb.Append("</td>");
                                }
                                else
                                {
                                    _ = sb.Append("</th>");
                                }
                            }
                            else
                            {
                                _ = sb.Append("</td>");
                            }
                            i++;
                        }
                    }
                    else
                    {
                        // row is empty, so write an empty row.
                        _ = sb.Append("<th scope=\"row\"></th>");
                    }

                    _ = sb.AppendLine();
                    _ = sb.AppendLine("</tr>");
                    return;
                }
                default:
                {
                    return;
                }
            }
        }
    }
}
