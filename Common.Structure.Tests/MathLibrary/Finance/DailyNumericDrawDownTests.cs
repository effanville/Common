﻿using System;
using System.Collections.Generic;

using Common.Structure.DataStructures.Numeric;
using Common.Structure.MathLibrary.Finance;

using NUnit.Framework;

namespace Common.Structure.Tests.MathLibrary.Finance
{
    [TestFixture]
    public sealed class DailyNumericDrawDownTests
    {
        private static List<DailyNumeric> ExampleValuationList(int listIndex)
        {
            switch (listIndex)
            {
                case 0:
                    return null;
                case 1:
                    return new List<DailyNumeric>();
                case 2:
                    return new List<DailyNumeric>()
                    {
                        new DailyNumeric(new DateTime(2018, 1, 1), 1000),
                        new DailyNumeric(new DateTime(2019, 1, 1), 2000)
                    };
                case 3:
                    return new List<DailyNumeric>()
                    {
                        new DailyNumeric(new DateTime(2018, 1, 1), 1000),
                        new DailyNumeric(new DateTime(2018, 6, 1), 1000),
                        new DailyNumeric(new DateTime(2019, 1, 1), 2000)
                    };
                case 4:
                    return new List<DailyNumeric>()
                    {
                        new DailyNumeric(new DateTime(2018, 1, 1), 1000),
                        new DailyNumeric(new DateTime(2018, 6, 1), 1000),
                        new DailyNumeric(new DateTime(2019, 1, 1), 500)
                    };
                case 5:
                    return new List<DailyNumeric>()
                    {
                        new DailyNumeric(new DateTime(2018, 1, 1), 1000),
                        new DailyNumeric(new DateTime(2018, 6, 1), 2000),
                        new DailyNumeric(new DateTime(2019, 1, 1), 1000),
                        new DailyNumeric(new DateTime(2020, 1, 1), 2000),
                        new DailyNumeric(new DateTime(2021, 1, 1), 1100)
                    };
                case 6:
                    return new List<DailyNumeric>()
                    {
                        new DailyNumeric(new DateTime(2018, 1, 1), 1000),
                        new DailyNumeric(new DateTime(2018, 6, 1), 2000),
                        new DailyNumeric(new DateTime(2019, 1, 1), 1000),
                        new DailyNumeric(new DateTime(2020, 1, 1), 2000),
                        new DailyNumeric(new DateTime(2021, 1, 1), 500)
                    };
                case 7:
                    return new List<DailyNumeric>()
                    {
                        new DailyNumeric(new DateTime(2018, 1, 1), 0.0),
                        new DailyNumeric(new DateTime(2018, 6, 1), 2000),
                        new DailyNumeric(new DateTime(2019, 1, 1), 1000),
                        new DailyNumeric(new DateTime(2020, 1, 1), 2000),
                        new DailyNumeric(new DateTime(2021, 1, 1), 500)
                    };
                case 8:
                    return new List<DailyNumeric>()
                    {
                        new DailyNumeric(new DateTime(2018, 1, 1), 0.0)
                    };
            }

            return null;
        }

        [TestCase(0, double.NaN)]
        [TestCase(1, double.NaN)]
        [TestCase(2, 0.0)]
        [TestCase(3, 0.0)]
        [TestCase(4, 50.0)]
        [TestCase(5, 45.0)]
        [TestCase(6, 75.0)]
        [TestCase(7, 75.0)]
        [TestCase(8, 0.0)]
        public void DrawDownTests(int listIndex, double expected)
        {
            double rate = FinanceFunctions.Drawdown(ExampleValuationList(listIndex));
            Assert.AreEqual(expected, rate, 1e-8, "MDD is not as expected.");
        }
    }
}
