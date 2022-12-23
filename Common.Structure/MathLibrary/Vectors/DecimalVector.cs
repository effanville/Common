using System;
using System.Collections.Generic;

namespace Common.Structure.MathLibrary.Vectors
{
    public sealed class DecimalVector : Vector<decimal>
    {
        public DecimalVector(decimal[] values) : base(values)
        {
        }

        public DecimalVector(int size, decimal defaultValue) : base(size, defaultValue)
        {
        }

        public static DecimalVector operator +(DecimalVector a, DecimalVector b)
        {
            if (a.Count != b.Count)
            {
                throw new Exception("Cannot add vectors of differing length");
            }

            decimal[] resultArray = new decimal[a.Count];
            for (int index = 0; index < a.Count; index++)
            {
                resultArray[index] = a[index] + b[index];
            }

            return new DecimalVector(resultArray);
        }

        public static DecimalVector operator -(DecimalVector a, DecimalVector b)
        {
            if (a.Count != b.Count)
            {
                throw new Exception("Cannot subtract vectors of differing length");
            }

            decimal[] resultArray = new decimal[a.Count];
            for (int index = 0; index < a.Count; index++)
            {
                resultArray[index] = a[index] - b[index];
            }

            return new DecimalVector(resultArray);
        }

        public static decimal operator *(DecimalVector a, DecimalVector b)
        {
            if (a.Count != b.Count)
            {
                throw new Exception("Cannot subtract vectors of differing length");
            }

            decimal product = 0.0m;
            int length = a.Count;
            for (int dimensionIndex = 0; dimensionIndex < length; dimensionIndex++)
            {
                product += a[dimensionIndex] * a[dimensionIndex];
            }

            return product;
        }

        /// <summary>
        /// Calculates the mean value of the subset consisting of the
        /// final number of values of a list of doubles
        /// </summary>
        /// <param name="number">The final number of values to consider.</param>
        /// <returns>The mean value.</returns>
        public decimal Mean(int number)
        {
            return Mean(_values, number, DefaultValue);
        }

        /// <summary>
        /// Calculates the mean value of the subset consisting of the
        /// final number of values of a list of doubles
        /// </summary>
        /// <param name="values">The list to calculate the mean for.</param>
        /// <param name="number">The final number of values to consider.</param>
        /// <returns>The mean value.</returns>
        public static decimal Mean(IReadOnlyList<decimal> values, int number, decimal defaultValue = decimal.MinValue)
        {
            if (values == null)
            {
                return defaultValue;
            }

            if (values.Count < number)
            {
                return defaultValue;
            }

            if (number.Equals(0))
            {
                return defaultValue;
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
        /// <param name="number">The final number of values to consider.</param>
        /// <returns>The variance.</returns>
        public decimal Variance(int number)
        {
            return Variance(_values, number, DefaultValue);
        }

        /// <summary>
        /// Calculates the variance value of the subset consisting of the
        /// final number of values of a list of doubles
        /// </summary>
        /// <param name="values">The list to calculate the variance for.</param>
        /// <param name="number">The final number of values to consider.</param>
        /// <returns>The variance.</returns>
        public static decimal Variance(IReadOnlyList<decimal> values, int number, decimal defaultValue = decimal.MinValue)
        {
            if (values == null)
            {
                return defaultValue;
            }

            if (values.Count < number)
            {
                return defaultValue;
            }

            if (number <= 1)
            {
                return defaultValue;
            }

            decimal mean = Mean(values, number, defaultValue);
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
        /// <param name="number">The final number of values to consider.</param>
        /// <returns>The standard deviation.</returns>
        public decimal StandardDev(int number)
        {
            return StandardDev(_values, number, DefaultValue);
        }

        /// <summary>
        /// Calculates the standard deviation value of the subset consisting of the
        /// final number of values of a list of doubles
        /// </summary>
        /// <param name="values">The list to calculate the standard deviation for.</param>
        /// <param name="number">The final number of values to consider.</param>
        /// <returns>The standard deviation.</returns>
        public static decimal StandardDev(IReadOnlyList<decimal> values, int number, decimal defaultValue = decimal.MinValue)
        {
            return Convert.ToDecimal(Math.Sqrt(Convert.ToDouble(Variance(values, number, defaultValue))));
        }
    }
}
