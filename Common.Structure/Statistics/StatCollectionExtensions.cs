using System;
using System.IO;
using System.IO.Abstractions;
using System.Text;

using Common.Structure.ReportWriting;

namespace Common.Statistics
{
    public static class StatCollectionExtensions
    {
        /// <summary>
        /// Export the collection to a file.
        /// </summary>
        /// <param name="collection">The collection to export.</param>
        /// <param name="fileSystem"></param>
        /// <param name="filePath"></param>
        /// <param name="exportType"></param>
        public static void ExportStats<S, T>(this IGenericStatCollection<S, T> collection, IFileSystem fileSystem, string filePath, DocumentType exportType)
            where S : Enum
            where T : class
        {
            try
            {
                StringBuilder sb = collection.ExportStats(exportType, DocumentElement.h1);

                using (Stream stream = fileSystem.FileStream.Create(filePath, FileMode.Create))
                using (StreamWriter fileWriter = new StreamWriter(stream))
                {
                    fileWriter.WriteLine(sb.ToString());
                }
            }
            catch (IOException)
            {
                return;
            }
        }
    }
}
