using Effanville.Common.ReportWriting.Documents;
using Effanville.Common.ReportWriting.Writers;

using System.Collections.Generic;
using NUnit.Framework;
using System.Text;

namespace Effanville.Common.ReportWriting.Unit.Tests;

public sealed class TestRow
{
    public string Value1 { get; set; }
    public int Value2 { get; set; }
}

[TestFixture]
internal class TableWriterExtensionTests
{

    public static IEnumerable<TestCaseData> CanWriteTableFromEnumerableOfEnumerableTestData()
    {
        yield return new TestCaseData(
            DocumentType.Md,
            new List<List<TestRow>>() { new List<TestRow>() },
            false,
            "").SetName($"{nameof(CanWriteTableFromEnumerableOfEnumerable)}-Md-Empty");
        yield return new TestCaseData(
            DocumentType.Html,
            new List<List<TestRow>>() { new List<TestRow>() },
            false,
            "").SetName($"{nameof(CanWriteTableFromEnumerableOfEnumerable)}-Html-Empty");
        yield return new TestCaseData(
            DocumentType.Csv,
            new List<List<TestRow>>() { new List<TestRow>() },
            false,
            "").SetName($"{nameof(CanWriteTableFromEnumerableOfEnumerable)}-Csv-Empty");
        yield return new TestCaseData(
            DocumentType.Md,
            new List<List<TestRow>>{new List<TestRow>()
            {
                new TestRow(){Value1="hi", Value2= 22 }
            }},
            false,
            "| Value1                                             | Value2 |\r\n| -------------------------------------------------- | ------ |\r\n| Effanville.Common.ReportWriting.Unit.Tests.TestRow |\r\n").SetName($"{nameof(CanWriteTableFromEnumerableOfEnumerable)}-Md-SingleRow");
        yield return new TestCaseData(
            DocumentType.Html,
            new List<List<TestRow>> { new List<TestRow>() { new TestRow() { Value1 = "hi", Value2 = 22 } } },
            false,
            "<table>\r\n<thead><tr>\r\n<th scope=\"col\">Value1</th><th>Value2</th>\r\n</tr></thead>\r\n<tbody>\r\n<tr>\r\n<td>Effanville.Common.ReportWriting.Unit.Tests.TestRow</td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n").SetName($"{nameof(CanWriteTableFromEnumerableOfEnumerable)}-Html-SingleRow");
        yield return new TestCaseData(
            DocumentType.Csv,
            new List<List<TestRow>>{new List<TestRow>() {
            new TestRow(){Value1="hi", Value2= 22 } } },
            false,
            "Value1,Value2\r\nEffanville.Common.ReportWriting.Unit.Tests.TestRow\r\n").SetName($"{nameof(CanWriteTableFromEnumerableOfEnumerable)}-Csv-SingleRow");
    }

    [TestCaseSource(nameof(CanWriteTableFromEnumerableOfEnumerableTestData))]
    public void CanWriteTableFromEnumerableOfEnumerable(
        DocumentType exportType,
        IEnumerable<IEnumerable<TestRow>> rows,
        bool headerFirstColumn,
        string expectedTable)
    {
        ITableWriter tableWriter = new TableWriterFactory().Create(exportType);

        StringBuilder stringBuilder = new StringBuilder();
        tableWriter.WriteTable(stringBuilder, rows, headerFirstColumn);

        string actualTable = stringBuilder.ToString();
        Assert.That(actualTable, Is.EqualTo(expectedTable));
    }
    public static IEnumerable<TestCaseData> CanWriteTableFromEnumerableTestData()
    {
        yield return new TestCaseData(
            DocumentType.Md,
            new List<TestRow>(),
            false,
            "").SetName($"{nameof(CanWriteTableFromEnumerable)}-Md-Empty");
        yield return new TestCaseData(
            DocumentType.Html,
            new List<TestRow>(),
            false,
            "").SetName($"{nameof(CanWriteTableFromEnumerable)}-Html-Empty");
        yield return new TestCaseData(
            DocumentType.Csv,
            new List<TestRow>(),
            false,
            "").SetName($"{nameof(CanWriteTableFromEnumerable)}-Csv-Empty");
        yield return new TestCaseData(
            DocumentType.Md,
            new List<TestRow>() {
             new TestRow(){Value1="hi", Value2= 22 } },
            false,
            "| Value1 | Value2 |\r\n| ------ | ------ |\r\n| hi     | 22     |\r\n").SetName($"{nameof(CanWriteTableFromEnumerable)}-Md-SingleRow");
        yield return new TestCaseData(
            DocumentType.Html,
            new List<TestRow>() { new TestRow() { Value1 = "hi", Value2 = 22 } },
            false,
            "<table>\r\n<thead><tr>\r\n<th scope=\"col\">Value1</th><th>Value2</th>\r\n</tr></thead>\r\n<tbody>\r\n<tr>\r\n<td>hi</td><td>22</td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n").SetName($"{nameof(CanWriteTableFromEnumerable)}-Html-SingleRow");
        yield return new TestCaseData(
            DocumentType.Csv,
            new List<TestRow>() {
            new TestRow(){Value1="hi", Value2= 22 } },
            false,
            "Value1,Value2\r\nhi,22\r\n").SetName($"{nameof(CanWriteTableFromEnumerable)}-Csv-SingleRow");
    }

    [TestCaseSource(nameof(CanWriteTableFromEnumerableTestData))]
    public void CanWriteTableFromEnumerable(
        DocumentType exportType,
        IEnumerable<TestRow> rows,
        bool headerFirstColumn,
        string expectedTable)
    {
        ITableWriter tableWriter = new TableWriterFactory().Create(exportType);

        StringBuilder stringBuilder = new StringBuilder();
        tableWriter.WriteTable(stringBuilder, rows, headerFirstColumn);

        string actualTable = stringBuilder.ToString();
        Assert.That(actualTable, Is.EqualTo(expectedTable));
    }

    public static IEnumerable<TestCaseData> CanWriteTableFromValuesTestData()
    {
        yield return new TestCaseData(
            DocumentType.Md,
            new List<string>(),
            new List<List<string>>(),
            false,
            "").SetName($"{nameof(CanWriteTableFromValues)}-Md-Empty");
        yield return new TestCaseData(
            DocumentType.Html,
            new List<string>(),
            new List<List<string>>(),
            false,
            "").SetName($"{nameof(CanWriteTableFromValues)}-Html-Empty");
        yield return new TestCaseData(
            DocumentType.Csv,
            new List<string>(),
            new List<List<string>>(),
            false,
            "").SetName($"{nameof(CanWriteTableFromValues)}-Csv-Empty");
        yield return new TestCaseData(
            DocumentType.Md,
            new List<string>() { "Hi" },
            new List<List<string>>(),
            false,
            "| Hi |\r\n| -- |\r\n").SetName($"{nameof(CanWriteTableFromValues)}-Md-Header");
        yield return new TestCaseData(
            DocumentType.Html,
            new List<string>() { "Hi" },
            new List<List<string>>(),
            false,
            "<table>\r\n<thead><tr>\r\n<th scope=\"col\">Hi</th>\r\n</tr></thead>\r\n<tbody>\r\n</tbody>\r\n</table>\r\n").SetName($"{nameof(CanWriteTableFromValues)}-Html-Header");
        yield return new TestCaseData(
            DocumentType.Csv,
            new List<string>() { "Hi" },
            new List<List<string>>(),
            false,
            "Hi\r\n").SetName($"{nameof(CanWriteTableFromValues)}-Csv-Header");
        yield return new TestCaseData(
            DocumentType.Md,
            new List<string>() { "Hi", "Hi2" },
            new List<List<string>>() {
            new List<string> { "Value1", "Value2" } },
            false,
            "| Hi     | Hi2    |\r\n| ------ | ------ |\r\n| Value1 | Value2 |\r\n").SetName($"{nameof(CanWriteTableFromValues)}-Md-SingleRow");
        yield return new TestCaseData(
            DocumentType.Html,
            new List<string>() { "Hi", "Hi2" },
            new List<List<string>>() {
            new List<string> { "Value1", "Value2" } },
            false,
            "<table>\r\n<thead><tr>\r\n<th scope=\"col\">Hi</th><th>Hi2</th>\r\n</tr></thead>\r\n<tbody>\r\n<tr>\r\n<td>Value1</td><td>Value2</td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n").SetName($"{nameof(CanWriteTableFromValues)}-Html-SingleRow");
        yield return new TestCaseData(
            DocumentType.Csv,
            new List<string>() { "Hi", "Hi2" },
            new List<List<string>>() {
            new List<string> { "Value1", "Value2" } },
            false,
            "Hi,Hi2\r\nValue1,Value2\r\n").SetName($"{nameof(CanWriteTableFromValues)}-Csv-SingleRow");
    }

    [TestCaseSource(nameof(CanWriteTableFromValuesTestData))]
    public void CanWriteTableFromValues(
        DocumentType exportType,
        List<string> headers,
        List<List<string>> rows,
        bool headerFirstColumn,
        string expectedTable)
    {
        ITableWriter tableWriter = new TableWriterFactory().Create(exportType);

        StringBuilder stringBuilder = new StringBuilder();
        tableWriter.WriteTable(stringBuilder, headers, rows, headerFirstColumn);

        string actualTable = stringBuilder.ToString();
        Assert.That(actualTable, Is.EqualTo(expectedTable));
    }

    public static IEnumerable<TestCaseData> CanWriteTableWithEnumerableValuesTestData()
    {
        yield return new TestCaseData(
            DocumentType.Md,
            new List<string>(),
            new List<List<string>>(),
            false,
            "|\r\n|\r\n").SetName($"{nameof(CanWriteTableWithEnumerableValues)}-Md-Empty");
        yield return new TestCaseData(
            DocumentType.Html,
            new List<string>(),
            new List<List<string>>(),
            false,
            "<table>\r\n<thead><tr>\r\n\r\n</tr></thead>\r\n<tbody>\r\n</tbody>\r\n</table>\r\n").SetName($"{nameof(CanWriteTableWithEnumerableValues)}-Html-Empty");
        yield return new TestCaseData(
            DocumentType.Csv,
            new List<string>(),
            new List<List<string>>(),
            false,
            "\r\n").SetName($"{nameof(CanWriteTableWithEnumerableValues)}-Csv-Empty");
        yield return new TestCaseData(
            DocumentType.Md,
            new List<string>() { "Hi" },
            new List<List<string>>(),
            false,
            "| Hi |\r\n| - |\r\n").SetName($"{nameof(CanWriteTableWithEnumerableValues)}-Md-Header");
        yield return new TestCaseData(
            DocumentType.Html,
            new List<string>() { "Hi" },
            new List<List<string>>(),
            false,
            "<table>\r\n<thead><tr>\r\n<th scope=\"col\">Hi</th>\r\n</tr></thead>\r\n<tbody>\r\n</tbody>\r\n</table>\r\n").SetName($"{nameof(CanWriteTableWithEnumerableValues)}-Html-Header");
        yield return new TestCaseData(
            DocumentType.Csv,
            new List<string>() { "Hi" },
            new List<List<string>>(),
            false,
            "Hi\r\n").SetName($"{nameof(CanWriteTableWithEnumerableValues)}-Csv-Header");
        yield return new TestCaseData(
            DocumentType.Md,
            new List<string>() { "Hi", "Hi2" },
            new List<List<string>>() {
            new List<string> { "Value1", "Value2" } },
            false,
            "| Hi | Hi2 |\r\n| - | - |\r\n| Value1 | Value2 |\r\n").SetName($"{nameof(CanWriteTableWithEnumerableValues)}-Md-SingleRow");
        yield return new TestCaseData(
            DocumentType.Html,
            new List<string>() { "Hi", "Hi2" },
            new List<List<string>>() {
            new List<string> { "Value1", "Value2" } },
            false,
            "<table>\r\n<thead><tr>\r\n<th scope=\"col\">Hi</th><th>Hi2</th>\r\n</tr></thead>\r\n<tbody>\r\n<tr>\r\n<td>Value1</td><td>Value2</td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n").SetName($"{nameof(CanWriteTableWithEnumerableValues)}-Html-SingleRow");
        yield return new TestCaseData(
            DocumentType.Csv,
            new List<string>() { "Hi", "Hi2" },
            new List<List<string>>() {
            new List<string> { "Value1", "Value2" } },
            false,
            "Hi,Hi2\r\nValue1,Value2\r\n").SetName($"{nameof(CanWriteTableWithEnumerableValues)}-Csv-SingleRow");
    }

    [TestCaseSource(nameof(CanWriteTableWithEnumerableValuesTestData))]
    public void CanWriteTableWithEnumerableValues(
        DocumentType exportType,
        IEnumerable<string> headers,
        IEnumerable<IEnumerable<string>> rows,
        bool headerFirstColumn,
        string expectedTable)
    {
        ITableWriter tableWriter = new TableWriterFactory().Create(exportType);

        StringBuilder stringBuilder = new StringBuilder();
        tableWriter.WriteTable(stringBuilder, headers, rows, headerFirstColumn);

        string actualTable = stringBuilder.ToString();
        Assert.That(actualTable, Is.EqualTo(expectedTable));
    }
}
