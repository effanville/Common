﻿using System.Text;

namespace Effanville.Common.ReportWriting.Writers.Html
{
    /// <summary>
    /// Class used to write html tags with corresponding closing tag.
    /// </summary>
    public sealed class WriteInlineHtmlTag : IDisposable
    {
        private readonly string fTag;
        private readonly StringBuilder fSb;

        /// <summary>
        /// Construct an instance and write the open tag.
        /// </summary>
        public WriteInlineHtmlTag(StringBuilder sb, string tag)
        {
            fTag = tag;
            fSb = sb;
            _ = fSb.Append($"<{fTag}>");
        }

        /// <summary>
        /// Writes the closing html tag.
        /// </summary>
        public void Dispose()
        {
            _ = fSb.AppendLine($"</{fTag}>");
        }
    }
}
