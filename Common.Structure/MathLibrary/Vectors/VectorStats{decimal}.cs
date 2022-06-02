using System;
using System.Collections.Generic;

namespace Common.Structure.MathLibrary.Vectors
{
    /// <summary>
    /// Contains helper methods for calculating a statistic from a list.
    /// </summary>
    public static partial class VectorStats
    {
        /// <summary>
        /// Calculates the maximum value of the subset consisting of the
        /// final number of values of a list of doubles
        /// </summary>
        /// <param name="values">The list to calculate the max for.</param>
        /// <param name="number">The final number of values to consider.</param>
        /// <returns>The maximum value.</returns>
        public static decimal Max(List<decimal> values, int number)
        {
            if (values == null)
            {
                return decimal.MinValue;
            }

            if (values.Count < number)
            {
                return decimal.MinValue;
            }

            decimal maximum = decimal.MinValue;
            for (int index = 0; index < number; index++)
            {
                decimal latestVal = values[values.Count - 1 - index];
                if (maximum < latestVal)
                {
                    maximum = latestVal;
                }
            }

            return maximum;
        }

        /// <summary>
        /// Calculates the minimum value of the subset consisting of the
        /// final number of values of a list of doubles
        /// </summary>
        /// <param name="values">The list to calculate the min for.</param>
        /// <param name="number">The final number of values to consider.</param>
        /// <returns>The minimum value.</returns>
        public static decimal Min(List<decimal> values, int number)
        {
            if (values == null)
            {
                return decimal.MaxValue;
            }

            if (values.Count < number)
            {
                return decimal.MaxValue;
            }

            decimal minimum = decimal.MaxValue;
            for (int index = 0; index < number; index++)
            {
                decimal latestVal = values[values.Count - 1 - index];
                if (latestVal < minimum)
                {
                    minimum = latestVal;
                }
            }

            return minimum;
        }

        /// <summary>
        /// Calculates the mean value of the subset consisting of the
        /// final number of values of a list of doubles
        /// </summary>
        /// <param name="values">The list to calculate the mean for.</param>
        /// <param name="number">The final number of values to consider.</param>
        /// <returns>The mean value.</returns>
        public static decimal Mean(List<decimal> values, int number)
        {
            if (values == null)
            {
                return decimal.MinValue;
            }

            if (values.Count < number)
            {
                return decimal.MinValue;
            }

            if (number.Equals(0))
            {
                return decimal.MinValue;
            }

            decimal sum = 0.0m;
            for (int index = 0; index < number; index++)
            {
                sum += values[values.Count - 1 - index];
            }

            return sum / number;
        }

        /// <summary>
        /// Calculates the variance value of the subset consisting of the
        /// final number of values of a list of doubles
        /// </summary>
        /// <param name="values">The list to calculate the variance for.</param>
        /// <param name="number">The final number of values to consider.</param>
        /// <returns>The variance.</returns>
        public static decimal Variance(List<decimal> values, int number)
        {
            if (values == null)
            {
                return decimal.MinValue;
            }

            if (values.Count < number)
            {
                return decimal.MinValue;
            }

            if (number <= 1)
            {
                return decimal.MinValue;
            }

            decimal mean = Mean(values, number);
            decimal sum = 0.0m;
            for (int index = 0; index < number; index++)
            {
                sum += (values[values.Count - 1 - index] - mean) * (values[values.Count - 1 - index] - mean);
            }

            return sum / (number - 1);
        }

        /// <summary>
        /// Calculates the standard deviation value of the subset consisting of the
        /// final number of values of a list of doubles
        /// </summary>
        /// <param name="values">The list to calculate the standard deviation for.</param>
        /// <param name="number">The final number of values to consider.</param>
        /// <returns>The standard deviation.</returns>
        public static decimal StandardDev(List<decimal> values, int number)
        {
            return Convert.ToDecimal(Math.Sqrt(Convert.ToDouble(Variance(values, number))));
        }

        /// <summary>
        /// Calculates the Sharpe ratio for a list of values.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public static decimal Sharpe(List<decimal> values, int number)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Calculates the Maximum draw down of a list of values.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public static decimal MDD(List<decimal> values, int number)
        {
            throw new NotImplementedException();
        }
    }
}
