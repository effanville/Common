namespace Common.Structure.MathLibrary.Optimisation
{
    public static class Helpers
    {
        public static void ShiftValues(ref double first, ref double second, double d)
        {
            first = second;
            second = d;
        }

        public static void ShiftValues(ref double first, ref double second, ref double third, double d)
        {
            first = second;
            second = third;
            third = d;
        }
    }
}
