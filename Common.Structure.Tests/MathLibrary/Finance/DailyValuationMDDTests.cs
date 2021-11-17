using System;
using System.Collections.Generic;

using Common.Structure.DataStructures;
using Common.Structure.MathLibrary.Finance;

using NUnit.Framework;

namespace Common.Structure.Tests.MathLibrary.Finance
{
    [TestFixture]
    public sealed class DailyValuationMDDTests
    {
        private static List<DailyValuation> ExampleValuationList(int listIndex)
        {
            switch (listIndex)
            {
                case 0:
                    return null;
                case 1:
                    return new List<DailyValuation>();
                case 2:
                    return new List<DailyValuation>()
                    {
                        new DailyValuation(new DateTime(2018, 1, 1), 1000),
                        new DailyValuation(new DateTime(2019, 1, 1), 2000)
                    };
                case 3:
                    return new List<DailyValuation>()
                    {
                        new DailyValuation(new DateTime(2018, 1, 1), 1000),
                        new DailyValuation(new DateTime(2018, 6, 1), 1000),
                        new DailyValuation(new DateTime(2019, 1, 1), 2000)
                    };
                case 4:
                    return new List<DailyValuation>()
                    {
                        new DailyValuation(new DateTime(2018, 1, 1), 1000),
                        new DailyValuation(new DateTime(2018, 6, 1), 1000),
                        new DailyValuation(new DateTime(2019, 1, 1), 500)
                    };
                case 5:
                    return new List<DailyValuation>()
                    {
                        new DailyValuation(new DateTime(2018, 1, 1), 1000),
                        new DailyValuation(new DateTime(2018, 6, 1), 2000),
                        new DailyValuation(new DateTime(2019, 1, 1), 1000),
                        new DailyValuation(new DateTime(2020, 1, 1), 2000),
                        new DailyValuation(new DateTime(2021, 1, 1), 1100)
                    };
                case 6:
                    return new List<DailyValuation>()
                    {
                        new DailyValuation(new DateTime(2018, 1, 1), 1000),
                        new DailyValuation(new DateTime(2018, 6, 1), 2000),
                        new DailyValuation(new DateTime(2019, 1, 1), 1000),
                        new DailyValuation(new DateTime(2020, 1, 1), 2000),
                        new DailyValuation(new DateTime(2021, 1, 1), 500)
                    };
                case 7:
                    return new List<DailyValuation>()
                    {
                        new DailyValuation(new DateTime(2018, 1, 1), 0.0m),
                        new DailyValuation(new DateTime(2018, 6, 1), 2000),
                        new DailyValuation(new DateTime(2019, 1, 1), 1000),
                        new DailyValuation(new DateTime(2020, 1, 1), 2000),
                        new DailyValuation(new DateTime(2021, 1, 1), 500)
                    };
                case 8:
                    return new List<DailyValuation>()
                    {
                        new DailyValuation(new DateTime(2018, 1, 1), 0.0m)
                    };
            }

            return null;
        }

        [TestCase(0, 7.9228162514264338E+28)]
        [TestCase(1, 7.9228162514264338E+28)]
        [TestCase(2, 0.0)]
        [TestCase(3, 0.0)]
        [TestCase(4, 50.0)]
        [TestCase(5, 50.0)]
        [TestCase(6, 75.0)]
        [TestCase(7, 75.0)]
        [TestCase(8, 0.0)]
        public void MDDTests(int listIndex, double expected)
        {
            decimal rate = FinanceFunctions.MDD(ExampleValuationList(listIndex));
            Assert.AreEqual(expected, (double)rate, 1e-8, "MDD is not as expected.");
        }
    }
}
