using System;
using System.Collections.Generic;

using NUnit.Framework;

using Common.Structure.MathLibrary.Finance;
using Common.Structure.DataStructures.Numeric;

namespace Common.Structure.Tests.MathLibrary.Finance
{
    [TestFixture]
    public class DailyNumericIRRTests
    {
        private static List<DailyNumeric> ExampleNumericLists(int listIndex)
        {
            switch (listIndex)
            {
                case 0:
                    return null;
                case 1:
                    return new List<DailyNumeric>() { new DailyNumeric(new DateTime(2018, 1, 1), 1000) };
                case 2:
                    return new List<DailyNumeric>() { new DailyNumeric(new DateTime(2018, 1, 1), 1000), new DailyNumeric(new DateTime(2018, 6, 1), 1000) };
            }

            return null;
        }

        [TestCase(0, "1/1/2019", 2000.0, double.NaN)]
        [TestCase(1, "1/1/2019", 2000.0, 1.0)]
        [TestCase(2, "1/1/2019", 2000.0, 4.76837158203125E-07)]
        [TestCase(2, "1/1/2019", 4000.0, 1.3498024940490723)]
        public void IRRTests(int switcher, DateTime last, double lastValue, double expected)
        {
            double rate = FinanceFunctions.IRR(ExampleNumericLists(switcher), new DailyNumeric(last, lastValue));
            Assert.AreEqual(expected, rate, 1e-8, "CAR is not as expected.");
        }

        [TestCase(0, "1/1/2018", 1000.0, "1/1/2019", 2000.0, double.NaN)]
        [TestCase(1, "1/1/2018", 1000.0, "1/1/2019", 2000.0, 1.0)]
        [TestCase(1, "6/1/2018", 1000.0, "1/1/2019", 2000.0, 2.2616624310427929)]
        [TestCase(2, "1/1/2018", 1000.0, "1/1/2019", 2000.0, 0.00048828125)]
        [TestCase(2, "1/1/2018", 1000.0, "1/1/2019", 4000.0, 1.35107421875)]
        public void IRR_Time_Tests(int switcher, DateTime start, double startValue, DateTime last, double lastValue, double expected)
        {
            double rate = FinanceFunctions.IRR(new DailyNumeric(start, startValue), ExampleNumericLists(switcher), new DailyNumeric(last, lastValue), 10);
            Assert.AreEqual(expected, rate, 1e-8, "CAR is not as expected.");
        }

        [TestCase(0, "1/1/2018", 1000.0, "1/1/2019", 2000.0, double.NaN)]
        [TestCase(1, "1/1/2018", 1000.0, "1/1/2019", 2000.0, 1.0)]
        [TestCase(1, "6/1/2018", 1000.0, "1/1/2019", 2000.0, 2.2616624310427929)]
        [TestCase(2, "1/1/2018", 1000.0, "1/1/2019", 2000.0, 4.76837158203125E-07)]
        [TestCase(2, "1/1/2018", 1000.0, "1/1/2019", 4000.0, 1.3498024940490723)]
        public void IRR_Time_Tests_HighAccuracy(int switcher, DateTime start, double startValue, DateTime last, double lastValue, double expected)
        {
            double rate = FinanceFunctions.IRR(new DailyNumeric(start, startValue), ExampleNumericLists(switcher), new DailyNumeric(last, lastValue), 20);
            Assert.AreEqual(expected, rate, 1e-8, "CAR is not as expected.");
        }
    }
}