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
        [Obsolete("Should use other method in DoubleVector instead")]
        public static decimal Max(List<decimal> values, int number)
        {
            return DecimalVector.Max(values, number, decimal.MinValue);
        }

        /// <summary>
        /// Calculates the minimum value of the subset consisting of the
        /// final number of values of a list of doubles
        /// </summary>
        /// <param name="values">The list to calculate the min for.</param>
        /// <param name="number">The final number of values to consider.</param>
        /// <returns>The minimum value.</returns>
        [Obsolete("Should use other method in DecimalVector instead")]
        public static decimal Min(List<decimal> values, int number)
        {
            return DecimalVector .Min(values, number, decimal.MaxValue);
        }

        /// <summary>
        /// Calculates the mean value of the subset consisting of the
        /// final number of values of a list of doubles
        /// </summary>
        /// <param name="values">The list to calculate the mean for.</param>
        /// <param name="number">The final number of values to consider.</param>
        /// <returns>The mean value.</returns>
        [Obsolete("Should use other method in DecimalVector instead")]
        public static decimal Mean(List<decimal> values, int number)
        {
            return DecimalVector.Mean(values, number);
        }

        /// <summary>
        /// Calculates the variance value of the subset consisting of the
        /// final number of values of a list of doubles
        /// </summary>
        /// <param name="values">The list to calculate the variance for.</param>
        /// <param name="number">The final number of values to consider.</param>
        /// <returns>The variance.</returns>
        [Obsolete("Should use other method in DecimalVector instead")]
        public static decimal Variance(List<decimal> values, int number)
        {
            return DecimalVector.Variance(values, number);
        }

        /// <summary>
        /// Calculates the standard deviation value of the subset consisting of the
        /// final number of values of a list of doubles
        /// </summary>
        /// <param name="values">The list to calculate the standard deviation for.</param>
        /// <param name="number">The final number of values to consider.</param>
        /// <returns>The standard deviation.</returns>
        [Obsolete("Should use other method in DecimalVector instead")]
        public static decimal StandardDev(List<decimal> values, int number)
        {
            return Convert.ToDecimal(Math.Sqrt(Convert.ToDouble(Variance(values, number))));
        }
    }
}
