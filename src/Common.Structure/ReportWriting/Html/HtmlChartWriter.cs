using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Structure.ReportWriting.Html
{
    /// <summary>
    /// Helper methods for writing charts into Html files.
    /// </summary>
    internal sealed class HtmlChartWriter : IChartWriter
    {
        /// <summary>
        /// Writes a line chart into the string builder.
        /// </summary>
        public void WriteLineChart(StringBuilder sb, string name, string title, IReadOnlyList<string> labels, IReadOnlyList<string> xValues, IReadOnlyList<IReadOnlyList<string>> yValues, bool xAxisIsTime)
        {
            if (labels.Count != yValues.Count)
            {
                return;
            }

            Random rnd = new Random(12345);
            using (new WriteHtmlTag(sb, "div"))
            {
                _ = sb.AppendLine($"<canvas id=\"{name}\" width=\"500\" height=\"300\"></canvas>");

                using (new WriteHtmlTag(sb, "script"))
                {
                    _ = sb.AppendLine($"var charting = document.getElementById('{name}').getContext('2d');");
                    _ = sb.AppendLine("new Chart(charting, {");
                    _ = sb.AppendLine("\ttype: 'line',");
                    _ = sb.AppendLine("\tdata: {");
                    _ = sb.AppendLine($"\t\tlabels: [{string.Join(",", xValues.Select(val => $"'{val}'"))}],");
                    _ = sb.AppendLine("\t\tdatasets: [");
                    for (int i = 0; i < labels.Count; i++)
                    {
                        IReadOnlyList<string> data = yValues[i];
                        List<string> vallls = new List<string>();
                        for (int j = 0; j < data.Count; j++)
                        {
                            vallls.Add($"{{x: '{data[j]}'");
                        }
                        string label = labels[i];
                        string randomColourCode = $"#{rnd.Next(0x1000000):X6}";
                        _ = sb.AppendLine("\t\t\t{");
                        _ = sb.AppendLine($"\t\t\t\tdata: [{string.Join(",", data)}],");
                        _ = sb.AppendLine($"\t\t\t\tlabel: \"{label}\",");
                        _ = sb.AppendLine($"\t\t\t\tborderColor: \"{randomColourCode}\",");
                        _ = sb.AppendLine($"\t\t\t\tbackgroundColor: \"{randomColourCode}\",");
                        _ = sb.AppendLine($"\t\t\t\tfill: false,");
                        _ = sb.AppendLine("\t\t\t},");
                    }

                    _ = sb.AppendLine("\t\t]");
                    _ = sb.AppendLine("\t},");
                    _ = sb.AppendLine("\toptions: {");
                    _ = sb.AppendLine("\t\ttitle: {");
                    _ = sb.AppendLine("\t\t\tdisplay: true,");
                    _ = sb.AppendLine($"\t\t\ttext: '{title}',");
                    _ = sb.AppendLine("\t\t},");
                    if (xAxisIsTime)
                    {
                        _ = sb.AppendLine("\t\tscales: {");
                        _ = sb.AppendLine("\t\t\txAxes: [{");
                        _ = sb.AppendLine("\t\t\t\ttype: 'time',");
                        _ = sb.AppendLine("\t\t\t\ttime: {");
                        _ = sb.AppendLine("\t\t\t\t\tdisplayFormats: {");
                        _ = sb.AppendLine("\t\t\t\t\t\tyear: 'YYYY',");
                        _ = sb.AppendLine("\t\t\t\t\t\tquarter: 'MM-YYYY',");
                        _ = sb.AppendLine("\t\t\t\t\t\tmonth: 'MM-YYYY',");
                        _ = sb.AppendLine("\t\t\t\t\t\tweek: 'DD-MM-YYYY',");
                        _ = sb.AppendLine("\t\t\t\t\t\tday: 'DD-MM-YYYY',");
                        _ = sb.AppendLine("\t\t\t\t\t}");
                        _ = sb.AppendLine("\t\t\t\t},");
                        _ = sb.AppendLine("\t\t\t}]");
                        _ = sb.AppendLine("\t\t}");
                    }

                    _ = sb.AppendLine("\t},");
                    _ = sb.AppendLine("});");
                }
            }
        }

        /// <summary>
        /// Writes a bar chart into the string builder.
        /// </summary>
        public void WriteBarChart(StringBuilder sb, string name, string title, string dataLabel, IReadOnlyList<string> labels, IReadOnlyList<string> yValues)
        {
            if (labels.Count != yValues.Count)
            {
                return;
            }

            Random rnd = new Random(12345);
            using (new WriteHtmlTag(sb, "div"))
            {
                _ = sb.AppendLine($"<canvas id=\"{name}\" width=\"500\" height=\"300\"></canvas>");

                using (new WriteHtmlTag(sb, "script"))
                {
                    _ = sb.AppendLine($"var charting = document.getElementById('{name}').getContext('2d');");
                    _ = sb.AppendLine("new Chart(charting, {");
                    _ = sb.AppendLine("\ttype: 'bar',");
                    _ = sb.AppendLine("\tdata: {");
                    _ = sb.AppendLine($"\t\tlabels: [{string.Join(",", labels.Select(val => $"'{val}'"))}],");
                    _ = sb.AppendLine("\t\tdatasets: [");

                    List<string> colours = new List<string>();
                    int index = 0;
                    while (index < yValues.Count)
                    {
                        string randomColourCode = $"\"#{rnd.Next(0x1000000):X6}\"";
                        colours.Add(randomColourCode);
                        index++;
                    }
                    _ = sb.AppendLine("\t\t{");
                    _ = sb.AppendLine($"\t\t\tlabel: \"{dataLabel}\",");
                    _ = sb.AppendLine($"\t\t\tdata: [{string.Join(",", yValues)}],");
                    _ = sb.AppendLine($"\t\t\tborderColor: [{string.Join(",", colours)}],");
                    _ = sb.AppendLine($"\t\t\tbackgroundColor: [{string.Join(",", colours)}],");
                    _ = sb.AppendLine($"\t\t\tborderWidth: 1,");
                    _ = sb.AppendLine("\t\t}");

                    _ = sb.AppendLine("\t\t]");
                    _ = sb.AppendLine("\t},");
                    _ = sb.AppendLine("\toptions: {");
                    _ = sb.AppendLine("\t\ttitle: {");
                    _ = sb.AppendLine("\t\t\tdisplay: true,");
                    _ = sb.AppendLine($"\t\t\ttext: '{title}',");
                    _ = sb.AppendLine("\t\t}");
                    _ = sb.AppendLine("\t},");
                    _ = sb.AppendLine("});");
                }
            }
        }

        /// <summary>
        /// Writes a bar chart into the string builder.
        /// </summary>
        public void WritePieChart(StringBuilder sb, string name, string title, string dataLabel, IReadOnlyList<string> labels, IReadOnlyList<string> yValues)
        {
            if (labels.Count != yValues.Count)
            {
                return;
            }

            Random rnd = new Random(12345);
            using (new WriteHtmlTag(sb, "div"))
            {
                _ = sb.AppendLine($"<canvas id=\"{name}\" width=\"500\" height=\"300\"></canvas>");

                using (new WriteHtmlTag(sb, "script"))
                {
                    _ = sb.AppendLine($"var charting = document.getElementById('{name}').getContext('2d');");
                    _ = sb.AppendLine("new Chart(charting, {");
                    _ = sb.AppendLine("\ttype: 'pie',");
                    _ = sb.AppendLine("\tdata: {");
                    _ = sb.AppendLine($"\t\tlabels: [{string.Join(",", labels.Select(val => $"'{val}'"))}],");
                    _ = sb.AppendLine("\t\tdatasets: [");

                    List<string> colours = new List<string>();
                    int index = 0;
                    while (index < yValues.Count)
                    {
                        string randomColourCode = $"\"#{rnd.Next(0x1000000):X6}\"";
                        colours.Add(randomColourCode);
                        index++;
                    }
                    _ = sb.AppendLine("\t\t{");
                    _ = sb.AppendLine($"\t\t\tlabel: \"{dataLabel}\",");
                    _ = sb.AppendLine($"\t\t\tdata: [{string.Join(",", yValues)}],");
                    _ = sb.AppendLine($"\t\t\tborderColor: [{string.Join(",", colours)}],");
                    _ = sb.AppendLine($"\t\t\tbackgroundColor: [{string.Join(",", colours)}],");
                    _ = sb.AppendLine($"\t\t\tborderWidth: 1,");
                    _ = sb.AppendLine("\t\t}");

                    _ = sb.AppendLine("\t\t]");
                    _ = sb.AppendLine("\t},");
                    _ = sb.AppendLine("\toptions: {");
                    _ = sb.AppendLine("\t\ttitle: {");
                    _ = sb.AppendLine("\t\t\tdisplay: true,");
                    _ = sb.AppendLine($"\t\t\ttext: '{title}',");
                    _ = sb.AppendLine("\t\t}");
                    _ = sb.AppendLine("\t},");
                    _ = sb.AppendLine("});");
                }
            }
        }
    }
}
