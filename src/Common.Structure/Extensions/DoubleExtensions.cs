using System;

namespace Effanville.Common.Structure.Extensions
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
        /// Compares two doubles up to but not including a tolerance.
        /// </summary>
        /// <param name="value">The main value to compare with.</param>
        /// <param name="otherValue">The other value to compare to.</param>
        /// <param name="tol">The tolerance to compare with.</param>
        public static bool Equals(this double value, double otherValue, double tol)
        {
            if (tol.Equals(0.0))
            {
                return value.Equals(otherValue);
            }

            if (Math.Abs(value - otherValue) < tol)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Compares two doubles up to but not including a tolerance 
        /// and with the possibility of a relative comparison.
        /// </summary>
        /// <param name="value">The main value to compare with.</param>
        /// <param name="otherValue">The other value to compare to.</param>
        /// <param name="tol">The tolerance to compare with.</param>
        /// <param name="isRelative">Should the comparison be relative.</param>
        public static bool Equals(this double value, double otherValue, double tol, bool isRelative)
        {
            if (tol.Equals(0.0))
            {
                return value.Equals(otherValue);
            }

            if (!isRelative || value.Equals(0.0))
            {
                return value.Equals(otherValue, tol);
            }

            if (Math.Min(Math.Abs(value - otherValue), Math.Abs(value - otherValue) / value) < tol)
            {
                return true;
            }

            return false;
        }
    }
}
