using System;
using System.IO;
using System.IO.Abstractions;
using System.Text;

using Common.Structure.ReportWriting;

namespace Common.Statistics
{
    /// <summary>
    /// Extensions for the <see cref="IGenericStatCollection{S, T}"/> interface.
    /// </summary>
    public static class StatCollectionExtensions
    {
        /// <summary>
        /// Export the collection to a file.
        /// </summary>
        /// <param name="collection">The collection to export.</param>
        /// <param name="fileSystem">The filesystem to use.</param>
        /// <param name="filePath">The path to export to.</param>
        /// <param name="documentType">The type of file to export to</param>
        /// <param name="error">A string recording any error, if present.</param>
        public static void ExportStats<S, T>(this IGenericStatCollection<S, T> collection, IFileSystem fileSystem, string filePath, DocumentType documentType, out string error)
            where S : Enum
            where T : class
        {
            error = null;
            try
            {
                StringBuilder sb = collection.ExportStats(documentType, DocumentElement.h1);

                using (Stream stream = fileSystem.FileStream.Create(filePath, FileMode.Create))
                using (StreamWriter fileWriter = new StreamWriter(stream))
                {
                    fileWriter.WriteLine(sb.ToString());
                }
            }
            catch (IOException ex)
            {
                error = $"Error in Exporting: {ex.Message}";
                return;
            }
        }
    }
}
