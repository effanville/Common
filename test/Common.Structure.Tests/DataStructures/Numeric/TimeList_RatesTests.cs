using System;
using System.Collections.Generic;

using Common.Structure.DataStructures.Numeric;

using NUnit.Framework;

namespace Effanville.Common.Structure.Tests.DataStructures.Numeric
{
    [TestFixture]
    public sealed class TimeList_RatesTests
    {
        private static IEnumerable<TestCaseData> CAROnOrBeforeTests()
        {
            yield return new TestCaseData(TimeListTestData.GetTestTimeList(TimeListTestData.EmptyListKey), "1/1/2018", "1/1/2019", double.NaN).SetName("CAROnOrBefore-EmptyList");
            yield return new TestCaseData(TimeListTestData.GetTestTimeList(TimeListTestData.SingleEntryKey), "1/1/2018", "1/1/2019", 0.0).SetName("CAROnOrBefore-OneEntry");
            yield return new TestCaseData(TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey), "1/1/2018", "1/1/2019", 0.7149548092821405).SetName("CAROnOrBefore-TwoEntry");
            yield return new TestCaseData(TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey), "1/1/2017", "1/1/2019", double.NaN).SetName("CAROnOrBefore-TwoEntryOutOfRange");
            yield return new TestCaseData(TimeListTestData.GetTestTimeList(TimeListTestData.ThreeEntryKey1), "1/1/2017", "1/1/2019", 0.1376).SetName("CAROnOrBefore-ThreeEntry");
        }

        [TestCaseSource(nameof(CAROnOrBeforeTests))]
        public void TimeList_CAROnOrBefore_Tests(TimeNumberList switcher, string earlierDate, string laterDate, double expected)
        {
            double rate = switcher.CAROnOrBefore(DateTime.Parse(earlierDate), DateTime.Parse(laterDate));
            Assert.AreEqual(expected, rate, 1e-3, "CAR is not as expected.");
        }
        private static IEnumerable<TestCaseData> CARTests()
        {
            yield return new TestCaseData(TimeListTestData.GetTestTimeList(TimeListTestData.EmptyListKey), "1/1/2018", "1/1/2019", double.NaN).SetName("CAR-EmptyList");
            yield return new TestCaseData(TimeListTestData.GetTestTimeList(TimeListTestData.SingleEntryKey), "1/1/2018", "1/1/2019", 0.0).SetName("CAR-OneEntry");
            yield return new TestCaseData(TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey), "1/1/2018", "1/1/2019", 0.7149548092821405).SetName("CAR-TwoEntry");
            yield return new TestCaseData(TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey), "1/1/2017", "1/1/2019", 0.7149548092821405).SetName("CAR-TwoEntryOutOfRange");
            yield return new TestCaseData(TimeListTestData.GetTestTimeList(TimeListTestData.ThreeEntryKey1), "1/1/2017", "1/1/2019", 0.1376).SetName("CAR-ThreeEntry");
        }

        [TestCaseSource(nameof(CARTests))]
        public void TimeList_CAR_Tests(TimeNumberList switcher, string earlierDate, string laterDate, double expected)
        {
            double rate = switcher.CAR(DateTime.Parse(earlierDate), DateTime.Parse(laterDate));
            Assert.AreEqual(expected, rate, 1e-3, "CAR is not as expected.");
        }

        private static IEnumerable<TestCaseData> SumTests()
        {
            yield return new TestCaseData(TimeListTestData.GetTestTimeList(TimeListTestData.EmptyListKey), 0.0).SetName("Sum-EmptyList");
            yield return new TestCaseData(TimeListTestData.GetTestTimeList(TimeListTestData.SingleEntryKey), 1000).SetName("Sum-OneEntry");
            yield return new TestCaseData(TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey), 2250).SetName("Sum-TwoEntry");
            yield return new TestCaseData(TimeListTestData.GetTestTimeList(TimeListTestData.ThreeEntryKey1), 3300).SetName("Sum-ThreeEntry1");
            yield return new TestCaseData(TimeListTestData.GetTestTimeList(TimeListTestData.ThreeEntryKey2), 1100).SetName("Sum-ThreeEntry2");
        }

        [TestCaseSource(nameof(SumTests))]
        public void TimeList_Sum_Tests(TimeNumberList listUnderTest, double expected)
        {
            double value = listUnderTest.Sum();
            Assert.AreEqual(expected, value, "Values should be equal.");
        }
    }
}