namespace Effanville.Common.ReportWriting
{
    /// <summary>
    /// The settings for the report.
    /// </summary>
    public sealed class DocumentWriterSettings
    {
        /// <summary>
        /// Whether to use colour styling or not in the report.
        /// </summary>
        public bool UseColours { get; set; }

        /// <summary>
        /// Whether default styling should be applied.
        /// </summary>
        public bool UseDefaultStyle { get; set; } = false;

        /// <summary>
        /// Should the report include scripts.
        /// Note this is only relevant for html reports that may need to execute javascript
        /// scripts.
        /// </summary>
        public bool UseScripts { get; set; } = true;

        /// <summary>
        /// Construct an instance
        /// </summary>
        public DocumentWriterSettings()
        {
        }

        /// <summary>
        /// Construct an instance
        /// </summary>
        public DocumentWriterSettings(bool useColours, bool useDefaultStyle, bool useScripts)
        {
            UseColours = useColours;
            UseDefaultStyle = useDefaultStyle;
            UseScripts = useScripts;
        }

        /// <summary>
        /// Default settings for a <see cref="ReportBuilder"/>
        /// </summary>
        public static DocumentWriterSettings Default()
            => new DocumentWriterSettings(useColours: true, useDefaultStyle: true, useScripts: true);
    }
}
