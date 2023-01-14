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
        public void WriteEmptyRow(StringBuilder sb, int numberColumns, TableSettings settings)
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
        public int WriteTableHeader(StringBuilder sb, IReadOnlyList<string> valuesToWrite, TableSettings settings)
        {
            bool extremumWidthsSet = settings.MaxColumnWidth.HasValue || settings.MinColumnWidth.HasValue;
            string classNameString = null;
            if (extremumWidthsSet)
            {
                string styleName = settings.WidthStyleName();
                using (new WriteHtmlTag(sb, "style"))
                {
                    _ = sb.AppendLine($".{styleName}{{ max-width: {settings.MaxColumnWidth}px; min-width: {settings.MinColumnWidth}px;}}");
                }
                classNameString = $" class=\"{styleName}\"";
            }

            using (new WriteHtmlTag(sb, "thead"))
            using (new WriteHtmlTag(sb, "tr"))
            {
                if (valuesToWrite == null || valuesToWrite.Count == 0)
                {
                    return 0;
                }

                using (new WriteHtmlTag(sb, "th", $"scope=\"col\"{classNameString}", false))
                {
                    _ = sb.Append(valuesToWrite[0]);
                }

                for (int index = 1; index < valuesToWrite.Count; index++)
                {
                    string headerTag = $"th{classNameString}";
                    int columnWidth = settings.GetColumnWidth(index);
                    string extraString = columnWidth != 0 ? $" style=\"width:{columnWidth}px\"" : "";
                    using (new WriteHtmlTag(sb, headerTag, extraString, false))
                    {
                        _ = sb.Append(valuesToWrite[index]);
                    }
                }

                _ = sb.AppendLine();
            }

            _ = sb.AppendLine("<tbody>");
            return valuesToWrite.Count;
        }

        /// <inheritdoc/>
        public void WriteTableRow(StringBuilder sb, IReadOnlyList<string> valuesToWrite, TableSettings settings)
        {
            bool extremumWidthsSet = settings.MaxColumnWidth.HasValue || settings.MinColumnWidth.HasValue;
            string classNameString = null;
            if (extremumWidthsSet)
            {
                string styleName = settings.WidthStyleName();
                classNameString = $" class=\"{styleName}\"";
            }

            _ = sb.AppendLine("<tr>");
            int i = 0;

            if (valuesToWrite.Any())
            {
                foreach (string property in valuesToWrite)
                {
                    bool isDouble = double.TryParse(property, out double value);
                    if (settings.FirstColumnAsHeader)
                    {
                        if (i != 0)
                        {
                            if (value < 0)
                            {
                                _ = sb.Append($"<td data-negative{classNameString}>");
                            }
                            else
                            {
                                _ = sb.Append($"<td>{classNameString}");
                            }
                        }
                        else
                        {
                            _ = sb.Append($"<th scope=\"row\"{classNameString}>");
                        }
                    }
                    else
                    {
                        _ = sb.Append($"<td{classNameString}>");
                    }

                    _ = sb.Append(property);
                    if (settings.FirstColumnAsHeader)
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
        public void WriteTableStart(StringBuilder sb)
        {
            _ = sb.AppendLine(StartTag);
        }
    }
}
