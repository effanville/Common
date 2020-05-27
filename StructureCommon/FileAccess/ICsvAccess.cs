using System.Collections.Generic;
using System.IO;
using StructureCommon.Reporting;

namespace StructureCommon.FileAccess
{
    public interface ICSVAccess
    {
        List<object> CreateDataFromCsv(List<string[]> valuationsToRead, IReportLogger reportLogger = null);

        void WriteDataToCsv(TextWriter writer, IReportLogger reportLogger = null);
    }
}
