using System;
using System.Collections.Generic;
using NUnit.Framework;
using Common.Structure.DataStructures;

namespace Common.Structure.Tests.DataStructures
{

    [TestFixture]
    public class TimeList_RatesTests
    {
        private static IEnumerable<TestCaseData> CARTests()
        {
            yield return new TestCaseData(TimeListTestData.GetTestTimeList(TimeListTestData.EmptyListKey), "1/1/2018", "1/1/2019", 0.0);
            yield return new TestCaseData(TimeListTestData.GetTestTimeList(TimeListTestData.SingleEntryKey), "1/1/2018", "1/1/2019", 0.0);
            yield return new TestCaseData(TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey), "1/1/2018", "1/1/2019", 0.0);
            yield return new TestCaseData(TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey), "1/1/2017", "1/1/2019", double.NaN);
            yield return new TestCaseData(TimeListTestData.GetTestTimeList(TimeListTestData.ThreeEntryKey1), "1/1/2017", "1/1/2019", 0.1376);
        }

        [TestCaseSource(nameof(CARTests))]
        public void TimeList_CAR_Tests(TimeList switcher, string earlierDate, string laterDate, double expected)
        {
            double rate = switcher.CAR(DateTime.Parse(earlierDate), DateTime.Parse(laterDate));
            Assert.AreEqual(expected, rate, 1e-3, "CAR is not as expected.");
        }

        private static IEnumerable<TestCaseData> SumTests()
        {
            yield return new TestCaseData(TimeListTestData.GetTestTimeList(TimeListTestData.EmptyListKey), double.NaN);
            yield return new TestCaseData(TimeListTestData.GetTestTimeList(TimeListTestData.SingleEntryKey), 1000);
            yield return new TestCaseData(TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey), 2000);
            yield return new TestCaseData(TimeListTestData.GetTestTimeList(TimeListTestData.ThreeEntryKey1), 3300);
            yield return new TestCaseData(TimeListTestData.GetTestTimeList(TimeListTestData.ThreeEntryKey2), 1100);
        }

        [TestCaseSource(nameof(SumTests))]
        public void TimeList_Sum_Tests(TimeList listUnderTest, double expected)
        {
            double value = listUnderTest.Sum();
            Assert.AreEqual(expected, value, "Values should be equal.");
        }
    }
}