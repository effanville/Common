using System;

namespace Common.Structure.Extensions
{
    public static class DecimalExtensions
    {
        /// <summary>
        /// Truncates the double, and returns the output as a string.
        /// </summary>
        public static string TruncateToString(this decimal value, int exp = 2)
        {
            decimal decimalPlaces = Pow(10, exp);
            return (Math.Truncate(value * decimalPlaces) / decimalPlaces).ToString();
        }

        /// <summary>
        /// Truncates the decimals of the double.
        /// </summary>
        public static decimal Truncate(this decimal value, int exp = 2)
        {
            decimal decimalPlaces = Pow(10, exp);
            return (Math.Truncate(value * decimalPlaces) / decimalPlaces);
        }

        /// <summary>
        /// Compares two doubles <paramref name="value"/> and <paramref name="otherValue"/> up to a tolerance <paramref name="tol"/>.
        /// </summary>
        public static bool Equals(this decimal value, decimal otherValue, decimal tol)
        {
            if (tol.Equals(0.0m))
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
        public static bool Equals(this decimal value, decimal otherValue, decimal tol, bool isRelative)
        {
            if (tol.Equals(0.0m))
            {
                return value.Equals(otherValue);
            }

            if (!isRelative || value.Equals(0.0m))
            {
                return value.Equals(otherValue, tol);
            }

            if (Math.Min(Math.Abs(value - otherValue), Math.Abs(value - otherValue) / value) < tol)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Raise a decimal to a non negative integral power.
        /// </summary>
        /// <param name="value">The decimal to raise to a power.</param>
        /// <param name="exp">The non negative integer power.</param>
        /// <returns>value^ exp</returns>
        /// <exception cref="ArgumentOutOfRangeException">An argument exception if exp is negative.</exception>
        public static decimal Pow(this decimal value, int exp)
        {
            if (exp < 0)
            {
                throw new ArgumentOutOfRangeException($"{nameof(exp)}");
            }

            decimal result = 1;

            for (int i = 0; i < exp; i++)
            {
                result *= value;
            }

            return result;
        }

        /// <summary>
        /// Raise a decimal to a non negative integral power with an alternative method.
        /// </summary>
        /// <param name="x">The decimal to raise to a power.</param>
        /// <param name="n">The non negative integer power.</param>
        /// <returns>value^ exp</returns>
        /// <exception cref="ArgumentOutOfRangeException">An argument exception if exp is negative.</exception>
        public static decimal PowFancy(decimal x, int n)
        {
            if (n < 0)
            {
                throw new ArgumentOutOfRangeException($"{nameof(n)}");
            }

            decimal result = 1;
            decimal multiplier = x;

            while (n > 0)
            {
                if ((n & 1) > 0)
                {
                    result *= multiplier;
                }

                multiplier *= multiplier;
                n >>= 1;
            }

            return result;
        }
    }
}
