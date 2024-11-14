using System.Text;

using Effanville.Common.ReportWriting.Documents;

namespace Effanville.Common.ReportWriting
{
    /// <summary>
    /// Provides the mechanism to create a report.
    /// </summary>
    public sealed class ReportBuilder
    {
        private readonly ITextWriter _textWriter;
        private readonly ITableWriter _tableWriter;
        private readonly IChartWriter _chartWriter;
        private readonly DocumentWriterSettings _settings;

        private readonly StringBuilder _report;

        /// <summary>
        /// Construct an instance of a <see cref="ReportBuilder"/>.
        /// </summary>
        /// <param name="docType"></param>
        /// <param name="settings"></param>
        public ReportBuilder(DocumentWriterSettings settings,
            ITableWriter tableWriter,
            ITextWriter textWriter,
            IChartWriter chartWriter)
        {
            _settings = settings;
            _tableWriter = tableWriter;
            _textWriter = textWriter;
            _chartWriter = chartWriter;
            _report = new StringBuilder();
        }

        /// <summary>
        /// Creates a generic header with default styles for a html page.
        /// </summary>
        /// <param name="pageTitle">A title to give the page</param>
        public ReportBuilder WriteHeader(string pageTitle)
        {
            _textWriter.WriteHeader(_report, pageTitle, _settings.UseColours);
            return this;
        }

        /// <summary>
        /// Writes a title line to the report.
        /// </summary>
        /// <param name="title">The title string to write.</param>
        /// <param name="tag">The specific <see cref="DocumentElement"/> to use in this title.</param>
        public ReportBuilder WriteTitle(string title, DocumentElement tag)
        {
            _textWriter.WriteTitle(_report, title, tag);
            return this;
        }

        /// <summary>
        /// Writes a paragraph to the report.
        /// </summary>
        /// <param name="sentences">The sentence to write in this paragraph.</param>
        /// <param name="tag">The <see cref="DocumentElement"/> to use.</param>
        public ReportBuilder WriteParagraph(string[] sentences, DocumentElement tag = DocumentElement.p)
        {
            _textWriter.WriteParagraph(_report, sentences, tag);
            return this;
        }

        /// <summary>
        /// Adds a generic footer to the document.
        /// </summary>
        public ReportBuilder WriteFooter()
        {
            _textWriter.WriteFooter(_report);
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
            _tableWriter.WriteTable(_report, values, headerFirstColumn);
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
            _tableWriter.WriteTable(_report, values, headerFirstColumn);
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
            _tableWriter.WriteTable(_report, headerValues, rowValues, headerFirstColumn);
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
            _tableWriter.WriteTable(_report, headerValues, rowValues, headerFirstColumn);
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
            _chartWriter.WriteLineChart(_report, name, title, labels, xValues, yValues, xAxisIsTime);
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
            _chartWriter.WriteBarChart(_report, name, title, dataLabel, labels, yValues);
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
            _chartWriter.WritePieChart(_report, name, title, dataLabel, labels, yValues);
            return this;
        }

        /// <summary>
        /// Add the contents of a <see cref="string"/> to the end of the report.
        /// </summary>
        public ReportBuilder Append(string value)
        {
            _ = _report.Append(value);
            return this;
        }

        /// <summary>
        /// Add the contents of the other <see cref="ReportBuilder"/> to the end of the report.
        /// </summary>
        public ReportBuilder Append(ReportBuilder other)
        {
            _ = _report.Append(other.GetReport());
            return this;
        }

        /// <summary>
        /// Add the contents of the other <see cref="StringBuilder"/> to the end of the report.
        /// </summary>
        public ReportBuilder Append(StringBuilder other)
        {
            _ = _report.Append(other);
            return this;
        }

        /// <summary>
        /// Add a line into the report.
        /// </summary>
        public ReportBuilder AppendLine()
        {
            _ = _report.AppendLine();
            return this;
        }

        /// <summary>
        /// Returns a string builder containing the report.
        /// </summary>
        /// <returns></returns>
        public StringBuilder GetReport() => _report;

        /// <summary>
        /// Returns the report as a string.
        /// </summary>
        public override string ToString() => _report.ToString();

        /// <summary>
        /// Clears the report in the <see cref="ReportBuilder"/>.
        /// </summary>
        public ReportBuilder Clear()
        {
            _ = _report.Clear();
            return this;
        }
    }
}
