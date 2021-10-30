using System.Text;

namespace Common.Structure.ReportWriting
{
    /// <summary>
    /// Class containing default routines to write to any file type in <see cref="ReportType"/>.
    /// </summary>
    public static class FileWritingSupport
    {
        /// <summary>
        /// Writes a paragraph to the file.
        /// </summary>
        /// <param name="sb">The writer to use</param>
        /// <param name="reportType">The type of file to export to.</param>
        /// <param name="sentence">The sentence to export.</param>
        /// <param name="tag">The <see cref="HtmlTag"/> to use.</param>
        public static void WriteParagraph(StringBuilder sb, ReportType reportType, string[] sentence, HtmlTag tag = HtmlTag.p)
        {
            switch (reportType)
            {
                case ReportType.Csv:
                {
                    _ = sb.AppendLine(string.Join(",", sentence));
                    return;
                }
                case ReportType.Html:
                {
                    using (new WriteInlineHtmlTag(sb, tag.ToString()))
                    {
                        _ = sb.Append(string.Join(" ", sentence));
                    }

                    return;
                }
                default:
                    return;
            }
        }

        /// <summary>
        /// Writes a title line to the file.
        /// </summary>
        /// <param name="sb">The writer to use.</param>
        /// <param name="reportType">The type of file to export to.</param>
        /// <param name="title">The title string to write.</param>
        /// <param name="tag">The specific <see cref="HtmlTag"/> to use in this title.</param>
        public static void WriteTitle(StringBuilder sb, ReportType reportType, string title, HtmlTag tag = HtmlTag.h1)
        {
            switch (reportType)
            {
                case ReportType.Csv:
                {
                    _ = sb.AppendLine("");
                    _ = sb.AppendLine(title);
                    _ = sb.AppendLine("");
                    return;
                }
                case ReportType.Html:
                {
                    using (new WriteInlineHtmlTag(sb, tag.ToString()))
                    {
                        _ = sb.Append(title);
                    }

                    return;
                }
                default:
                    return;
            }
        }

        /// <summary>
        /// Creates a generic header with default styles for a html page.
        /// </summary>
        /// <param name="sb">The stringbuilder to write the page with</param>
        /// <param name="title">A title to give the page</param>
        /// <param name="useColours">Whether to use colour styling or not.</param>
        public static void CreateHTMLHeader(StringBuilder sb, string title, bool useColours)
        {
            _ = sb.AppendLine("<!DOCTYPE html>");
            _ = sb.AppendLine("<html>");
            using (new WriteHtmlTag(sb, "head"))
            {
                _ = sb.AppendLine($"<title>{title}</title>");
                using (new WriteHtmlTag(sb, "style"))
                {
                    _ = sb.AppendLine("html, h1, h2, h3, h4, h5, h6 {font-family: \"Arial\", cursive, sans-serif; }");
                    _ = sb.AppendLine("h1 { font-family: \"Arial\", cursive, sans-serif; margin-top: 1.5em; }");
                    _ = sb.AppendLine("h2 { font-family: \"Arial\", cursive, sans-serif; margin-top: 1.5em; }");
                    _ = sb.AppendLine("body{ font-family: \"Arial\", cursive, sans-serif; font-size: 10px }");
                    _ = sb.AppendLine("table { border-collapse: collapse;}");
                    _ = sb.AppendLine("table, th, td { border: 1px solid black; }");
                    _ = sb.AppendLine("caption { margin-bottom: 1.2em; font-family: \"Arial\", cursive, sans-serif; font-size:medium; }");
                    _ = sb.AppendLine("tr {text-align: center;}");
                    _ = sb.AppendLine("div { max-width: 1000px; max-height: 600px; margin: left; margin-bottom: 1.5em;");

                    if (useColours)
                    {
                        _ = sb.AppendLine("tr:nth-child(even) {background-color: #f0f8ff;}");
                        _ = sb.AppendLine("th{ background-color: #ADD8E6; height: 1.5em; }");
                        _ = sb.AppendLine("[data-negative] { background-color: red;}");
                    }
                    else
                    {
                        _ = sb.AppendLine("th{ height: 1.5em; }");
                    }

                    _ = sb.AppendLine("p { line-height: 1.5em; margin-bottom: 1.5em;}");
                }

                // include namespace for rendering charts.
                _ = sb.AppendLine("<script src=\"https://cdnjs.cloudflare.com/ajax/libs/Chart.js/2.6.0/Chart.min.js\"></script>");
            }
            _ = sb.AppendLine("<body>");
        }

        /// <summary>
        /// Creates a generic footer for a html page.
        /// </summary>
        public static void CreateHTMLFooter(StringBuilder sb)
        {
            _ = sb.AppendLine("</body>");
            _ = sb.AppendLine("</html>");
        }
    }
}
