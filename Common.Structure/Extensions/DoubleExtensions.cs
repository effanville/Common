using System;

namespace Common.Structure.Extensions
{
    /// <summary>
    /// Miscellaneous custom extension functions for the <see cref="double"/> type.
    /// </summary>
    public static class DoubleExtensions
    {
        /// <summary>
        /// Truncates the double, and returns the output as a string.
        /// </summary>
        public static string TruncateToString(this double value, int exp = 2)
        {
            double decimalPlaces = Math.Pow(10, exp);
            return (Math.Truncate(value * decimalPlaces) / decimalPlaces).ToString();
        }

        /// <summary>
        /// Truncates the decimals of the double.
        /// </summary>
        public static double Truncate(this double value, int exp = 2)
        {
            double decimalPlaces = Math.Pow(10, exp);
            return (Math.Truncate(value * decimalPlaces) / decimalPlaces);
        }

        /// <summary>
        /// Compares two doubles <paramref name="value"/> and <paramref name="otherValue"/> up to a tolerance <paramref name="tol"/>.
        /// </summary>
        public static bool Equals(this double value, double otherValue, double tol)
        {
            if (Math.Abs(value - otherValue) < tol)
            {
                return true;
            }

            return false;
        }
    }
}
