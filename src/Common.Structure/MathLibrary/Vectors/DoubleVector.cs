using System;
using System.Collections.Generic;

namespace Effanville.Common.Structure.MathLibrary.Vectors
{
    /// <summary>
    /// A vector of <see cref="double"/> values.
    /// </summary>
    public sealed class DoubleVector : Vector<double>
    {
        /// <summary>
        /// Construct an instance of a <see cref="DoubleVector"/>
        /// </summary>
        public DoubleVector(double[] values)
            : base(values)
        {
            DefaultValue = double.NaN;
        }

        /// <summary>
        /// Construct an instance of a <see cref="DoubleVector"/>
        /// </summary>
        public DoubleVector(int size, double defaultValue)
            : base(size, defaultValue)
        {
            DefaultValue = double.NaN;
        }

        /// <summary>
        /// The addition operator for two doublevectors.
        /// </summary>
        public static DoubleVector operator +(DoubleVector a, DoubleVector b)
        {
            if (a.Count != b.Count)
            {
                throw new Exception("Cannot add vectors of differing length");
            }

            double[] resultArray = new double[a.Count];
            for (int index = 0; index < a.Count; index++)
            {
                resultArray[index] = a[index] + b[index];
            }

            return new DoubleVector(resultArray);
        }

        /// <summary>
        /// The subtraction operator for two doublevectors.
        /// </summary>
        public static DoubleVector operator -(DoubleVector a, DoubleVector b)
        {
            if (a.Count != b.Count)
            {
                throw new Exception("Cannot subtract vectors of differing length");
            }

            double[] resultArray = new double[a.Count];
            for (int index = 0; index < a.Count; index++)
            {
                resultArray[index] = a[index] - b[index];
            }

            return new DoubleVector(resultArray);
        }

        /// <summary>
        /// The dot product operator for two doublevectors.
        /// </summary>
        public static double operator *(DoubleVector a, DoubleVector b)
        {
            if (a.Count != b.Count)
            {
                throw new Exception("Cannot subtract vectors of differing length");
            }

            double product = 0.0;
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
        public double Mean(int number)
        {
            return Mean(_values, number, DefaultValue);
        }

        /// <summary>
        /// Calculates the mean value of the subset consisting of the
        /// final number of values of a list of doubles
        /// </summary>
        /// <param name="values">The list to calculate the mean for.</param>
        /// <param name="number">The final number of values to consider.</param>
        /// <param name="defaultValue">The default value to use in case of insufficient data.</param>
        /// <returns>The mean value.</returns>
        public static double Mean(IReadOnlyList<double> values, int number, double defaultValue = double.NaN)
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
        /// <param name="number">The final number of values to consider.</param>
        /// <returns>The variance.</returns>
        public double Variance(int number)
        {
            return Variance(_values, number, DefaultValue);
        }

        /// <summary>
        /// Calculates the variance value of the subset consisting of the
        /// final number of values of a list of doubles
        /// </summary>
        /// <param name="values">The list to calculate the variance for.</param>
        /// <param name="number">The final number of values to consider.</param>
        /// <param name="defaultValue">The default value to use in case of insufficient data.</param>
        /// <returns>The variance.</returns>
        public static double Variance(IReadOnlyList<double> values, int number, double defaultValue = double.NaN)
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

            double mean = Mean(values, number, defaultValue);
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
        /// <param name="number">The final number of values to consider.</param>
        /// <returns>The standard deviation.</returns>
        public double StandardDev(int number)
        {
            return StandardDev(_values, number, DefaultValue);
        }

        /// <summary>
        /// Calculates the standard deviation value of the subset consisting of the
        /// final number of values of a list of doubles
        /// </summary>
        /// <param name="values">The list to calculate the standard deviation for.</param>
        /// <param name="number">The final number of values to consider.</param>
        /// <param name="defaultValue">The default value to use in case of insufficient data.</param>
        /// <returns>The standard deviation.</returns>
        public static double StandardDev(IReadOnlyList<double> values, int number, double defaultValue = double.NaN)
        {
            return Math.Sqrt(Variance(values, number, defaultValue));
        }

        /// <summary>
        /// Calculates the Sharpe ratio for a list of values.
        /// </summary>
        public double Sharpe(int number, double riskFreeRate = 0.0)
        {
            return Sharpe(_values, number, riskFreeRate);
        }

        /// <summary>
        /// Calculates the Sharpe ratio for a list of values.
        /// </summary>
        public static double Sharpe(IReadOnlyList<double> values, int number, double riskFreeRate = 0.0)
        {
            return (Mean(values, number) - riskFreeRate) / StandardDev(values, number);
        }

        /// <summary>
        /// Calculates the Maximum draw down of a list of values.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public static double MDD(IReadOnlyList<double> values, int number)
        {
            throw new NotImplementedException();
        }
    }
}
