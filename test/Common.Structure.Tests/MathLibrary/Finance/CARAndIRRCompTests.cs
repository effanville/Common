﻿using System;
using System.Collections.Generic;

using Effanville.Common.Structure.DataStructures;
using Effanville.Common.Structure.MathLibrary.Finance;

using NUnit.Framework;

namespace Effanville.Common.Structure.Tests.MathLibrary.Finance
{
    [TestFixture]
    internal sealed class CARAndIRRCompTests
    {
        [TestCase("1/1/2018", 10.0, "1/1/2019", 20.0)]
        [TestCase("1/1/2017", 10.0, "1/1/2019", 90.0)]
        [TestCase("1/1/2018", 0.0, "1/1/2019", 90.0)]
        [TestCase("1/1/2018", 0.0, "1/1/2019", 0.0)]
        [TestCase("1/1/2018", 10.0, "1/1/2019", 0.0)]
        [TestCase("1/1/2018", 10.0, "1/1/2018", 11.0)]
        [TestCase("1/1/2018", 10.0, "1/1/2018", 10.0)]
        public void CAR_Decimal_ComplexInput_Tests(DateTime first, decimal firstValue, DateTime last, decimal lastValue)
        {
            double CAR = FinanceFunctions.CAR(new DailyValuation(first, firstValue), new DailyValuation(last, lastValue));

            double IRR = FinanceFunctions.IRR(new DailyValuation(first, firstValue), new List<DailyValuation>(), new DailyValuation(last, lastValue));
            Assert.That(IRR, Is.EqualTo(CAR), "CAR is not as expected.");
        }
    }
}
