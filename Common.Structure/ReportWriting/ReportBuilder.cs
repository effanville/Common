using System.Collections.Generic;
using System.Text;

namespace Common.Structure.ReportWriting
{
    /// <summary>
    /// The settings for the report.
    /// </summary>
    public sealed class ReportSettings
    {
        /// <summary>
        /// Whether to use colour styling or not in the report.
        /// </summary>
        public bool UseColours
        {
            get;
            set;
        }

        /// <summary>
        /// Whether default styling should be applied.
        /// </summary>
        public bool UseDefaultStyle
        {
            get;
            set;
        } = false;

        /// <summary>
        /// Should the report include scripts.
        /// Note this is only relevant for html reports that may need to execute javascript
        /// scripts.
        /// </summary>
        public bool UseScripts
        {
            get;
            set;
        } = true;

        /// <summary>
        /// Construct an instance
        /// </summary>
        public ReportSettings()
        {
        }

        /// <summary>
        /// Construct an instance
        /// </summary>
        public ReportSettings(bool useColours, bool useDefaultStyle, bool useScripts)
        {
            UseColours = useColours;
            UseDefaultStyle = useDefaultStyle;
            UseScripts = useScripts;
        }

        /// <summary>
        /// Default settings for a <see cref="ReportBuilder"/>
        /// </summary>
        public static ReportSettings Default()
        {
            return new ReportSettings(useColours: true, useDefaultStyle: true, useScripts: true);
        }
    }

    /// <summary>
    /// Provides the mechanism to create a report.
    /// </summary>
    public sealed class ReportBuilder
    {
        private readonly ITextWriter fTextWriter;
        private readonly ITableWriter fTableWriter;
        private readonly IChartWriter fChartWriter;
        private readonly ReportSettings fSettings;

        private readonly StringBuilder fReport;

        /// <summary>
        /// Construct an instance of a <see cref="ReportBuilder"/>.
        /// </summary>
        /// <param name="docType"></param>
        /// <param name="settings"></param>
        public ReportBuilder(DocumentType docType, ReportSettings settings)
        {
            fTableWriter = TableWriterFactory.Create(docType);
            fTextWriter = TextWriterFactory.Create(docType, settings);
            fChartWriter = ChartWriterFactory.Create(docType);
            fSettings = settings;
            fReport = new StringBuilder();
        }

        /// <summary>
        /// Creates a generic header with default styles for a html page.
        /// </summary>
        /// <param name="pageTitle">A title to give the page</param>
        public ReportBuilder WriteHeader(string pageTitle)
        {
            fTextWriter.WriteHeader(fReport, pageTitle, fSettings.UseColours);
            return this;
        }

        /// <summary>
        /// Writes a title line to the report.
        /// </summary>
        /// <param name="title">The title string to write.</param>
        /// <param name="tag">The specific <see cref="DocumentElement"/> to use in this title.</param>
        public ReportBuilder WriteTitle(string title, DocumentElement tag)
        {
            fTextWriter.WriteTitle(fReport, title, tag);
            return this;
        }

        /// <summary>
        /// Writes a paragraph to the report.
        /// </summary>
        /// <param name="sentences">The sentence to write in this paragraph.</param>
        /// <param name="tag">The <see cref="DocumentElement"/> to use.</param>
        public ReportBuilder WriteParagraph(string[] sentences, DocumentElement tag = DocumentElement.p)
        {
            fTextWriter.WriteParagraph(fReport, sentences, tag);
            return this;
        }

        /// <summary>
        /// Adds a generic footer to the document.
        /// </summary>
        public ReportBuilder WriteFooter()
        {
            fTextWriter.WriteFooter(fReport);
            return this;
        }

        /// <summary>
        /// Writes an enumerable to a table, where the columns are all the properties of the type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of object to write out.</typeparam>
        /// <param name="values">The values to write into the table. The property names for <typeparamref name="T"/> give the header values.</param>
        /// <param name="headerFirstColumn">Whether first column should be header style or not.</param>
        public ReportBuilder WriteTableFromEnumerable<T>(IEnumerable<IEnumerable<T>> values, bool headerFirstColumn)
        {
            fTableWriter.WriteTable(fReport, values, headerFirstColumn);
            return this;
        }

        /// <summary>
        /// Writes an enumerable to a table, where the columns are all the properties of the type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of object to write out.</typeparam>
        /// <param name="values">The values to write into the table. The property names for <typeparamref name="T"/> give the header values.</param>
        /// <param name="headerFirstColumn">Whether first column should be header style or not.</param>
        public ReportBuilder WriteTable<T>(IEnumerable<T> values, bool headerFirstColumn)
        {
            fTableWriter.WriteTable(fReport, values, headerFirstColumn);
            return this;
        }

        /// <summary>
        /// Writes an enumerable to a table, where the column headers are specified./>.
        /// </summary>
        /// <typeparam name="T">The type of object to write out.</typeparam>
        /// <param name="headerValues">The values to write into the table header.</param>
        /// <param name="rowValues">The values to write for the table data. Each item is a row, and each row is a list of values. The values use the ToString() method to output the value.</param>
        /// <param name="headerFirstColumn">Whether first column should be header style or not.</param>
        public ReportBuilder WriteTableFromEnumerable<T>(IEnumerable<string> headerValues, IEnumerable<IEnumerable<T>> rowValues, bool headerFirstColumn)
        {
            fTableWriter.WriteTable(fReport, headerValues, rowValues, headerFirstColumn);
            return this;
        }

        /// <summary>
        /// Writes a table to the file, given header values which are a subset of the property names of the type <typeparamref name="T"/>.
        /// <para> If the value in rowValues is null, then this writes a break line into the table.</para>
        /// </summary>
        /// <typeparam name="T">The object to write the properties out of.</typeparam>
        /// <param name="headerValues">The values to write out.</param>
        /// <param name="rowValues">The values to place in successive rows.</param>
        /// <param name="headerFirstColumn">Whether first column should be header style or not.</param>
        public ReportBuilder WriteTable<T>(IEnumerable<string> headerValues, IEnumerable<T> rowValues, bool headerFirstColumn)
        {
            fTableWriter.WriteTable(fReport, headerValues, rowValues, headerFirstColumn);
            return this;
        }


        /// <summary>
        /// Writes a line chart into the report.
        /// </summary>
        public ReportBuilder WriteLineChart(
            string name,
            string title,
            IReadOnlyList<string> labels,
            IReadOnlyList<string> xValues,
            IReadOnlyList<IReadOnlyList<string>> yValues,
            bool xAxisIsTime)
        {
            fChartWriter.WriteLineChart(fReport, name, title, labels, xValues, yValues, xAxisIsTime);
            return this;
        }

        /// <summary>
        /// Writes a bar chart into the report.
        /// </summary>
        public ReportBuilder WriteBarChart(
            string name,
            string title,
            string dataLabel,
            IReadOnlyList<string> labels,
            IReadOnlyList<string> yValues)
        {
            fChartWriter.WriteBarChart(fReport, name, title, dataLabel, labels, yValues);
            return this;
        }

        /// <summary>
        /// Writes a bar chart into the report.
        /// </summary>
        public ReportBuilder WritePieChart(
            string name,
            string title,
            string dataLabel,
            IReadOnlyList<string> labels,
            IReadOnlyList<string> yValues)
        {
            fChartWriter.WritePieChart(fReport, name, title, dataLabel, labels, yValues);
            return this;
        }

        /// <summary>
        /// Add the contents of a <see cref="string"/> to the end of the report.
        /// </summary>
        public ReportBuilder Append(string value)
        {
            _ = fReport.Append(value);
            return this;
        }

        /// <summary>
        /// Add the contents of the other <see cref="ReportBuilder"/> to the end of the report.
        /// </summary>
        public ReportBuilder Append(ReportBuilder other)
        {
            _ = fReport.Append(other.GetReport());
            return this;
        }

        /// <summary>
        /// Add the contents of the other <see cref="StringBuilder"/> to the end of the report.
        /// </summary>
        public ReportBuilder Append(StringBuilder other)
        {
            _ = fReport.Append(other);
            return this;
        }

        /// <summary>
        /// Add a line into the report.
        /// </summary>
        public ReportBuilder AppendLine()
        {
            _ = fReport.AppendLine();
            return this;
        }

        /// <summary>
        /// Returns a string builder containing the report.
        /// </summary>
        /// <returns></returns>
        public StringBuilder GetReport()
        {
            return fReport;
        }

        /// <summary>
        /// Returns the report as a string.
        /// </summary>
        public override string ToString()
        {
            return fReport.ToString();
        }

        /// <summary>
        /// Clears the report in the <see cref="ReportBuilder"/>.
        /// </summary>
        public ReportBuilder Clear()
        {
            _ = fReport.Clear();
            return this;
        }
    }
}
