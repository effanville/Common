using System;

namespace Common.Structure.MathLibrary.RootFinding
{
    public static class RootFindingHelpers
    {
        /// <summary>
        /// Enforces the sign of <paramref name="sign"/> onto the value of
        /// <paramref name="value"/>.
        /// </summary>
        public static double SIGN(double value, double sign)
        {
            if (sign > 0.0)
            {
                return Math.Abs(value);
            }
            else
            {
                return -1 * Math.Abs(value);
            }
        }
    }
}
