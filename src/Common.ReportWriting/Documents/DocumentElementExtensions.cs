namespace Effanville.Common.ReportWriting.Documents
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
            return docElement switch
            {
                DocumentElement.h1 => DocumentElement.h2,
                DocumentElement.h2 => DocumentElement.h3,
                DocumentElement.h3 => DocumentElement.h4,
                DocumentElement.h4 => DocumentElement.h5,
                DocumentElement.h5 or DocumentElement.h6 => DocumentElement.h6,
                DocumentElement.None => DocumentElement.p,
                DocumentElement.p => DocumentElement.p,
                DocumentElement.br => DocumentElement.p,
                DocumentElement.table => DocumentElement.p,
                DocumentElement.chart => DocumentElement.p,
                _ => DocumentElement.p,
            };
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
