using System;
using System.Collections.Generic;

using NUnit.Framework;

using Common.Structure.DataStructures;
using Common.Structure.MathLibrary.Finance;

namespace Common.Structure.Tests.MathLibrary.Finance
{
    [TestFixture]
    public class RatesMoneyTests
    {
        private static List<DailyValuation> Switcher(int i)
        {
            switch (i)
            {
                case 0:
                    return null;
                case 1:
                    return new List<DailyValuation>() { new DailyValuation(DateTime.Parse("1/1/2018"), 1000) };
                case 2:
                    return new List<DailyValuation>() { new DailyValuation(DateTime.Parse("1/1/2018"), 1000), new DailyValuation(DateTime.Parse("1/6/2018"), 1000) };
            }

            return null;
        }

        [TestCase("1/1/2018", 10.0, "1/1/2019", 20.0, 1.0)]
        [TestCase("1/1/2017", 10.0, "1/1/2019", 90.0, 2.0)]
        [TestCase("1/1/2018", 0.0, "1/1/2019", 90.0, double.NaN)]
        [TestCase("1/1/2018", 0.0, "1/1/2019", 0.0, 0.0)]
        [TestCase("1/1/2018", 10.0, "1/1/2019", 0.0, -1.0)]
        [TestCase("1/1/2018", 10.0, "1/1/2018", 11.0, double.NaN)]
        [TestCase("1/1/2018", 10.0, "1/1/2018", 10.0, 0.0)]
        public void CAR_BasicData_Tests(DateTime first, decimal firstValue, DateTime last, decimal lastValue, double expected)
        {
            double rate = FinanceFunctions.CAR(first, firstValue, last, lastValue);
            Assert.AreEqual(expected, rate, "CAR is not as expected.");
        }

        [TestCase("1/1/2018", 10.0, "1/1/2019", 20.0, 1.0)]
        [TestCase("1/1/2017", 10.0, "1/1/2019", 90.0, 2.0)]
        [TestCase("1/1/2018", 0.0, "1/1/2019", 90.0, double.NaN)]
        [TestCase("1/1/2018", 0.0, "1/1/2019", 0.0, 0.0)]
        [TestCase("1/1/2018", 10.0, "1/1/2019", 0.0, -1.0)]
        [TestCase("1/1/2018", 10.0, "1/1/2018", 11.0, double.NaN)]
        [TestCase("1/1/2018", 10.0, "1/1/2018", 10.0, 0.0)]
        public void CAR_ComplexInput_Tests(DateTime first, decimal firstValue, DateTime last, decimal lastValue, double expected)
        {
            double rate = FinanceFunctions.CAR(new DailyValuation(first, firstValue), new DailyValuation(last, lastValue));
            Assert.AreEqual(expected, rate, "CAR is not as expected.");
        }

        [TestCase(0, "1/1/2019", 2000.0, double.NaN)]
        [TestCase(1, "1/1/2019", 2000.0, 1.0)]
        [TestCase(2, "1/1/2019", 2000.0, 4.76837158203125E-07)]
        [TestCase(2, "1/1/2019", 4000.0, 1.3498024940490723)]
        public void IRRTests(int switcher, DateTime last, decimal lastValue, double expected)
        {
            double rate = FinanceFunctions.IRR(Switcher(switcher), new DailyValuation(last, lastValue));
            Assert.AreEqual(expected, rate, 1e-8, "CAR is not as expected.");
        }

        [TestCase(0, "1/1/2018", 1000.0, "1/1/2019", 2000.0, double.NaN)]
        [TestCase(1, "1/1/2018", 1000.0, "1/1/2019", 2000.0, 1.0)]
        [TestCase(1, "6/1/2018", 1000.0, "1/1/2019", 2000.0, 2.2616624310427929)]
        [TestCase(2, "1/1/2018", 1000.0, "1/1/2019", 2000.0, 0.00048828125)]
        [TestCase(2, "1/1/2018", 1000.0, "1/1/2019", 4000.0, 1.35107421875)]
        public void IRR_Time_Tests(int switcher, DateTime start, decimal startValue, DateTime last, decimal lastValue, double expected)
        {
            double rate = FinanceFunctions.IRR(new DailyValuation(start, startValue), Switcher(switcher), new DailyValuation(last, lastValue), 10);
            Assert.AreEqual(expected, rate, 1e-8, "CAR is not as expected.");
        }

        [TestCase(0, "1/1/2018", 1000.0, "1/1/2019", 2000.0, double.NaN)]
        [TestCase(1, "1/1/2018", 1000.0, "1/1/2019", 2000.0, 1.0)]
        [TestCase(1, "6/1/2018", 1000.0, "1/1/2019", 2000.0, 2.2616624310427929)]
        [TestCase(2, "1/1/2018", 1000.0, "1/1/2019", 2000.0, 4.76837158203125E-07)]
        [TestCase(2, "1/1/2018", 1000.0, "1/1/2019", 4000.0, 1.3498024940490723)]
        public void IRR_Time_Tests_HighAccuracy(int switcher, DateTime start, decimal startValue, DateTime last, decimal lastValue, double expected)
        {
            double rate = FinanceFunctions.IRR(new DailyValuation(start, startValue), Switcher(switcher), new DailyValuation(last, lastValue), 20);
            Assert.AreEqual(expected, rate, 1e-8, "CAR is not as expected.");
        }
    }
}