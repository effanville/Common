using System;
using System.Collections.Generic;
using NUnit.Framework;
using Common.Structure.DataStructures;

namespace Common.Structure.Tests.DataStructures.Money
{
    [TestFixture]
    public sealed class TimeList_RatesTests
    {
        private static IEnumerable<TestCaseData> CARTests()
        {
            yield return new TestCaseData(TimeListTestData.GetTestTimeList(TimeListTestData.EmptyListKey), "1/1/2018", "1/1/2019", double.NaN).SetName("CAR-EmptyList");
            yield return new TestCaseData(TimeListTestData.GetTestTimeList(TimeListTestData.SingleEntryKey), "1/1/2018", "1/1/2019", 0.0).SetName("CAR-OneEntry");
            yield return new TestCaseData(TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey), "1/1/2018", "1/1/2019", 0.0).SetName("CAR-TwoEntry");
            yield return new TestCaseData(TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey), "1/1/2017", "1/1/2019", double.NaN).SetName("CAR-TwoEntryOutOfRange");
            yield return new TestCaseData(TimeListTestData.GetTestTimeList(TimeListTestData.ThreeEntryKey1), "1/1/2017", "1/1/2019", 0.1376).SetName("CAR-ThreeEntry");
        }

        [TestCaseSource(nameof(CARTests))]
        public void TimeList_CAR_Tests(TimeList switcher, string earlierDate, string laterDate, double expected)
        {
            double rate = switcher.CAR(DateTime.Parse(earlierDate), DateTime.Parse(laterDate));
            Assert.AreEqual(expected, rate, 1e-3, "CAR is not as expected.");
        }

        private static IEnumerable<TestCaseData> SumTests()
        {
            yield return new TestCaseData(TimeListTestData.GetTestTimeList(TimeListTestData.EmptyListKey), 0.0m).SetName("Sum-EmptyList");
            yield return new TestCaseData(TimeListTestData.GetTestTimeList(TimeListTestData.SingleEntryKey), 1000m).SetName("Sum-OneEntry");
            yield return new TestCaseData(TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey), 2000m).SetName("Sum-TwoEntry");
            yield return new TestCaseData(TimeListTestData.GetTestTimeList(TimeListTestData.ThreeEntryKey1), 3300m).SetName("Sum-ThreeEntry1");
            yield return new TestCaseData(TimeListTestData.GetTestTimeList(TimeListTestData.ThreeEntryKey2), 1100m).SetName("Sum-ThreeEntry2");
        }

        [TestCaseSource(nameof(SumTests))]
        public void TimeList_Sum_Tests(TimeList listUnderTest, decimal expected)
        {
            decimal value = listUnderTest.Sum();
            Assert.AreEqual(expected, value, "Values should be equal.");
        }
    }
}