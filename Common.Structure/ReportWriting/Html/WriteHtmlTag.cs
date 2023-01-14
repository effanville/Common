using System;
using System.Text;

namespace Common.Structure.ReportWriting.Html
{
    /// <summary>
    /// Class used to write html tags with corresponding closing tag.
    /// </summary>
    public sealed class WriteHtmlTag : IDisposable
    {
        private readonly string fTag;
        private readonly bool fUseNewLines;
        private readonly StringBuilder fSb;

        /// <summary>
        /// Construct an instance and write the open tag.
        /// </summary>
        public WriteHtmlTag(StringBuilder sb, string tag, string extraHeader = null, bool useNewLines = true)
        {
            fTag = tag;
            fSb = sb;
            fUseNewLines = useNewLines;
            string headerString = string.IsNullOrWhiteSpace(extraHeader) ? "" : $" {extraHeader}";
            if (fUseNewLines)
            {
                _ = fSb.AppendLine($"<{fTag}{headerString}>");
            }
            else
            {
                _ = fSb.Append($"<{fTag}{headerString}>");
            }
        }

        /// <summary>
        /// Writes the closing html tag.
        /// </summary>
        public void Dispose()
        {
            if (fUseNewLines)
            {
                _ = fSb.AppendLine($"</{fTag}>");
            }
            else
            {
                _ = fSb.Append($"</{fTag}>");
            }
        }
    }
}
