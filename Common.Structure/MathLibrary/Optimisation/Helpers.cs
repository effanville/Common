using System;

namespace Common.Structure.MathLibrary.Optimisation
{
    public static class Helpers
    {
        public static void ShiftValues(ref double first, ref double second, ref double third, double d)
        {
            first = second;
            second = third;
            third = d;
        }

        public static double Sign(double a, double b)
        {
            return ((b) >= 0.0 ? Math.Abs(a) : -Math.Abs(a));
        }
    }
}
