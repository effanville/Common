using System;
using System.Collections.Generic;

namespace Common.Structure.MathLibrary.Vectors
{
    /// <summary>
    /// Contains helper methods for calculating a statistic from a list.
    /// </summary>
    public static class VectorStats
    {
        /// <summary>
        /// Calculates the maximum value of the subset consisting of the
        /// final number of values of a list of doubles
        /// </summary>
        /// <param name="values">The list to calculate the max for.</param>
        /// <param name="number">The final number of values to consider.</param>
        /// <returns>The maximum value.</returns>
        public static double Max(List<double> values, int number)
        {
            if (values == null)
            {
                return double.NaN;
            }

            if (values.Count < number)
            {
                return double.NaN;
            }

            double maximum = double.MinValue;
            for (int index = 0; index < number; index++)
            {
                double latestVal = values[values.Count - 1 - index];
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
        public static double Min(List<double> values, int number)
        {
            if (values == null)
            {
                return double.NaN;
            }

            if (values.Count < number)
            {
                return double.NaN;
            }

            double minimum = double.MaxValue;
            for (int index = 0; index < number; index++)
            {
                double latestVal = values[values.Count - 1 - index];
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
        public static double Mean(List<double> values, int number)
        {
            if (values == null)
            {
                return double.NaN;
            }

            if (values.Count < number)
            {
                return double.NaN;
            }

            if (number.Equals(0))
            {
                return double.NaN;
            }

            double sum = 0.0;
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
        public static double Variance(List<double> values, int number)
        {
            if (values == null)
            {
                return double.NaN;
            }

            if (values.Count < number)
            {
                return double.NaN;
            }

            if (number <= 1)
            {
                return double.NaN;
            }

            double mean = Mean(values, number);
            double sum = 0.0;
            for (int index = 0; index < number; index++)
            {
                sum += Math.Pow(values[values.Count - 1 - index] - mean, 2.0);
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
        public static double StandardDev(List<double> values, int number)
        {
            return Math.Sqrt(Variance(values, number));
        }

        /// <summary>
        /// Calculates the Sharpe ratio for a list of values.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public static double Sharpe(List<double> values, int number)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Calculates the Maximum draw down of a list of values.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public static double MDD(List<double> values, int number)
        {
            throw new NotImplementedException();
        }
    }
}
