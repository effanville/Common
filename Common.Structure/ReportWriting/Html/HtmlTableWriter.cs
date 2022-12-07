using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Structure.ReportWriting.Html
{
    /// <summary>
    /// A writer of tables in an Html format.
    /// </summary>
    public sealed class HtmlTableWriter : ITableWriter
    {
        public static readonly string StartTag = $"<{DocumentElement.table}>";
        public static readonly string EndTag = $"</{DocumentElement.table}>";

        /// <inheritdoc/>
        public void WriteEmptyRow(StringBuilder sb, int numberColumns)
        {
            _ = sb.AppendLine("<tr><td><br/></td></tr>");
        }

        /// <inheritdoc/>
        public void WriteTableEnd(StringBuilder sb)
        {
            _ = sb.AppendLine("</tbody>");
            _ = sb.AppendLine(EndTag);
        }

        /// <summary>
        /// Writes the header of a table
        /// </summary>
        /// <param name="sb">The StreamWriter to use</param>
        /// <param name="valuesToWrite">The values to use for the header names.</param>
        public int WriteTableHeader(StringBuilder sb, IReadOnlyList<string> valuesToWrite)
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
            _ = sb.AppendLine("<tbody>");
            return valuesToWrite.Count;
        }

        /// <inheritdoc/>
        public int WriteTableHeader(StringBuilder sb, IReadOnlyList<string> valuesToWrite, IReadOnlyList<int> columnWidths)
        {
            return WriteTableHeader(sb, valuesToWrite);
        }

        /// <inheritdoc/>
        public void WriteTableRow(StringBuilder sb, IReadOnlyList<string> valuesToWrite, bool headerFirstColumn)
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
        }

        /// <inheritdoc/>
        public void WriteTableRow(StringBuilder sb, IReadOnlyList<string> valuesToWrite, bool headerFirstColumn, IReadOnlyList<int> columnWidths)
        {
            WriteTableRow(sb, valuesToWrite, headerFirstColumn);
        }

        /// <inheritdoc/>
        public void WriteTableStart(StringBuilder sb)
        {
            _ = sb.AppendLine(StartTag);
        }
    }
}
