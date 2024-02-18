namespace Effanville.Common.Structure.MathLibrary.ParameterEstimation
{
    internal static class EstimatorHelpers
    {
        /// <summary>
        /// Standard Evaluation mechanism for a vector of weights at a point.
        /// </summary>
        internal static double Evaluate(double[] weights, double[] point)
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
