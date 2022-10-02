namespace Common.Structure.MathLibrary.ParameterEstimation
{
    internal static class EstimatorHelpers
    {
        public static double Evaluate(double[] weights, double[] point)
        {
            if (weights.Length != point.Length)
            {
                return double.NaN;
            }

            double value = 0.0;
            for (int index = 0; index < weights.Length; index++)
            {
                value += weights[index] * point[index];
            }

            return value;
        }
    }
}
