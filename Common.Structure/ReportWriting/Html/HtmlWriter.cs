using System.Text;

namespace Common.Structure.ReportWriting.Html
{
    internal sealed class HtmlWriter : ITextWriter
    {
        /// <inheritdoc/>
        public void WriteHeader(StringBuilder sb, string title, bool useColours)
        {
            _ = sb.AppendLine("<!DOCTYPE html>");
            _ = sb.AppendLine("<html lang=\"en\">");
            using (new WriteHtmlTag(sb, "head"))
            {
                _ = sb.AppendLine("<meta charset=\"utf-8\" http-equiv=\"x-ua-compatible\" content=\"IE=11\"/>");
                _ = sb.AppendLine($"<title>{title}</title>");
                using (new WriteHtmlTag(sb, "style"))
                {
                    _ = sb.AppendLine("html, h1, h2, h3, h4, h5, h6 { font-family: \"Arial\", cursive, sans-serif; }");
                    _ = sb.AppendLine("h1 { font-family: \"Arial\", cursive, sans-serif; margin-top: 1.5em; }");
                    _ = sb.AppendLine("h2 { font-family: \"Arial\", cursive, sans-serif; margin-top: 1.5em; }");
                    _ = sb.AppendLine("body{ font-family: \"Arial\", cursive, sans-serif; font-size: 10px; }");
                    _ = sb.AppendLine("table { border-collapse: collapse; }");
                    _ = sb.AppendLine("table { border: 1px solid black; }");
                    _ = sb.AppendLine("th, td { border: 1px solid black; max-width: 175px; min-width: 25px;}");
                    _ = sb.AppendLine("caption { margin-bottom: 1.2em; font-family: \"Arial\", cursive, sans-serif; font-size:medium; }");
                    _ = sb.AppendLine("tr { text-align: center; }");
                    _ = sb.AppendLine("div { max-width: 1000px; max-height: 600px; margin: left; margin-bottom: 1.5em; }");

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
                _ = sb.AppendLine("<script src=\"https://cdnjs.cloudflare.com/ajax/libs/Chart.js/2.9.4/Chart.min.js\"></script>");
                _ = sb.AppendLine("<script src=\"https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.29.1/moment.min.js\"></script>");
                _ = sb.AppendLine("<script src=\"https://cdn.jsdelivr.net/npm/chart.js@2.9.4/dist/Chart.min.js\"></script>");
            }
            _ = sb.AppendLine("<body>");
        }

        /// <inheritdoc/>
        public void WriteParagraph(StringBuilder sb, string[] sentence, DocumentElement tag = DocumentElement.p)
        {
            using (new WriteInlineHtmlTag(sb, tag.ToString()))
            {
                _ = sb.AppendLine(string.Join(" ", sentence));
            }
        }

        /// <inheritdoc/>
        public void WriteTitle(StringBuilder sb, string title, DocumentElement tag = DocumentElement.h1)
        {
            using (new WriteInlineHtmlTag(sb, tag.ToString()))
            {
                _ = sb.AppendLine(title);
            }
        }

        /// <inheritdoc/>
        public void WriteFooter(StringBuilder sb)
        {
            _ = sb.AppendLine("</body>");
            _ = sb.AppendLine("</html>");
        }
    }
}
