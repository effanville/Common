using System;
using System.Collections.Generic;

using Effanville.Common.Structure.DataStructures.Numeric;

using NUnit.Framework;

namespace Effanville.Common.Structure.Tests.DataStructures.Numeric
{
    [TestFixture]
    public sealed class TimeListValuesTests
    {
        private static IEnumerable<TestCaseData> ValueTestData()
        {
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.EmptyListKey),
                new DateTime(2018, 1, 1),
                true,
                default(DateTime),
                0.0).SetName($"{nameof(ValueTest)}-EmptyTimeList1");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.EmptyListKey),
                new DateTime(2022, 1, 1),
                true,
                default(DateTime),
                0.0).SetName($"{nameof(ValueTest)}-EmptyTimeList2");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.SingleEntryKey),
                new DateTime(2017, 1, 1),
                false,
                new DateTime(2018, 1, 1),
                1000.0).SetName($"{nameof(ValueTest)}-SingleEntryTimeList1");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.SingleEntryKey),
                new DateTime(2018, 1, 1),
                false,
                new DateTime(2018, 1, 1),
                1000.0).SetName($"{nameof(ValueTest)}-SingleEntryTimeList2");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.SingleEntryKey),
                new DateTime(2022, 1, 1),
                false,
                new DateTime(2018, 1, 1),
                1000.0).SetName($"{nameof(ValueTest)}-SingleEntryTimeList3");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey),
                new DateTime(2017, 1, 1),
                false,
                new DateTime(2018, 1, 1),
                1000.0).SetName($"{nameof(ValueTest)}-TwoEntryValues1");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey),
                new DateTime(2018, 1, 1),
                false,
                new DateTime(2018, 1, 1),
                1000.0).SetName($"{nameof(ValueTest)}-TwoEntryValues2");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey),
                new DateTime(2018, 3, 1),
                false,
                new DateTime(2018, 3, 1),
                1097.68).SetName($"{nameof(ValueTest)}-TwoEntryValues3");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey),
                new DateTime(2018, 6, 1),
                false,
                new DateTime(2018, 6, 1),
                1250.0).SetName($"{nameof(ValueTest)}-TwoEntryValues4");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey),
                new DateTime(2020, 1, 1),
                false,
                new DateTime(2018, 6, 1),
                1250.0).SetName($"{nameof(ValueTest)}-TwoEntryValues5");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.FourEntrySameDayKey),
                new DateTime(2019, 1, 1, 8, 2, 3),
                false,
                new DateTime(2019, 1, 1, 8, 2, 3),
                0.4136986301369863).SetName($"{nameof(ValueTest)}-FourEntrySameDay");
        }

        [TestCaseSource(nameof(ValueTestData))]
        public void ValueTest(TimeNumberList timelist, DateTime queryDate, bool isNull, DateTime expectedDate, double expectedValue)
        {
            var value = timelist.Value(queryDate);
            if (isNull)
            {
                Assert.That(value, Is.Null);
            }
            else
            {
                Assert.Multiple(() =>
                {
                    Assert.That(value, Is.Not.Null);
                    Assert.That(expectedValue - value.Value, Is.LessThan(1e-8));
                    Assert.That(value.Day, Is.EqualTo(expectedDate));
                });
            }
        }

        private static IEnumerable<TestCaseData> ValueZeroBeforeTestData()
        {
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.EmptyListKey),
                new DateTime(2018, 1, 1),
                true,
                default(DateTime),
                0.0).SetName($"{nameof(ValueZeroBeforeTest)}-EmptyTimeList1");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.EmptyListKey),
                new DateTime(2022, 1, 1),
                true,
                default(DateTime),
                0.0).SetName($"{nameof(ValueZeroBeforeTest)}-EmptyTimeList2");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.SingleEntryKey),
                new DateTime(2017, 1, 1),
                false,
                new DateTime(2017, 1, 1),
                0.0).SetName($"{nameof(ValueZeroBeforeTest)}-SingleEntryTimeList1");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.SingleEntryKey),
                new DateTime(2018, 1, 1),
                false,
                new DateTime(2018, 1, 1),
                1000.0).SetName($"{nameof(ValueZeroBeforeTest)}-SingleEntryTimeList2");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.SingleEntryKey),
                new DateTime(2022, 1, 1),
                false,
                new DateTime(2018, 1, 1),
                1000.0).SetName($"{nameof(ValueZeroBeforeTest)}-SingleEntryTimeList3");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey),
                new DateTime(2017, 1, 1),
                false,
                new DateTime(2017, 1, 1),
                0.0).SetName($"{nameof(ValueZeroBeforeTest)}-TwoEntryValues1");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey),
                new DateTime(2018, 1, 1),
                false,
                new DateTime(2018, 1, 1),
                1000.0).SetName($"{nameof(ValueZeroBeforeTest)}-TwoEntryValues2");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey),
                new DateTime(2018, 3, 1),
                false,
                new DateTime(2018, 3, 1),
                1097.68).SetName($"{nameof(ValueZeroBeforeTest)}-TwoEntryValues3");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey),
                new DateTime(2018, 6, 1),
                false,
                new DateTime(2018, 6, 1),
                1250.0).SetName($"{nameof(ValueZeroBeforeTest)}-TwoEntryValues4");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey),
                new DateTime(2020, 1, 1),
                false,
                new DateTime(2018, 6, 1),
                1250.0).SetName($"{nameof(ValueZeroBeforeTest)}-TwoEntryValues5");
        }

        [TestCaseSource(nameof(ValueZeroBeforeTestData))]
        public void ValueZeroBeforeTest(TimeNumberList timelist, DateTime queryDate, bool isNull, DateTime expectedDate, double expectedValue)
        {
            var value = timelist.ValueZeroBefore(queryDate);
            if (isNull)
            {
                Assert.That(value, Is.Null);
            }
            else
            {
                Assert.Multiple(() =>
                {
                    Assert.That(value, Is.Not.Null);
                    Assert.That(expectedValue - value.Value, Is.LessThan(1e-8), $"Expected {expectedValue} but was {value.Value}");
                    Assert.That(value.Day, Is.EqualTo(expectedDate));
                });
            }
        }

        private static IEnumerable<TestCaseData> ValueOnOrBeforeTestData()
        {
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.EmptyListKey),
                new DateTime(2018, 1, 1),
                true,
                default(DateTime),
                0.0).SetName($"{nameof(ValueOnOrBeforeTest)}-EmptyTimeList1");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.EmptyListKey),
                new DateTime(2022, 1, 1),
                true,
                default(DateTime),
                0.0).SetName($"{nameof(ValueOnOrBeforeTest)}-EmptyTimeList2");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.SingleEntryKey),
                new DateTime(2017, 1, 1),
                true,
                default(DateTime),
                0.0).SetName($"{nameof(ValueOnOrBeforeTest)}-SingleEntryTimeList1");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.SingleEntryKey),
                new DateTime(2018, 1, 1),
                false,
                new DateTime(2018, 1, 1),
                1000.0).SetName($"{nameof(ValueOnOrBeforeTest)}-SingleEntryTimeList2");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.SingleEntryKey),
                new DateTime(2022, 1, 1),
                false,
                new DateTime(2018, 1, 1),
                1000.0).SetName($"{nameof(ValueOnOrBeforeTest)}-SingleEntryTimeList3");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey),
                new DateTime(2017, 1, 1),
                true,
                new DateTime(2018, 1, 1),
                0.0).SetName($"{nameof(ValueOnOrBeforeTest)}-TwoEntryValues1");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey),
                new DateTime(2018, 1, 1),
                false,
                new DateTime(2018, 1, 1),
                1000.0).SetName($"{nameof(ValueOnOrBeforeTest)}-TwoEntryValues2");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey),
                new DateTime(2018, 3, 1),
                false,
                new DateTime(2018, 1, 1),
                1000.0).SetName($"{nameof(ValueOnOrBeforeTest)}-TwoEntryValues3");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey),
                new DateTime(2018, 6, 1),
                false,
                new DateTime(2018, 6, 1),
                250.0).SetName($"{nameof(ValueOnOrBeforeTest)}-TwoEntryValues4");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey),
                new DateTime(2020, 1, 1),
                false,
                new DateTime(2018, 6, 1),
                1250.0).SetName($"{nameof(ValueOnOrBeforeTest)}-TwoEntryValues5");
        }

        [TestCaseSource(nameof(ValueOnOrBeforeTestData))]
        public void ValueOnOrBeforeTest(TimeNumberList timelist, DateTime queryDate, bool isNull, DateTime expectedDate, double expectedValue)
        {
            var value = timelist.ValueOnOrBefore(queryDate);
            if (isNull)
            {
                Assert.That(value, Is.Null);
            }
            else
            {
                Assert.Multiple(() =>
                {
                    Assert.That(value, Is.Not.Null);
                    Assert.That(expectedValue - value.Value, Is.LessThan(1e-8), $"Expected {expectedValue} but was {value.Value}");
                    Assert.That(value.Day, Is.EqualTo(expectedDate));
                });
            }
        }

        private static IEnumerable<TestCaseData> ValueBeforeTestData()
        {
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.EmptyListKey),
                new DateTime(2018, 1, 1),
                true,
                default(DateTime),
                0.0).SetName($"{nameof(ValueBeforeTest)}-EmptyTimeList1");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.EmptyListKey),
                new DateTime(2022, 1, 1),
                true,
                default(DateTime),
                0.0).SetName($"{nameof(ValueBeforeTest)}-EmptyTimeList2");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.SingleEntryKey),
                new DateTime(2017, 1, 1),
                true,
                default(DateTime),
                0.0).SetName($"{nameof(ValueBeforeTest)}-SingleEntryTimeList1");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.SingleEntryKey),
                new DateTime(2018, 1, 1),
                true,
                new DateTime(2018, 1, 1),
                1000.0).SetName($"{nameof(ValueBeforeTest)}-SingleEntryTimeList2");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.SingleEntryKey),
                new DateTime(2022, 1, 1),
                false,
                new DateTime(2018, 1, 1),
                1000.0).SetName($"{nameof(ValueBeforeTest)}-SingleEntryTimeList3");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey),
                new DateTime(2017, 1, 1),
                true,
                new DateTime(2018, 1, 1),
                0.0).SetName($"{nameof(ValueBeforeTest)}-TwoEntryValues1");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey),
                new DateTime(2018, 1, 1),
                true,
                new DateTime(2018, 1, 1),
                1000.0).SetName($"{nameof(ValueBeforeTest)}-TwoEntryValues2");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey),
                new DateTime(2018, 3, 1),
                false,
                new DateTime(2018, 1, 1),
                1000.0).SetName($"{nameof(ValueBeforeTest)}-TwoEntryValues3");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey),
                new DateTime(2018, 6, 1),
                false,
                new DateTime(2018, 1, 1),
                250.0).SetName($"{nameof(ValueBeforeTest)}-TwoEntryValues4");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey),
                new DateTime(2020, 1, 1),
                false,
                new DateTime(2018, 6, 1),
                1250.0).SetName($"{nameof(ValueBeforeTest)}-TwoEntryValues5");
        }

        [TestCaseSource(nameof(ValueBeforeTestData))]
        public void ValueBeforeTest(TimeNumberList timelist, DateTime queryDate, bool isNull, DateTime expectedDate, double expectedValue)
        {
            var value = timelist.ValueBefore(queryDate);
            if (isNull)
            {
                Assert.That(value, Is.Null);
            }
            else
            {
                Assert.Multiple(() =>
                {
                    Assert.That(value, Is.Not.Null);
                    Assert.That(expectedValue - value.Value, Is.LessThan(1e-8), $"Expected {expectedValue} but was {value.Value}");
                    Assert.That(value.Day, Is.EqualTo(expectedDate));
                });
            }
        }


        private static IEnumerable<TestCaseData> ValueAfterTestData()
        {
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.EmptyListKey),
                new DateTime(2018, 1, 1),
                true,
                default(DateTime),
                0.0).SetName($"{nameof(ValueAfterTest)}-EmptyTimeList1");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.EmptyListKey),
                new DateTime(2022, 1, 1),
                true,
                default(DateTime),
                0.0).SetName($"{nameof(ValueAfterTest)}-EmptyTimeList2");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.SingleEntryKey),
                new DateTime(2017, 1, 1),
                false,
                new DateTime(2018, 1, 1),
                1000.0).SetName($"{nameof(ValueAfterTest)}-SingleEntryTimeList1");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.SingleEntryKey),
                new DateTime(2018, 1, 1),
                true,
                new DateTime(2018, 1, 1),
                1000.0).SetName($"{nameof(ValueAfterTest)}-SingleEntryTimeList2");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.SingleEntryKey),
                new DateTime(2022, 1, 1),
                true,
                default(DateTime),
                0.0).SetName($"{nameof(ValueAfterTest)}-SingleEntryTimeList3");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey),
                new DateTime(2017, 1, 1),
                false,
                new DateTime(2018, 1, 1),
                1000.0).SetName($"{nameof(ValueAfterTest)}-TwoEntryValues1");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey),
                new DateTime(2018, 1, 1),
                false,
                new DateTime(2018, 6, 1),
                1250.0).SetName($"{nameof(ValueAfterTest)}-TwoEntryValues2");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey),
                new DateTime(2018, 3, 1),
                false,
                new DateTime(2018, 6, 1),
                1000.0).SetName($"{nameof(ValueAfterTest)}-TwoEntryValues3");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey),
                new DateTime(2018, 6, 1),
                true,
                default(DateTime),
                0.0).SetName($"{nameof(ValueAfterTest)}-TwoEntryValues4");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey),
                new DateTime(2020, 1, 1),
                true,
                default(DateTime),
                0.0).SetName($"{nameof(ValueAfterTest)}-TwoEntryValues5");
        }

        [TestCaseSource(nameof(ValueAfterTestData))]
        public void ValueAfterTest(TimeNumberList timelist, DateTime queryDate, bool isNull, DateTime expectedDate, double expectedValue)
        {
            var value = timelist.ValueAfter(queryDate);
            if (isNull)
            {
                Assert.That(value, Is.Null);
            }
            else
            {
                Assert.Multiple(() =>
                {
                    Assert.That(value, Is.Not.Null);
                    Assert.That(expectedValue - value.Value, Is.LessThan(1e-8), $"Expected {expectedValue} but was {value.Value}");
                    Assert.That(value.Day, Is.EqualTo(expectedDate));
                });
            }
        }


        [TestCaseSource(nameof(ValuesSpecialFuncTestSource))]
        public void ValuesSpecialFuncTests(TimeNumberList timelist, DateTime queryDate, bool isNull, DateTime expectedDate, double expectedResult)
        {
            DailyNumeric interpolator(DailyNumeric earlier, DailyNumeric later, DateTime chosenDate) => new DailyNumeric(chosenDate, earlier.Value + (later.Value - earlier.Value) / (later.Day - earlier.Day).Days * (chosenDate - earlier.Day).Days);
            var actualValue = timelist.Value(queryDate, interpolator);
            if (isNull)
            {
                Assert.That(actualValue, Is.Null);
            }
            else
            {
                Assert.That(actualValue.Day, Is.EqualTo(expectedDate), $" date not correct");
                Assert.That(actualValue.Value, Is.EqualTo(expectedResult), $" value not correct");
            }
        }

        private static IEnumerable<TestCaseData> ValuesSpecialFuncTestSource()
        {
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.EmptyListKey),
                new DateTime(2018, 1, 1),
                true,
                new DateTime(2018, 1, 1),
                double.NaN).SetName($"{nameof(ValuesSpecialFuncTests)}-EmptyList");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.HundredEntryKey),
                new DateTime(2018, 1, 1),
                false,
                new DateTime(2018, 1, 1),
                298.79272727272718).SetName($"{nameof(ValuesSpecialFuncTests)}-HundredValues1");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.HundredEntryKey),
                new DateTime(2019, 3, 5),
                false,
                new DateTime(2019, 3, 5),
                393.73090909090888).SetName($"{nameof(ValuesSpecialFuncTests)}-HundredValues2");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.HundredEntryKey),
                new DateTime(2019, 5, 1),
                false,
                new DateTime(2019, 5, 1),
                406.37454545454523).SetName($"{nameof(ValuesSpecialFuncTests)}-HundredValues3");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.HundredEntryKey),
                new DateTime(2022, 1, 1),
                false,
                new DateTime(2022, 1, 1),
                622.86909090909103).SetName($"{nameof(ValuesSpecialFuncTests)}-HundredValues4");
        }
    }
}
