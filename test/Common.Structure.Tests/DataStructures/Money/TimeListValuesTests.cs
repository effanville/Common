using System;
using System.Collections.Generic;

using Effanville.Common.Structure.DataStructures;

using NUnit.Framework;

namespace Effanville.Common.Structure.Tests.DataStructures.Money
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
                0.0m).SetName($"{nameof(ValueTest)}-EmptyTimeList1");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.EmptyListKey),
                new DateTime(2022, 1, 1),
                true,
                default(DateTime),
                0.0m).SetName($"{nameof(ValueTest)}-EmptyTimeList2");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.SingleEntryKey),
                new DateTime(2017, 1, 1),
                false,
                new DateTime(2018, 1, 1),
                1000.0m).SetName($"{nameof(ValueTest)}-SingleEntryTimeList1");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.SingleEntryKey),
                new DateTime(2018, 1, 1),
                false,
                new DateTime(2018, 1, 1),
                1000.0m).SetName($"{nameof(ValueTest)}-SingleEntryTimeList2");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.SingleEntryKey),
                new DateTime(2022, 1, 1),
                false,
                new DateTime(2018, 1, 1),
                1000.0m).SetName($"{nameof(ValueTest)}-SingleEntryTimeList3");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryZeroValuesKey),
                new DateTime(2017, 1, 1),
                false,
                new DateTime(2018, 1, 1),
                0.0m).SetName($"{nameof(ValueTest)}-TwoEntryZeroValues1");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryZeroValuesKey),
                new DateTime(2018, 1, 1),
                false,
                new DateTime(2018, 1, 1),
                0.0m).SetName($"{nameof(ValueTest)}-TwoEntryZeroValues2");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryZeroValuesKey),
                new DateTime(2018, 6, 1),
                false,
                new DateTime(2018, 6, 1),
                0.0m).SetName($"{nameof(ValueTest)}-TwoEntryZeroValues3");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryZeroValuesKey),
                new DateTime(2020, 1, 1),
                false,
                new DateTime(2019, 1, 1),
                0.0m).SetName($"{nameof(ValueTest)}-TwoEntryZeroValues4");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey),
                new DateTime(2017, 1, 1),
                false,
                new DateTime(2018, 1, 1),
                1000.0m).SetName($"{nameof(ValueTest)}-TwoEntryValues1");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey),
                new DateTime(2018, 1, 1),
                false,
                new DateTime(2018, 1, 1),
                1000.0m).SetName($"{nameof(ValueTest)}-TwoEntryValues2");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey),
                new DateTime(2018, 3, 1),
                false,
                new DateTime(2018, 3, 1),
                1097.68m).SetName($"{nameof(ValueTest)}-TwoEntryValues3");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey),
                new DateTime(2018, 6, 1),
                false,
                new DateTime(2018, 6, 1),
                1250.0m).SetName($"{nameof(ValueTest)}-TwoEntryValues4");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey),
                new DateTime(2020, 1, 1),
                false,
                new DateTime(2018, 6, 1),
                1250.0m).SetName($"{nameof(ValueTest)}-TwoEntryValues5");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey2),
                new DateTime(2011, 1, 1),
                false,
                new DateTime(2012, 8, 31),
                824.59m).SetName($"{nameof(ValueTest)}-TwoEntry2Values1");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey2),
                new DateTime(2012, 8, 31),
                false,
                new DateTime(2012, 8, 31),
                824.59m).SetName($"{nameof(ValueTest)}-TwoEntry2Values2");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey2),
                new DateTime(2018, 3, 1),
                false,
                new DateTime(2018, 3, 1),
                824.59303323m).SetName($"{nameof(ValueTest)}-TwoEntry2Values3");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey2),
                new DateTime(2019, 12, 1),
                false,
                new DateTime(2019, 12, 1),
                824.594m).SetName($"{nameof(ValueTest)}-TwoEntry2Values4");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey2),
                new DateTime(2020, 1, 1),
                false,
                new DateTime(2019, 12, 1),
                824.594m).SetName($"{nameof(ValueTest)}-TwoEntry2Values5");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.FourEntryKey2),
                new DateTime(2018, 6, 1),
                false,
                new DateTime(2018, 6, 1),
                0.4136986301369863m).SetName($"{nameof(ValueTest)}-FourEntryDifferentDate");
        }

        [TestCaseSource(nameof(ValueTestData))]
        public void ValueTest(TimeList timelist, DateTime queryDate, bool isNull, DateTime expectedDate, decimal expectedValue)
        {
            var value = timelist.Value(queryDate);
            if (isNull)
            {
                Assert.IsNull(value);
            }
            else
            {
                Assert.Multiple(() =>
                {
                    Assert.IsNotNull(value);
                    Assert.That(expectedValue - value.Value, Is.LessThan(1e-8));
                    Assert.AreEqual(expectedDate, value.Day);
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
                0.0m).SetName($"{nameof(ValueZeroBeforeTest)}-EmptyTimeList1");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.EmptyListKey),
                new DateTime(2022, 1, 1),
                true,
                default(DateTime),
                0.0m).SetName($"{nameof(ValueZeroBeforeTest)}-EmptyTimeList2");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.SingleEntryKey),
                new DateTime(2017, 1, 1),
                false,
                new DateTime(2017, 1, 1),
                0.0m).SetName($"{nameof(ValueZeroBeforeTest)}-SingleEntryTimeList1");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.SingleEntryKey),
                new DateTime(2018, 1, 1),
                false,
                new DateTime(2018, 1, 1),
                1000.0m).SetName($"{nameof(ValueZeroBeforeTest)}-SingleEntryTimeList2");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.SingleEntryKey),
                new DateTime(2022, 1, 1),
                false,
                new DateTime(2018, 1, 1),
                1000.0m).SetName($"{nameof(ValueZeroBeforeTest)}-SingleEntryTimeList3");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryZeroValuesKey),
                new DateTime(2017, 1, 1),
                false,
                new DateTime(2017, 1, 1),
                0.0m).SetName($"{nameof(ValueZeroBeforeTest)}-TwoEntryZeroValues1");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryZeroValuesKey),
                new DateTime(2018, 1, 1),
                false,
                new DateTime(2018, 1, 1),
                0.0m).SetName($"{nameof(ValueZeroBeforeTest)}-TwoEntryZeroValues2");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryZeroValuesKey),
                new DateTime(2018, 6, 1),
                false,
                new DateTime(2018, 6, 1),
                0.0m).SetName($"{nameof(ValueZeroBeforeTest)}-TwoEntryZeroValues3");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryZeroValuesKey),
                new DateTime(2020, 1, 1),
                false,
                new DateTime(2019, 1, 1),
                0.0m).SetName($"{nameof(ValueZeroBeforeTest)}-TwoEntryZeroValues4");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey),
                new DateTime(2017, 1, 1),
                false,
                new DateTime(2017, 1, 1),
                0.0m).SetName($"{nameof(ValueZeroBeforeTest)}-TwoEntryValues1");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey),
                new DateTime(2018, 1, 1),
                false,
                new DateTime(2018, 1, 1),
                1000.0m).SetName($"{nameof(ValueZeroBeforeTest)}-TwoEntryValues2");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey),
                new DateTime(2018, 3, 1),
                false,
                new DateTime(2018, 3, 1),
                1097.68m).SetName($"{nameof(ValueZeroBeforeTest)}-TwoEntryValues3");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey),
                new DateTime(2018, 6, 1),
                false,
                new DateTime(2018, 6, 1),
                1250.0m).SetName($"{nameof(ValueZeroBeforeTest)}-TwoEntryValues4");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey),
                new DateTime(2020, 1, 1),
                false,
                new DateTime(2018, 6, 1),
                1250.0m).SetName($"{nameof(ValueZeroBeforeTest)}-TwoEntryValues5");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey2),
                new DateTime(2011, 1, 1),
                false,
                new DateTime(2011, 1, 1),
                0.0m).SetName($"{nameof(ValueZeroBeforeTest)}-TwoEntry2Values1");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey2),
                new DateTime(2012, 8, 31),
                false,
                new DateTime(2012, 8, 31),
                824.59m).SetName($"{nameof(ValueZeroBeforeTest)}-TwoEntry2Values2");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey2),
                new DateTime(2018, 3, 1),
                false,
                new DateTime(2018, 3, 1),
                824.59303323m).SetName($"{nameof(ValueZeroBeforeTest)}-TwoEntry2Values3");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey2),
                new DateTime(2019, 12, 1),
                false,
                new DateTime(2019, 12, 1),
                824.594m).SetName($"{nameof(ValueZeroBeforeTest)}-TwoEntry2Values4");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey2),
                new DateTime(2020, 1, 1),
                false,
                new DateTime(2019, 12, 1),
                824.594m).SetName($"{nameof(ValueZeroBeforeTest)}-TwoEntry2Values5");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.FourEntryKey2),
                new DateTime(2018, 6, 1),
                false,
                new DateTime(2018, 6, 1),
                0.4136986301369863m).SetName($"{nameof(ValueZeroBeforeTest)}-FourEntryDifferentDate");
        }

        [TestCaseSource(nameof(ValueZeroBeforeTestData))]
        public void ValueZeroBeforeTest(TimeList timelist, DateTime queryDate, bool isNull, DateTime expectedDate, decimal expectedValue)
        {
            var value = timelist.ValueZeroBefore(queryDate);
            if (isNull)
            {
                Assert.IsNull(value);
            }
            else
            {
                Assert.Multiple(() =>
                {
                    Assert.IsNotNull(value);
                    Assert.That(expectedValue - value.Value, Is.LessThan(1e-8), $"Expected {expectedValue} but was {value.Value}");
                    Assert.AreEqual(expectedDate, value.Day);
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
                0.0m).SetName($"{nameof(ValueOnOrBeforeTest)}-EmptyTimeList1");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.EmptyListKey),
                new DateTime(2022, 1, 1),
                true,
                default(DateTime),
                0.0m).SetName($"{nameof(ValueOnOrBeforeTest)}-EmptyTimeList2");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.SingleEntryKey),
                new DateTime(2017, 1, 1),
                true,
                default(DateTime),
                0.0m).SetName($"{nameof(ValueOnOrBeforeTest)}-SingleEntryTimeList1");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.SingleEntryKey),
                new DateTime(2018, 1, 1),
                false,
                new DateTime(2018, 1, 1),
                1000.0m).SetName($"{nameof(ValueOnOrBeforeTest)}-SingleEntryTimeList2");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.SingleEntryKey),
                new DateTime(2022, 1, 1),
                false,
                new DateTime(2018, 1, 1),
                1000.0m).SetName($"{nameof(ValueOnOrBeforeTest)}-SingleEntryTimeList3");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryZeroValuesKey),
                new DateTime(2017, 1, 1),
                true,
                new DateTime(2017, 1, 1),
                0.0m).SetName($"{nameof(ValueOnOrBeforeTest)}-TwoEntryZeroValues1");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryZeroValuesKey),
                new DateTime(2018, 1, 1),
                false,
                new DateTime(2018, 1, 1),
                0.0m).SetName($"{nameof(ValueOnOrBeforeTest)}-TwoEntryZeroValues2");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryZeroValuesKey),
                new DateTime(2018, 6, 1),
                false,
                new DateTime(2018, 1, 1),
                0.0m).SetName($"{nameof(ValueOnOrBeforeTest)}-TwoEntryZeroValues3");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryZeroValuesKey),
                new DateTime(2020, 1, 1),
                false,
                new DateTime(2019, 1, 1),
                0.0m).SetName($"{nameof(ValueOnOrBeforeTest)}-TwoEntryZeroValues4");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey),
                new DateTime(2017, 1, 1),
                true,
                new DateTime(2018, 1, 1),
                0.0m).SetName($"{nameof(ValueOnOrBeforeTest)}-TwoEntryValues1");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey),
                new DateTime(2018, 1, 1),
                false,
                new DateTime(2018, 1, 1),
                1000.0m).SetName($"{nameof(ValueOnOrBeforeTest)}-TwoEntryValues2");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey),
                new DateTime(2018, 3, 1),
                false,
                new DateTime(2018, 1, 1),
                1000.0m).SetName($"{nameof(ValueOnOrBeforeTest)}-TwoEntryValues3");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey),
                new DateTime(2018, 6, 1),
                false,
                new DateTime(2018, 6, 1),
                250.0m).SetName($"{nameof(ValueOnOrBeforeTest)}-TwoEntryValues4");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey),
                new DateTime(2020, 1, 1),
                false,
                new DateTime(2018, 6, 1),
                1250.0m).SetName($"{nameof(ValueOnOrBeforeTest)}-TwoEntryValues5");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey2),
                new DateTime(2011, 1, 1),
                true,
                default(DateTime),
                0.0m).SetName($"{nameof(ValueOnOrBeforeTest)}-TwoEntry2Values1");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey2),
                new DateTime(2012, 8, 31),
                false,
                new DateTime(2012, 8, 31),
                824.59m).SetName($"{nameof(ValueOnOrBeforeTest)}-TwoEntry2Values2");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey2),
                new DateTime(2018, 3, 1),
                false,
                new DateTime(2012, 8, 31),
                824.0m).SetName($"{nameof(ValueOnOrBeforeTest)}-TwoEntry2Values3");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey2),
                new DateTime(2019, 12, 1),
                false,
                new DateTime(2019, 12, 1),
                824.59m).SetName($"{nameof(ValueOnOrBeforeTest)}-TwoEntry2Values4");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey2),
                new DateTime(2020, 1, 1),
                false,
                new DateTime(2019, 12, 1),
                824.594m).SetName($"{nameof(ValueOnOrBeforeTest)}-TwoEntry2Values5");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey2),
                new DateTime(2016, 6, 27),
                false,
                new DateTime(2012, 8, 31),
                824.59m).SetName($"{nameof(ValueOnOrBeforeTest)}-TwoEntry2Values6");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey2),
                new DateTime(2022, 6, 27),
                false,
                new DateTime(2019, 12, 01),
                824.594m).SetName($"{nameof(ValueOnOrBeforeTest)}-TwoEntry2Values7");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.FourEntryKey2),
                new DateTime(2018, 6, 1),
                false,
                new DateTime(2018, 1, 1),
                0.0m).SetName($"{nameof(ValueOnOrBeforeTest)}-FourEntryDifferentDate");
        }

        [TestCaseSource(nameof(ValueOnOrBeforeTestData))]
        public void ValueOnOrBeforeTest(TimeList timelist, DateTime queryDate, bool isNull, DateTime expectedDate, decimal expectedValue)
        {
            var value = timelist.ValueOnOrBefore(queryDate);
            if (isNull)
            {
                Assert.IsNull(value);
            }
            else
            {
                Assert.Multiple(() =>
                {
                    Assert.IsNotNull(value);
                    Assert.That(expectedValue - value.Value, Is.LessThan(1e-8), $"Expected {expectedValue} but was {value.Value}");
                    Assert.AreEqual(expectedDate, value.Day);
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
                0.0m).SetName($"{nameof(ValueBeforeTest)}-EmptyTimeList1");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.EmptyListKey),
                new DateTime(2022, 1, 1),
                true,
                default(DateTime),
                0.0m).SetName($"{nameof(ValueBeforeTest)}-EmptyTimeList2");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.SingleEntryKey),
                new DateTime(2017, 1, 1),
                true,
                default(DateTime),
                0.0m).SetName($"{nameof(ValueBeforeTest)}-SingleEntryTimeList1");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.SingleEntryKey),
                new DateTime(2018, 1, 1),
                true,
                new DateTime(2018, 1, 1),
                1000.0m).SetName($"{nameof(ValueBeforeTest)}-SingleEntryTimeList2");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.SingleEntryKey),
                new DateTime(2022, 1, 1),
                false,
                new DateTime(2018, 1, 1),
                1000.0m).SetName($"{nameof(ValueBeforeTest)}-SingleEntryTimeList3");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryZeroValuesKey),
                new DateTime(2017, 1, 1),
                true,
                new DateTime(2017, 1, 1),
                0.0m).SetName($"{nameof(ValueBeforeTest)}-TwoEntryZeroValues1");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryZeroValuesKey),
                new DateTime(2018, 1, 1),
                true,
                new DateTime(2018, 1, 1),
                0.0m).SetName($"{nameof(ValueBeforeTest)}-TwoEntryZeroValues2");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryZeroValuesKey),
                new DateTime(2018, 6, 1),
                false,
                new DateTime(2018, 1, 1),
                0.0m).SetName($"{nameof(ValueBeforeTest)}-TwoEntryZeroValues3");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryZeroValuesKey),
                new DateTime(2020, 1, 1),
                false,
                new DateTime(2019, 1, 1),
                0.0m).SetName($"{nameof(ValueBeforeTest)}-TwoEntryZeroValues4");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey),
                new DateTime(2017, 1, 1),
                true,
                new DateTime(2018, 1, 1),
                0.0m).SetName($"{nameof(ValueBeforeTest)}-TwoEntryValues1");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey),
                new DateTime(2018, 1, 1),
                true,
                new DateTime(2018, 1, 1),
                1000.0m).SetName($"{nameof(ValueBeforeTest)}-TwoEntryValues2");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey),
                new DateTime(2018, 3, 1),
                false,
                new DateTime(2018, 1, 1),
                1000.0m).SetName($"{nameof(ValueBeforeTest)}-TwoEntryValues3");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey),
                new DateTime(2018, 6, 1),
                false,
                new DateTime(2018, 1, 1),
                250.0m).SetName($"{nameof(ValueBeforeTest)}-TwoEntryValues4");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey),
                new DateTime(2020, 1, 1),
                false,
                new DateTime(2018, 6, 1),
                1250.0m).SetName($"{nameof(ValueBeforeTest)}-TwoEntryValues5");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey2),
                new DateTime(2011, 1, 1),
                true,
                default(DateTime),
                0.0m).SetName($"{nameof(ValueBeforeTest)}-TwoEntry2Values1");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey2),
                new DateTime(2012, 8, 31),
                true,
                default(DateTime),
                0.0m).SetName($"{nameof(ValueBeforeTest)}-TwoEntry2Values2");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey2),
                new DateTime(2018, 3, 1),
                false,
                new DateTime(2012, 8, 31),
                824.0m).SetName($"{nameof(ValueBeforeTest)}-TwoEntry2Values3");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey2),
                new DateTime(2019, 12, 1),
                false,
                new DateTime(2012, 8, 31),
                824.59m).SetName($"{nameof(ValueBeforeTest)}-TwoEntry2Values4");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey2),
                new DateTime(2020, 1, 1),
                false,
                new DateTime(2019, 12, 1),
                824.594m).SetName($"{nameof(ValueBeforeTest)}-TwoEntry2Values5");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey2),
                new DateTime(2016, 6, 27),
                false,
                new DateTime(2012, 8, 31),
                824.59m).SetName($"{nameof(ValueBeforeTest)}-TwoEntry2Values6");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey2),
                new DateTime(2022, 6, 27),
                false,
                new DateTime(2019, 12, 01),
                824.594m).SetName($"{nameof(ValueBeforeTest)}-TwoEntry2Values7");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.FourEntryKey2),
                new DateTime(2018, 6, 1),
                false,
                new DateTime(2018, 1, 1),
                0.0m).SetName($"{nameof(ValueBeforeTest)}-FourEntryDifferentDate");
        }

        [TestCaseSource(nameof(ValueBeforeTestData))]
        public void ValueBeforeTest(TimeList timelist, DateTime queryDate, bool isNull, DateTime expectedDate, decimal expectedValue)
        {
            var value = timelist.ValueBefore(queryDate);
            if (isNull)
            {
                Assert.IsNull(value);
            }
            else
            {
                Assert.Multiple(() =>
                {
                    Assert.IsNotNull(value);
                    Assert.That(expectedValue - value.Value, Is.LessThan(1e-8), $"Expected {expectedValue} but was {value.Value}");
                    Assert.AreEqual(expectedDate, value.Day);
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
                0.0m).SetName($"{nameof(ValueAfterTest)}-EmptyTimeList1");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.EmptyListKey),
                new DateTime(2022, 1, 1),
                true,
                default(DateTime),
                0.0m).SetName($"{nameof(ValueAfterTest)}-EmptyTimeList2");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.SingleEntryKey),
                new DateTime(2017, 1, 1),
                false,
                new DateTime(2018, 1, 1),
                1000.0m).SetName($"{nameof(ValueAfterTest)}-SingleEntryTimeList1");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.SingleEntryKey),
                new DateTime(2018, 1, 1),
                true,
                new DateTime(2018, 1, 1),
                1000.0m).SetName($"{nameof(ValueAfterTest)}-SingleEntryTimeList2");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.SingleEntryKey),
                new DateTime(2022, 1, 1),
                true,
                default(DateTime),
                0.0m).SetName($"{nameof(ValueAfterTest)}-SingleEntryTimeList3");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryZeroValuesKey),
                new DateTime(2017, 1, 1),
                false,
                new DateTime(2018, 1, 1),
                0.0m).SetName($"{nameof(ValueAfterTest)}-TwoEntryZeroValues1");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryZeroValuesKey),
                new DateTime(2018, 1, 1),
                false,
                new DateTime(2019, 1, 1),
                0.0m).SetName($"{nameof(ValueAfterTest)}-TwoEntryZeroValues2");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryZeroValuesKey),
                new DateTime(2018, 6, 1),
                false,
                new DateTime(2019, 1, 1),
                0.0m).SetName($"{nameof(ValueAfterTest)}-TwoEntryZeroValues3");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryZeroValuesKey),
                new DateTime(2020, 1, 1),
                true,
                default(DateTime),
                0.0m).SetName($"{nameof(ValueAfterTest)}-TwoEntryZeroValues4");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey),
                new DateTime(2017, 1, 1),
                false,
                new DateTime(2018, 1, 1),
                1000.0m).SetName($"{nameof(ValueAfterTest)}-TwoEntryValues1");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey),
                new DateTime(2018, 1, 1),
                false,
                new DateTime(2018, 6, 1),
                1250.0m).SetName($"{nameof(ValueAfterTest)}-TwoEntryValues2");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey),
                new DateTime(2018, 3, 1),
                false,
                new DateTime(2018, 6, 1),
                1000.0m).SetName($"{nameof(ValueAfterTest)}-TwoEntryValues3");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey),
                new DateTime(2018, 6, 1),
                true,
                default(DateTime),
                0.0m).SetName($"{nameof(ValueAfterTest)}-TwoEntryValues4");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey),
                new DateTime(2020, 1, 1),
                true,
                default(DateTime),
                0.0m).SetName($"{nameof(ValueAfterTest)}-TwoEntryValues5");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey2),
                new DateTime(2011, 1, 1),
                false,
                new DateTime(2012, 8, 31),
                824.59m).SetName($"{nameof(ValueAfterTest)}-TwoEntry2Values1");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey2),
                new DateTime(2012, 8, 31),
                false,
                new DateTime(2019, 12, 1),
                824.594m).SetName($"{nameof(ValueAfterTest)}-TwoEntry2Values2");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey2),
                new DateTime(2018, 3, 1),
                false,
                new DateTime(2019, 12, 1),
                824.0m).SetName($"{nameof(ValueAfterTest)}-TwoEntry2Values3");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey2),
                new DateTime(2019, 12, 1),
                true,
                new DateTime(2012, 8, 31),
                824.59m).SetName($"{nameof(ValueAfterTest)}-TwoEntry2Values4");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey2),
                new DateTime(2020, 1, 1),
                true,
                new DateTime(2019, 12, 1),
                824.594m).SetName($"{nameof(ValueAfterTest)}-TwoEntry2Values5");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey2),
                new DateTime(2016, 6, 27),
                false,
                new DateTime(2019, 12, 1),
                824.59m).SetName($"{nameof(ValueAfterTest)}-TwoEntry2Values6");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey2),
                new DateTime(2022, 6, 27),
                true,
                new DateTime(2019, 12, 01),
                824.594m).SetName($"{nameof(ValueAfterTest)}-TwoEntry2Values7");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.FourEntryKey2),
                new DateTime(2018, 6, 1),
                false,
                new DateTime(2019, 1, 1),
                0.0m).SetName($"{nameof(ValueAfterTest)}-FourEntryDifferentDate");
        }

        [TestCaseSource(nameof(ValueAfterTestData))]
        public void ValueAfterTest(TimeList timelist, DateTime queryDate, bool isNull, DateTime expectedDate, decimal expectedValue)
        {
            var value = timelist.ValueAfter(queryDate);
            if (isNull)
            {
                Assert.IsNull(value);
            }
            else
            {
                Assert.Multiple(() =>
                {
                    Assert.IsNotNull(value);
                    Assert.That(expectedValue - value.Value, Is.LessThan(1e-8), $"Expected {expectedValue} but was {value.Value}");
                    Assert.AreEqual(expectedDate, value.Day);
                });
            }
        }


        [TestCaseSource(nameof(ValuesSpecialFuncTestSource))]
        public void ValuesSpecialFuncTests(decimal? expectedResult, DateTime expectedDate, DateTime date, TimeList timelist)
        {
            DailyValuation interpolator(DailyValuation earlier, DailyValuation later, DateTime chosenDate) => new DailyValuation(chosenDate, earlier.Value + (later.Value - earlier.Value) / (later.Day - earlier.Day).Days * (chosenDate - earlier.Day).Days);
            var actualValue = timelist.Value(date, interpolator);
            if (expectedResult == null)
            {
                Assert.IsNull(actualValue);
            }
            else
            {
                Assert.AreEqual(expectedDate, actualValue.Day, $" date not correct");
                Assert.AreEqual(expectedResult, actualValue.Value, $" value not correct");
            }
        }

        private static IEnumerable<TestCaseData> ValuesSpecialFuncTestSource()
        {
            foreach (var value in ValuesTestSourceData())
            {
                yield return new TestCaseData(value.Item2, value.Item3, value.Item4, value.Item5).SetName($"{nameof(ValuesSpecialFuncTests)}-{value.Name}");
            }
        }

        private static IEnumerable<(string Name, decimal?, DateTime, DateTime, TimeList)> ValuesTestSourceData()
        {
            yield return ("EmptyTimeList",
                null,
                new DateTime(2018, 1, 1),
                new DateTime(2018, 1, 1),
                TimeListTestData.GetTestTimeList(TimeListTestData.EmptyListKey));
            yield return ("TwoEntryZeroValues",
                0.0m,
                new DateTime(2018, 1, 1),
                new DateTime(2018, 1, 1),
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryZeroValuesKey));
            yield return ("FourEntryZeroValuesDifferentDate",
                0.0m,
                new DateTime(2018, 1, 1),
                new DateTime(2017, 1, 1),
                TimeListTestData.GetTestTimeList(TimeListTestData.FourEntryZeroValuesKey));
            yield return ("FourEntryZeroValuesSameDate",
                0.0m,
                new DateTime(2018, 1, 1),
                new DateTime(2018, 1, 1),
                TimeListTestData.GetTestTimeList(TimeListTestData.FourEntryZeroValuesKey));
            yield return ("FourEntryZeroValuesTest3",
                0.0m,
                new DateTime(2019, 5, 5),
                new DateTime(2020, 1, 1),
                TimeListTestData.GetTestTimeList(TimeListTestData.FourEntryZeroValuesKey));
            yield return ("FourEntryValues",
                0.0m,
                new DateTime(2018, 1, 1),
                new DateTime(2018, 1, 1),
                TimeListTestData.GetTestTimeList(TimeListTestData.FourEntryKey));
            yield return ("FourEntryValuesSecondTest",
                1.0500000000000000000000000021m,
                new DateTime(2019, 3, 5),
                new DateTime(2019, 3, 5),
                TimeListTestData.GetTestTimeList(TimeListTestData.FourEntryKey));
            yield return ("FourEntryValues2",
                2.0m,
                new DateTime(2019, 5, 1),
                new DateTime(2019, 5, 1),
                TimeListTestData.GetTestTimeList(TimeListTestData.FourEntryKey));
            yield return ("FourEntryValues2SecondTest",
                0.0m,
                new DateTime(2018, 1, 1),
                new DateTime(2018, 1, 1),
                TimeListTestData.GetTestTimeList(TimeListTestData.FourEntryKey2));
        }
    }
}
