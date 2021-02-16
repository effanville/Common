using System;
using System.Collections.Generic;
using NUnit.Framework;
using StructureCommon.DataStructures;

namespace StructureCommon.Tests.DataStructures
{
    public class TimeList_Tests
    {
        [TestCaseSource(nameof(TryAddValueTestSource))]
        public void TryAddValueTests(int count, params (DateTime date, double value)[] first)
        {
            TimeList newList = new TimeList();
            foreach (var value in first)
            {
                _ = newList.TryAddValue(value.date, value.value);
            }
            Assert.AreEqual(count, newList.Count());
        }

        private static IEnumerable<TestCaseData> TryAddValueTestSource()
        {
            yield return new TestCaseData(0, Array.Empty<(DateTime, double)>());
            yield return new TestCaseData(1, new[] { (new DateTime(2018, 1, 1), 0.0), (new DateTime(2018, 1, 1), 0.0) });
            yield return new TestCaseData(2, new[] { (new DateTime(2018, 1, 1), 0.0), (new DateTime(2019, 1, 1), 0.0) });
        }

        [TestCaseSource(nameof(AnyTestSource))]
        public void AnyTests(bool result, params (DateTime date, double value)[] first)
        {
            TimeList newList = new TimeList();
            foreach (var value in first)
            {
                _ = newList.TryAddValue(value.date, value.value);
            }
            Assert.AreEqual(result, newList.Any());
        }

        private static IEnumerable<TestCaseData> AnyTestSource()
        {
            yield return new TestCaseData(false, Array.Empty<(DateTime, double)>());
            yield return new TestCaseData(true, new[] { (new DateTime(2018, 1, 1), 0.0), (new DateTime(2018, 1, 1), 0.0) });
        }

        [TestCaseSource(nameof(CleanValuesTestSource))]
        public void CleanValuesTests((DateTime date, double value)[] result, params (DateTime date, double value)[] first)
        {
            var timelist = new TimeList();
            foreach (var value in first)
            {
                _ = timelist.TryAddValue(value.date, value.value);
            }

            timelist.CleanValues();

            Assert.AreEqual(result.Length, timelist.Count());
            for (int i = 0; i < result.Length; ++i)
            {
                Assert.AreEqual(result[i].date, timelist.Values[i].Day, $"Index {i} date not correct");
                Assert.AreEqual(result[i].value, timelist.Values[i].Value, $"Index {i} value not correct");
            }
        }

        private static IEnumerable<TestCaseData> CleanValuesTestSource()
        {
            yield return new TestCaseData(Array.Empty<(DateTime, double)>(), Array.Empty<(DateTime, double)>());
            yield return new TestCaseData(new[] { (new DateTime(2018, 1, 1), 0.0) }, new[] { (new DateTime(2018, 1, 1), 0.0), (new DateTime(2019, 1, 1), 0.0) });
            yield return new TestCaseData(new[] { (new DateTime(2018, 1, 1), 0.0) }, new[] { (new DateTime(2018, 1, 1), 0.0), (new DateTime(2019, 1, 1), 0.0), (new DateTime(2019, 5, 1), 0.0), (new DateTime(2019, 5, 5), 0.0) });
            yield return new TestCaseData(new[] { (new DateTime(2018, 1, 1), 0.0), (new DateTime(2019, 5, 1), 2.0) }, new[] { (new DateTime(2018, 1, 1), 0.0), (new DateTime(2019, 1, 1), 0.0), (new DateTime(2019, 5, 1), 2.0), (new DateTime(2019, 5, 5), 2.0) });
            yield return new TestCaseData(new[] { (new DateTime(2018, 1, 1), 0.0), (new DateTime(2019, 1, 1), 1.0), (new DateTime(2019, 5, 1), 2.0), (new DateTime(2019, 5, 5), 0.0) }, new[] { (new DateTime(2018, 1, 1), 0.0), (new DateTime(2019, 1, 1), 1.0), (new DateTime(2019, 5, 1), 2.0), (new DateTime(2019, 5, 5), 0.0) });
        }

        [TestCaseSource(nameof(ValuesTestSource))]
        public void ValuesTests(double? expectedResult, DateTime expectedDate, DateTime date, params (DateTime date, double value)[] first)
        {
            var timelist = new TimeList();
            foreach (var value in first)
            {
                _ = timelist.TryAddValue(value.date, value.value);
            }

            var actualValue = timelist.Value(date);
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

        [TestCaseSource(nameof(ValuesTestSource))]
        public void ValuesSpecialFuncTests(double? expectedResult, DateTime expectedDate, DateTime date, params (DateTime date, double value)[] first)
        {
            var timelist = new TimeList();
            foreach (var value in first)
            {
                _ = timelist.TryAddValue(value.date, value.value);
            }

            Func<DailyValuation, DailyValuation, DateTime, double> interpolator = (earlier, later, chosenDate) => earlier.Value + (later.Value - earlier.Value) / (later.Day - earlier.Day).Days * (chosenDate - earlier.Day).Days;
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

        private static IEnumerable<TestCaseData> ValuesTestSource()
        {
            yield return new TestCaseData(null, new DateTime(2018, 1, 1), new DateTime(2018, 1, 1), Array.Empty<(DateTime, double)>());
            yield return new TestCaseData(0.0, new DateTime(2018, 1, 1), new DateTime(2018, 1, 1), new[] { (new DateTime(2018, 1, 1), 0.0), (new DateTime(2019, 1, 1), 0.0) });
            yield return new TestCaseData(0.0, new DateTime(2018, 1, 1), new DateTime(2017, 1, 1), new[] { (new DateTime(2018, 1, 1), 0.0), (new DateTime(2019, 1, 1), 0.0), (new DateTime(2019, 5, 1), 0.0), (new DateTime(2019, 5, 5), 0.0) });
            yield return new TestCaseData(0.0, new DateTime(2018, 1, 1), new DateTime(2018, 1, 1), new[] { (new DateTime(2018, 1, 1), 0.0), (new DateTime(2019, 1, 1), 0.0), (new DateTime(2019, 5, 1), 0.0), (new DateTime(2019, 5, 5), 0.0) });
            yield return new TestCaseData(0.0, new DateTime(2019, 5, 5), new DateTime(2020, 1, 1), new[] { (new DateTime(2018, 1, 1), 0.0), (new DateTime(2019, 1, 1), 0.0), (new DateTime(2019, 5, 1), 0.0), (new DateTime(2019, 5, 5), 0.0) });
            yield return new TestCaseData(0.0, new DateTime(2018, 1, 1), new DateTime(2018, 1, 1), new[] { (new DateTime(2018, 1, 1), 0.0), (new DateTime(2019, 1, 1), 0.0), (new DateTime(2019, 5, 1), 2.0), (new DateTime(2019, 5, 5), 2.0) });
            yield return new TestCaseData(1.05, new DateTime(2019, 3, 5), new DateTime(2019, 3, 5), new[] { (new DateTime(2018, 1, 1), 0.0), (new DateTime(2019, 1, 1), 0.0), (new DateTime(2019, 5, 1), 2.0), (new DateTime(2019, 5, 5), 2.0) });
            yield return new TestCaseData(2.0, new DateTime(2019, 5, 1), new DateTime(2019, 5, 1), new[] { (new DateTime(2018, 1, 1), 0.0), (new DateTime(2019, 1, 1), 0.0), (new DateTime(2019, 5, 1), 2.0), (new DateTime(2019, 5, 5), 2.0) });
            yield return new TestCaseData(0.0, new DateTime(2018, 1, 1), new DateTime(2018, 1, 1), new[] { (new DateTime(2018, 1, 1), 0.0), (new DateTime(2019, 1, 1), 1.0), (new DateTime(2019, 5, 1), 2.0), (new DateTime(2019, 5, 5), 0.0) });
        }
    }
}
