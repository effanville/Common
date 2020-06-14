using System;

namespace StructureCommon.MathLibrary
{
    public static class Residuals
    {
        public static double MeanSquareError(double[] A, double[] B)
        {
            if (A.Length != B.Length)
            {
                return -1;
            }

            double sum = 0;
            for (int vectorIndex = 0; vectorIndex < A.Length; vectorIndex++)
            {
                sum += Math.Pow(A[vectorIndex] - B[vectorIndex], 2);
            }
            return sum;
        }
    }
}
