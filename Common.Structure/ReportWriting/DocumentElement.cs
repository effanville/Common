namespace Common.Structure.ReportWriting
{
    /// <summary>
    /// Container for all used document tags
    /// </summary>
    public enum DocumentElement
    {
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
        p
    }

    /// <summary>
    /// Extensions for the <see cref="DocumentElement"/> enum.
    /// </summary>
    public static class DocumentElementExtensions
    {
        /// <summary>
        /// Get the next lower level enum value.
        /// </summary>
        public static DocumentElement GetNext(this DocumentElement docElement)
        {
            switch (docElement)
            {
                case DocumentElement.h1:
                    return DocumentElement.h2;
                case DocumentElement.h2:
                    return DocumentElement.h3;
                case DocumentElement.h3:
                    return DocumentElement.h4;
                case DocumentElement.h4:
                    return DocumentElement.h5;
                case DocumentElement.h5:
                case DocumentElement.h6:
                    return DocumentElement.h6;
                case DocumentElement.p:
                default:
                    return DocumentElement.p;
            }
        }
    }
}