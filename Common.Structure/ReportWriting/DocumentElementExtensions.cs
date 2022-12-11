namespace Common.Structure.ReportWriting
{
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

        /// <summary>
        /// Returns if the element is a header type.
        /// </summary>
        public static bool IsHeader(this DocumentElement documentElement)
        {
            return documentElement == DocumentElement.h1
                || documentElement == DocumentElement.h2
                || documentElement == DocumentElement.h3
                || documentElement == DocumentElement.h4
                || documentElement == DocumentElement.h5
                || documentElement == DocumentElement.h6;
        }
    }
}
