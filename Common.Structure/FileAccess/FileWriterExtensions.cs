using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using Common.Structure.ReportWriting;

namespace Common.Structure.FileAccess
{
    /// <summary>
    /// Class containing default routines to write to any file type in <see cref="ExportType"/>.
    /// </summary>
    public static class FileWritingSupport
    {
        /// <summary>
        /// Writes a paragraph to the file.
        /// </summary>
        /// <param name="writer">The writer to use</param>
        /// <param name="exportType">The type of file to export to.</param>
        /// <param name="sentence">The sentence to export.</param>
        /// <param name="tag">The <see cref="HtmlTag"/> to use.</param>
        [Obsolete("should use TextWriting instead.")]
        public static void WriteParagraph(this StreamWriter writer, ExportType exportType, string[] sentence, HtmlTag tag = HtmlTag.p)
        {
            var sb = new StringBuilder();
            TextWriting.WriteParagraph(sb, exportType, sentence, tag);
            writer.Write(sb.ToString());
        }

        /// <summary>
        /// Writes a title line to the file.
        /// </summary>
        /// <param name="writer">The writer to use.</param>
        /// <param name="exportType">The type of file to export to.</param>
        /// <param name="title">The title string to write.</param>
        /// <param name="tag">The specific <see cref="HtmlTag"/> to use in this title.</param>

        [Obsolete("should use TextWriting instead.")]
        public static void WriteTitle(this StreamWriter writer, ExportType exportType, string title, HtmlTag tag = HtmlTag.h1)
        {
            var sb = new StringBuilder();
            TextWriting.WriteTitle(sb, exportType, title, tag);
            writer.Write(sb.ToString());
        }

        [Obsolete("should use TextWriting instead.")]
        public static void WriteTableFromEnumerable<T>(this StreamWriter writer, ExportType exportType, IEnumerable<IEnumerable<T>> values, bool headerFirstColumn)
        {
            var sb = new StringBuilder();
            TableWriting.WriteTableFromEnumerable(sb, exportType, values, headerFirstColumn);
            writer.Write(sb.ToString());
        }

        /// <summary>
        /// Writes an enumerable to a table, where the columns are all the properties of the type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of object to write out.</typeparam>
        /// <param name="writer">The writer to write with.</param>
        /// <param name="exportType">The type of file to export to.</param>
        /// <param name="values">The values to write into the table.</param>
        /// <param name="headerFirstColumn">Whether first column should be header style or not.</param>
        [Obsolete("should use TextWriting instead.")]
        public static void WriteTable<T>(this StreamWriter writer, ExportType exportType, IEnumerable<T> values, bool headerFirstColumn)
        {

            var sb = new StringBuilder();
            TableWriting.WriteTable(sb, exportType, values, headerFirstColumn);
            writer.Write(sb.ToString());
        }

        /// <summary>
        /// Writes an enumerable to a table, where the column headers are specified./>.
        /// </summary>
        /// <typeparam name="T">The type of object to write out.</typeparam>
        /// <param name="writer">The writer to write with.</param>
        /// <param name="exportType">The type of file to export to.</param>
        /// <param name="headerValues">The values to write into the table.</param>
        /// <param name="headerFirstColumn">Whether first column should be header style or not.</param>
        [Obsolete("should use TextWriting instead.")]
        public static void WriteTableFromEnumerable<T>(this StreamWriter writer, ExportType exportType, IEnumerable<string> headerValues, IEnumerable<IEnumerable<T>> rowValues, bool headerFirstColumn)
        {
            var sb = new StringBuilder();
            TableWriting.WriteTableFromEnumerable(sb, exportType, headerValues, rowValues, headerFirstColumn);
            writer.Write(sb.ToString());
        }

        /// <summary>
        /// Writes a table to the file, given header values which are a subset of the property names of the type <typeparamref name="T"/>.
        /// <para> If the value in rowValues is null, then this writes a break line into the table.</para>
        /// </summary>
        /// <typeparam name="T">The object to write the properties out of.</typeparam>
        /// <param name="writer">The writer to write with.</param>
        /// <param name="exportType">The type of file to write to.</param>
        /// <param name="headerValues">The values to write out.</param>
        /// <param name="rowValues">The values to place in successive rows.</param>
        /// <param name="headerFirstColumn">Whether first column should be header style or not.</param>
        [Obsolete("should use TextWriting instead.")]
        public static void WriteTable<T>(this StreamWriter writer, ExportType exportType, IEnumerable<string> headerValues, IEnumerable<T> rowValues, bool headerFirstColumn)
        {
            var sb = new StringBuilder();
            TableWriting.WriteTable(sb, exportType, headerValues, rowValues, headerFirstColumn);
            writer.Write(sb.ToString());
        }

        /// <summary>
        /// Writes the header of a table
        /// </summary>
        /// <param name="writer">The StreamWriter to use</param>
        /// <param name="exportType">The type of file to export to</param>
        /// <param name="valuesToWrite">The values to use for the header names.</param>
        [Obsolete("should use TextWriting instead.")]
        public static void WriteTableHeader(this StreamWriter writer, ExportType exportType, IEnumerable<string> valuesToWrite)
        {
            var sb = new StringBuilder();
            TableWriting.WriteTableHeader(sb, exportType, valuesToWrite);
            writer.Write(sb.ToString());
        }

        /// <summary>
        /// Writes a row of a table from an enumerable of string values.
        /// </summary>
        /// <param name="writer">The StreamWriter to use</param>
        /// <param name="exportType">The type of file to export to</param>
        /// <param name="valuesToWrite">The values to use for the header names.</param>
        /// <param name="headerFirstColumn">Whether first column should be header style or not.</param>
        [Obsolete("should use TextWriting instead.")]
        public static void WriteTableRow(this StreamWriter writer, ExportType exportType, IEnumerable<string> valuesToWrite, bool headerFirstColumn)
        {
            var sb = new StringBuilder();
            TableWriting.WriteTableRow(sb, exportType, valuesToWrite, headerFirstColumn);
            writer.Write(sb.ToString());
        }

        /// <summary>
        /// Creates a generic header with default styles for a html page.
        /// </summary>
        /// <param name="writer">The writer to write the page with</param>
        /// <param name="title">A title to give the page</param>
        /// <param name="useColours">Whether to use colour styling or not.</param>
        [Obsolete("should use TextWriting instead.")]
        public static void CreateHTMLHeader(this StreamWriter writer, string title, bool useColours)
        {

            var sb = new StringBuilder();
            TextWriting.CreateHTMLHeader(sb, title, useColours);
            writer.Write(sb.ToString());
        }

        /// <summary>
        /// Creates a generic footer for a html page.
        /// </summary>
        /// <param name="writer">The writer to write the page with</param>
        [Obsolete("should use TextWriting instead.")]
        public static void CreateHTMLFooter(this StreamWriter writer)
        {
            var sb = new StringBuilder();
            TextWriting.CreateHTMLFooter(sb);
            writer.Write(sb.ToString());
        }
    }
}
