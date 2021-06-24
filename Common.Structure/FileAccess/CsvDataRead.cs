using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using Common.Structure.Reporting;

namespace Common.Structure.FileAccess
{
    /// <summary>
    /// Contains routines to extract data from csv files.
    /// </summary>
    public static class CsvReaderWriter
    {
        /// <summary>
        /// Reads data from a csv file.
        /// </summary>
        /// <param name="dataGainer">The object to read data out of.</param>
        /// <param name="filePath">The path of file to read from.</param>
        /// <param name="reportLogger">Reporting Callback.</param>
        public static List<object> ReadFromCsv<T>(T dataGainer, string filePath, IReportLogger reportLogger = null) where T : ICSVAccess
        {
            return ReadFromCsv(dataGainer, new FileSystem(), filePath, reportLogger);
        }

        /// <summary>
        /// Reads data from a csv file.
        /// </summary>
        /// <param name="dataGainer">The object to read data out of.</param>
        /// <param name="filePath">The path of file to read from.</param>
        /// <param name="reportLogger">Reporting Callback.</param>
        public static List<object> ReadFromCsv<T>(T dataGainer, IFileSystem fileSystem, string filePath, IReportLogger reportLogger = null) where T : ICSVAccess
        {
            try
            {
                using (Stream stream = fileSystem.FileStream.Create(filePath, FileMode.Open))
                using (TextReader reader = new StreamReader(stream))
                {
                    string line = null;
                    List<string[]> valuationsToRead = new List<string[]>();
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] words = line.Split(',');
                        valuationsToRead.Add(words);
                    }

                    return dataGainer.CreateDataFromCsv(valuationsToRead, reportLogger);
                }
            }
            catch (Exception ex)
            {
                _ = reportLogger?.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.Loading, ex.Message);
                return new List<object>();
            }
        }

        /// <summary>
        /// Exports data to a file in csv format.
        /// </summary>
        /// <param name="dataTypeToWrite">The object to read data out of.</param>
        /// <param name="filePath">The path of file to read from.</param>
        /// <param name="reportLogger">Reporting Callback.</param>
        public static void WriteToCSVFile<T>(T dataTypeToWrite, string filePath, IReportLogger reportLogger = null) where T : ICSVAccess
        {
            WriteToCSVFile(dataTypeToWrite, new FileSystem(), filePath, reportLogger);
        }

        /// <summary>
        /// Exports data to a file in csv format.
        /// </summary>
        /// <param name="dataTypeToWrite">The object to read data out of.</param>
        /// <param name="filePath">The path of file to read from.</param>
        /// <param name="reportLogger">Reporting Callback.</param>
        public static void WriteToCSVFile<T>(T dataTypeToWrite, IFileSystem fileSystem, string filePath, IReportLogger reportLogger = null) where T : ICSVAccess
        {
            try
            {
                using (Stream stream = fileSystem.FileStream.Create(filePath, FileMode.Create))
                using (TextWriter writer = new StreamWriter(stream))
                {
                    dataTypeToWrite.WriteDataToCsv(writer, reportLogger);
                }
            }
            catch (Exception ex)
            {
                _ = reportLogger?.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.Loading, ex.Message);
            }
        }
    }
}
