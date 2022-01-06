using System;
using System.Collections.Generic;

using Common.Structure.DataStructures;

using NUnit.Framework;

namespace Common.Structure.Tests.DataStructures.Money
{
    [TestFixture]
    public sealed class TimeListValuesTests
    {
        private static IEnumerable<TestCaseData> ValueBeforeTestData()
        {
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey2),
                new DateTime(2016, 6, 27),
                new DateTime(2012, 8, 31),
                824.59m).SetName($"{nameof(ValueBeforeTests)}-Entry1");
            yield return new TestCaseData(
                TimeListTestData.GetTestTimeList(TimeListTestData.TwoEntryKey2),
                new DateTime(2022, 6, 27),
                new DateTime(2019, 12, 01),
                824.594m).SetName($"{nameof(ValueBeforeTests)}-Entry1");
        }

        [TestCaseSource(nameof(ValueBeforeTestData))]
        public void ValueBeforeTests(TimeList timelist, DateTime queryDate, DateTime expectedDate, decimal expectedValue)
        {
            var value = timelist.ValueBefore(queryDate);
            Assert.Multiple(() =>
            {
                Assert.AreEqual(expectedValue, value.Value);
                Assert.AreEqual(expectedDate, value.Day);
            });
        }
    }
}
