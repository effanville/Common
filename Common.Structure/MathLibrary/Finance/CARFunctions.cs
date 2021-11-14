using System;

using Common.Structure.DataStructures;
using Common.Structure.DataStructures.Numeric;

namespace Common.Structure.MathLibrary.Finance
{
    /// <summary>
    /// Contains functions for calculating the compound annual rate.
    /// </summary>
    public static partial class FinanceFunctions
    {
        /// <summary>
        /// Returns the compound annual rate from the <paramref name="firstValue"/> to the <paramref name="lastValue"/> in the time last-first
        /// </summary>
        public static double CAR(DailyValuation firstValue, DailyValuation lastValue)
        {
            if (firstValue == null || lastValue == null)
            {
                return double.NaN;
            }

            return CAR(firstValue.Day, firstValue.Value, lastValue.Day, lastValue.Value);
        }

        /// <summary>
        /// Returns the compound annual rate from the <paramref name="firstValue"/> to the <paramref name="lastValue"/> in the time last-first
        /// </summary>
        public static double CAR(DailyNumeric firstValue, DailyNumeric lastValue)
        {
            if (firstValue == null || lastValue == null)
            {
                return double.NaN;
            }

            return CAR(firstValue.Day, firstValue.Value, lastValue.Day, lastValue.Value);
        }

        /// <summary>
        /// Returns the compound annual rate from the firstValue to the lastValue in the time last-first
        /// </summary>
        public static double CAR(DateTime first, double firstValue, DateTime last, double lastValue)
        {
            if (firstValue == lastValue)
            {
                return 0.0;
            }
            if (firstValue == 0 || (last - first).Days == 0)
            {
                return double.NaN;
            }
            if (lastValue == 0)
            {
                return -1.0;
            }

            return Math.Pow(lastValue / firstValue, 365.0 / (last - first).Days) - 1;
        }

        /// <summary>
        /// Returns the compound annual rate from the firstValue to the lastValue in the time last-first
        /// </summary>
        public static double CAR(DateTime first, decimal firstValue, DateTime last, decimal lastValue)
        {
            if (firstValue == lastValue)
            {
                return 0.0;
            }
            if (firstValue == 0 || (last - first).Days == 0)
            {
                return double.NaN;
            }
            if (lastValue == 0)
            {
                return -1.0;
            }

            return Math.Pow((double)(lastValue / firstValue), 365.0 / (last - first).Days) - 1;
        }
    }
}
