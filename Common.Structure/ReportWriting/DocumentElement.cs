namespace Common.Structure.ReportWriting
{
    /// <summary>
    /// Container for all used document tags
    /// </summary>
    public enum DocumentElement
    {
        /// <summary>
        /// Default element type that is no element.
        /// </summary>
        None,

        /// <summary>
        /// The first header tag.
        /// </summary>
        h1,

        /// <summary>
        /// The second header tag.
        /// </summary>
        h2,

        /// <summary>
        /// The third header tag.
        /// </summary>
        h3,

        /// <summary>
        /// The fourth header tag.
        /// </summary>
        h4,

        /// <summary>
        /// The fifth header tag.
        /// </summary>
        h5,

        /// <summary>
        /// The sixth header tag.
        /// </summary>
        h6,

        /// <summary>
        /// Paragraph tag.
        /// </summary>
        p,

        /// <summary>
        /// The table tag.
        /// </summary>
        table,

        /// <summary>
        /// The chart element part
        /// </summary>
        chart
    }
}