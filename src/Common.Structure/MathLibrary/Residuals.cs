namespace Common.Structure.MathLibrary
{
    /// <summary>
    /// Contains standard methods for calculating residual errors.
    /// </summary>
    public static class Residuals
    {
        /// <summary>
        /// The mean square error between two arrays.
        /// </summary>
        public static double MeanSquareError(double[] A, double[] B)
        {
            if (A.Length != B.Length)
            {
                return -1;
            }

            double sum = 0;
            for (int vectorIndex = 0; vectorIndex < A.Length; vectorIndex++)
            {
                double residual = (A[vectorIndex] - B[vectorIndex]);
                sum += residual * residual;
            }

            return sum;
        }
    }
}
