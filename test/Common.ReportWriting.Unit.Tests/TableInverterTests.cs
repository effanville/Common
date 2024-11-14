using System.Collections.Generic;

using Effanville.Common.ReportWriting.Documents;

using NUnit.Framework;

namespace Effanville.Common.ReportWriting.Unit.Tests
{
    [TestFixture]
    internal class TableInverterTests
    {
        private static string enl = TestConstants.EnvNewLine;

        private static IEnumerable<TestCaseData> TestCases()
        {
            yield return new TestCaseData(
                DocumentType.Md,
                $"| Row1 | Info | More Stuff |{enl}| ---- | ---- | ---------- |{enl}| Row1 | Info | More Stuff |{enl}",
                new List<string>() { "Row1", "Info", "More Stuff" },
                new List<List<string>> { new List<string>() { "Row1", "Info", "More Stuff" } });
            yield return new TestCaseData(
                DocumentType.Md,
                $"| Row1        | Info | More Stuff |{enl}| ---- | ---- | ---------- |{enl}| Row1 | Info | More Stuff |{enl}| Row2 | thing   | More thing |{enl}",
                new List<string>() { "Row1", "Info", "More Stuff" },
                new List<List<string>> { new List<string>() { "Row1", "Info", "More Stuff" }, new List<string>() { "Row2", "thing", "More thing" } });
            yield return new TestCaseData(
                DocumentType.Md,
                $"| Row1        | | More Stuff |{enl}| ---- | ---- | ---------- |{enl}| Row1 | Info | More Stuff |{enl}| Row2 | thing   | More thing |{enl}",
                new List<string>() { "Row1", "", "More Stuff" },
                new List<List<string>> { new List<string>() { "Row1", "Info", "More Stuff" }, new List<string>() { "Row2", "thing", "More thing" } });
            yield return new TestCaseData(
                DocumentType.Md,
                $"| Row1        | Info | More Stuff |{enl}| ---- | ---- | ---------- |{enl}| Row1 | | More Stuff |{enl}| | thing   | More thing |{enl}",
                new List<string>() { "Row1", "Info", "More Stuff" },
                new List<List<string>> { new List<string>() { "Row1", "", "More Stuff" }, new List<string>() { "", "thing", "More thing" } });
            yield return new TestCaseData(
                DocumentType.Md,
                $"| Row1        | Info | More Stuff |{enl}| ---- | ---- | ---------- |{enl}| __Row1__ | | More Stuff |{enl}| __thing__|   | More thing |{enl}",
                new List<string>() { "Row1", "Info", "More Stuff" },
                new List<List<string>> { new List<string>() { "Row1", "", "More Stuff" }, new List<string>() { "thing", "", "More thing" } });
            yield return new TestCaseData(
                DocumentType.Html,
                $"<table>{enl}<thead><tr>{enl}<th scope=\"col\">Row1</th><th>Info</th><th>More Stuff</th>{enl}</tr></thead>{enl}<tbody>{enl}<tr>{enl}<td>Row1</td><td>Info</td><td>More Stuff</td>{enl}</tr>{enl}</tbody>{enl}</table>{enl}",
                new List<string>() { "Row1", "Info", "More Stuff" },
                new List<List<string>> { new List<string>() { "Row1", "Info", "More Stuff" } });
            yield return new TestCaseData(
                DocumentType.Html,
                @"<table>
<thead><tr>
<th scope=""col""></th><th></th>
</tr></thead>
<tbody>
<tr>
<th scope=""row"">Byes</th><td>2</td>
</tr>
<tr>
<th scope=""row"">Leg Byes</th><td>5</td>
</tr>
<tr>
<th scope=""row"">Wides</th><td>7</td>
</tr>
<tr>
<th scope=""row"">No Balls</th><td>9</td>
</tr>
<tr>
<th scope=""row"">Penalties</th><td>0</td>
</tr>
<tr>
<th scope=""row"">Total Extras</th><td>12</td>
</tr>
</tbody>
</table>",
                new List<string> { "", "" },
                new List<List<string>>()
                {
                    new List<string>{ "Byes", "2"},
                    new List<string>{ "Leg Byes", "5"},
                    new List<string>{ "Wides", "7"},
                    new List<string>{ "No Balls", "9"},
                    new List<string>{ "Penalties", "0"},
                    new List<string>{ "Total Extras", "12"}
                });
            yield return new TestCaseData(
                DocumentType.Html,
                @"<table>
<thead><tr>
<th scope=""col""></th><th>Batsman</th><th>How Out</th><th>Bowler</th><th>Total</th>
</tr></thead>
<tbody>
<tr>
<td>1</td><td>Some Bat</td><td>Bowled </td><td>Some Bowler </td><td>10</td>
</tr>
<tr>
<td>2</td><td>Some OtherBat</td><td>Caught SomeFielder </td><td> SomeOther Bowler</td><td>61</td>
</tr>
<tr>
<td></td><td></td><td></td><td>Batting Total</td><td>154</td>
</tr>
<tr>
<td></td><td></td><td></td><td>Total Extras </td><td>27</td>
</tr>
<tr>
<td></td><td></td><td></td><td>Total</td><td>181</td>
</tr>
</tbody>
</table>",
                new List<string> { "", "Batsman", "How Out", "Bowler", "Total" },
                new List<List<string>>
                {
                    new List<string>{ "1", "Some Bat", "Bowled", "Some Bowler", "10"},
                    new List<string>{ "2", "Some OtherBat", "Caught SomeFielder", "SomeOther Bowler", "61"},
                    new List<string>{ "", "", "", "Batting Total", "154" },
                    new List<string>{ "", "", "", "Total Extras", "27" },
                    new List<string>{ "", "", "", "Total", "181" }
                });
            yield return new TestCaseData(
                DocumentType.Html,
                @"<table>
<thead><tr>
<th scope=""col"">Bowler</th><th>Wides</th><th>NB</th><th>Overs</th><th>Mdns</th><th>Runs</th><th>Wkts</th><th>Avg</th>
</tr></thead>
<tbody>
<tr>
<td>Some Bowler</td><td>0</td><td>0</td><td>7.0</td><td>4</td><td>23</td><td>1</td><td>23</td>
</tr>
<tr>
<td>Also Bowler</td><td>0</td><td>0</td><td>7.0</td><td>1</td><td>26</td><td>1</td><td>26</td>
</tr>
<tr>
<td>Bowling Totals</td><td>0</td><td>0</td><td>40.0</td><td>6</td><td>175</td><td>7</td><td>25</td>
</tr>
</tbody>
</table>",
                new List<string> { "Bowler", "Wides", "NB", "Overs", "Mdns", "Runs", "Wkts", "Avg" },
                new List<List<string>>
                {
                    new List<string>{ "Some Bowler", "0", "0", "7.0", "4", "23", "1", "23" },
                    new List<string>{ "Also Bowler", "0", "0", "7.0", "1", "26", "1", "26" },
                    new List<string>{ "Bowling Totals", "0", "0", "40.0", "6", "175", "7", "25" },
                });
            yield return new TestCaseData(
                DocumentType.Md,
                $"|||{enl} | -- | -- | {enl}|Byes|0|{enl}|Leg Byes|4|{enl}",
                 new List<string>(),
                 new List<List<string>>
                 {
                     new List<string>{ "Byes", "0"},
                     new List<string>{ "Leg Byes", "4"}
                 });
        }

        [TestCaseSource(nameof(TestCases))]
        public void InvertTableTests(DocumentType docType, string table, List<string> expectedHeaders, List<List<string>> expectedRows)
        {
            var actualTable = TableInverter.InvertTable(docType, table);
            CollectionAssert.AreEqual(expectedHeaders, actualTable.TableHeaders);
            CollectionAssert.AreEqual(expectedRows, actualTable.TableRows);
        }
    }
}
