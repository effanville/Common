using System;
using System.Collections.Generic;
using NUnit.Framework;
using Common.Structure.DataStructures;

namespace Common.Structure.Tests.DataStructures
{
    public class TimeList_RatesTests
    {
        private static List<TimeList> TestLists()
        {
            var output = new List<TimeList>();
            output.Add(null);
            output.Add(new TimeList(new List<DailyValuation>() { new DailyValuation(new DateTime(2018, 1, 1), 1000) }));
            output.Add(new TimeList(new List<DailyValuation>() { new DailyValuation(DateTime.Parse("1/1/2018"), 1000), new DailyValuation(DateTime.Parse("1/6/2018"), 1000) }));
            output.Add(new TimeList(new List<DailyValuation>() { new DailyValuation(DateTime.Parse("1/1/2017"), 1000), new DailyValuation(DateTime.Parse("1/1/2018"), 1100), new DailyValuation(DateTime.Parse("1/6/2018"), 1200) }));
            output.Add(new TimeList(new List<DailyValuation>() { new DailyValuation(DateTime.Parse("1/1/2017"), 1000), new DailyValuation(DateTime.Parse("1/1/2018"), -1100), new DailyValuation(DateTime.Parse("1/6/2018"), 1200) }));
            return output;
        }

        private static IEnumerable<TestCaseData> CARTests()
        {
            var tests = TestLists();
            yield return new TestCaseData(tests[1], "1/1/2018", "1/1/2019", 0.0);
            yield return new TestCaseData(tests[2], "1/1/2018", "1/1/2019", 0.0);
            yield return new TestCaseData(tests[2], "1/1/2017", "1/1/2019", double.NaN);
            yield return new TestCaseData(tests[4], "1/1/2017", "1/1/2019", 0.1376);
        }

        [TestCaseSource(nameof(CARTests))]
        public void TimeList_CAR_Tests(TimeList switcher, string earlierDate, string laterDate, double expected)
        {
            double rate = switcher.CAR(DateTime.Parse(earlierDate), DateTime.Parse(laterDate));
            Assert.AreEqual(expected, rate, 1e-3, "CAR is not as expected.");
        }

        private static IEnumerable<TestCaseData> SumTests()
        {
            var tests = TestLists();
            yield return new TestCaseData(tests[1], 1000);
            yield return new TestCaseData(tests[2], 2000);
            yield return new TestCaseData(tests[3], 3300);
            yield return new TestCaseData(tests[4], 1100);
        }

        [TestCaseSource(nameof(SumTests))]
        public void TimeList_Sum_Tests(TimeList listUnderTest, double expected)
        {
            double value = listUnderTest.Sum();
            Assert.AreEqual(expected, value, "Values should be equal.");
        }
    }
}