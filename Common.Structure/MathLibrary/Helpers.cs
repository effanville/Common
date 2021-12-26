using System;

namespace Common.Structure.MathLibrary
{
    internal sealed class Helpers
    {
        /// <summary>
        /// Enforces the sign of <paramref name="sign"/> onto the value of
        /// <paramref name="value"/>.
        /// </summary>
        public static double Sign(double value, double sign)
        {
            return (sign >= 0.0 ? Math.Abs(value) : -Math.Abs(value));
        }
    }
}
