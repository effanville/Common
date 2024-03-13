using System.Collections.Generic;
using System.Text;

namespace Effanville.Common.Structure.ReportWriting
{
    /// <summary>
    /// Contains methods for writing various different types of charts.
    /// </summary>
    public interface IChartWriter
    {
        /// <summary>
        /// Writes a line chart into the string builder.
        /// </summary>
        void WriteLineChart(
            StringBuilder sb,
            string name,
            string title,
            IReadOnlyList<string> labels,
            IReadOnlyList<string> xValues,
            IReadOnlyList<IReadOnlyList<string>> yValues,
            bool xAxisIsTime);

        /// <summary>
        /// Writes a bar chart into the string builder.
        /// </summary>
        void WriteBarChart(
            StringBuilder sb,
            string name,
            string title,
            string dataLabel,
            IReadOnlyList<string> labels,
            IReadOnlyList<string> yValues);

        /// <summary>
        /// Writes a bar chart into the string builder.
        /// </summary>
        void WritePieChart(
            StringBuilder sb,
            string name,
            string title,
            string dataLabel,
            IReadOnlyList<string> labels,
            IReadOnlyList<string> yValues);
    }
}
