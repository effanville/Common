using System.Collections.Generic;
using System.Text;

namespace Common.Structure.ReportWriting
{
    /// <summary>
    /// Contains static methods for writing charts in any document type.
    /// </summary>
    public static class ChartWriting
    {
        /// <summary>
        /// Writes a line chart into the string builder.
        /// </summary>
        public static void WriteLineChart(
            StringBuilder sb,
            DocumentType documentType,
            string name,
            string title,
            IReadOnlyList<string> labels,
            IReadOnlyList<string> xValues,
            IReadOnlyList<IReadOnlyList<string>> yValues,
            bool xAxisIsTime)
        {
            IChartWriter chartWriter = ChartWriterFactory.Create(documentType);
            chartWriter.WriteLineChart(sb, name, title, labels, xValues, yValues, xAxisIsTime);
        }

        /// <summary>
        /// Writes a bar chart into the string builder.
        /// </summary>
        public static void WriteBarChart(
            StringBuilder sb,
            DocumentType documentType,
            string name,
            string title,
            string dataLabel,
            IReadOnlyList<string> labels,
            IReadOnlyList<string> yValues)
        {
            IChartWriter chartWriter = ChartWriterFactory.Create(documentType);
            chartWriter.WriteBarChart(sb, name, title, dataLabel, labels, yValues);
        }

        /// <summary>
        /// Writes a bar chart into the string builder.
        /// </summary>
        public static void WritePieChart(
            StringBuilder sb,
            DocumentType documentType,
            string name,
            string title,
            string dataLabel,
            IReadOnlyList<string> labels,
            IReadOnlyList<string> yValues)
        {
            IChartWriter chartWriter = ChartWriterFactory.Create(documentType);
            chartWriter.WritePieChart(sb, name, title, dataLabel, labels, yValues);
        }
    }
}
