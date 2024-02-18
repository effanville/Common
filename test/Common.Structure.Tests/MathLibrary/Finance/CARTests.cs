using System;

using Common.Structure.DataStructures;
using Common.Structure.DataStructures.Numeric;
using Common.Structure.MathLibrary.Finance;

using NUnit.Framework;

namespace Effanville.Common.Structure.Tests.MathLibrary.Finance
{
    [TestFixture]
    public sealed class CARTests
    {
        [TestCase("1/1/2018", 10.0, "1/1/2019", 20.0, 1.0)]
        [TestCase("1/1/2017", 10.0, "1/1/2019", 90.0, 2.0)]
        [TestCase("1/1/2018", 0.0, "1/1/2019", 90.0, double.NaN)]
        [TestCase("1/1/2018", 0.0, "1/1/2019", 0.0, 0.0)]
        [TestCase("1/1/2018", 10.0, "1/1/2019", 0.0, -1.0)]
        [TestCase("1/1/2018", 10.0, "1/1/2018", 11.0, double.NaN)]
        [TestCase("1/1/2018", 10.0, "1/1/2018", 10.0, 0.0)]
        public void CAR_BasicData_Tests(DateTime first, double firstValue, DateTime last, double lastValue, double expected)
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
        public void CAR_ComplexInput_Tests(DateTime first, double firstValue, DateTime last, double lastValue, double expected)
        {
            double rate = FinanceFunctions.CAR(new DailyNumeric(first, firstValue), new DailyNumeric(last, lastValue));
            Assert.AreEqual(expected, rate, "CAR is not as expected.");
        }

        [TestCase("1/1/2018", 10.0, "1/1/2019", 20.0, 1.0)]
        [TestCase("1/1/2017", 10.0, "1/1/2019", 90.0, 2.0)]
        [TestCase("1/1/2018", 0.0, "1/1/2019", 90.0, double.NaN)]
        [TestCase("1/1/2018", 0.0, "1/1/2019", 0.0, 0.0)]
        [TestCase("1/1/2018", 10.0, "1/1/2019", 0.0, -1.0)]
        [TestCase("1/1/2018", 10.0, "1/1/2018", 11.0, double.NaN)]
        [TestCase("1/1/2018", 10.0, "1/1/2018", 10.0, 0.0)]
        public void CAR_Decimal_BasicData_Tests(DateTime first, decimal firstValue, DateTime last, decimal lastValue, double expected)
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
        public void CAR_Decimal_ComplexInput_Tests(DateTime first, decimal firstValue, DateTime last, decimal lastValue, double expected)
        {
            double rate = FinanceFunctions.CAR(new DailyValuation(first, firstValue), new DailyValuation(last, lastValue));
            Assert.AreEqual(expected, rate, "CAR is not as expected.");
        }
    }
}
