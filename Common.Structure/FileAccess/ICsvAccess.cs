using System.Collections.Generic;
using System.IO;

using Common.Structure.Reporting;

namespace Common.Structure.FileAccess
{
    /// <summary>
    /// Contains methods to obtain and export data from csv file
    /// </summary>
    public interface ICSVAccess
    {
        /// <summary>
        /// obtains a list of data from the csv file.
        /// </summary>
        List<object> CreateDataFromCsv(List<string[]> valuationsToRead, IReportLogger reportLogger = null);

        /// <summary>
        /// Exports data to a csv file.
        /// </summary>
        void WriteDataToCsv(TextWriter writer, IReportLogger reportLogger = null);
    }
}
