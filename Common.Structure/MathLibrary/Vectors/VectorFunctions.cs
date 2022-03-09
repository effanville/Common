namespace Common.Structure.MathLibrary.Vectors
{
    public static class VectorFunctions
    {
        /// <summary>
        /// Calculates the dot product between two vectors.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static double DotProduct(double[] first, double[] second)
        {
            double product = 0.0;
            int length = first.Length;
            for (int dimensionIndex = 0; dimensionIndex < length; dimensionIndex++)
            {
                product += first[dimensionIndex] * second[dimensionIndex];
            }

            return product;
        }

        public static double[] Subtract(this double[] first, double[] second)
        {
            int length = first.Length;
            double[] difference = new double[length];
            for (int dimensionIndex = 0; dimensionIndex < length; dimensionIndex++)
            {
                difference[dimensionIndex] = first[dimensionIndex] - second[dimensionIndex];
            }

            return difference;
        }

        public static double[] Negative(this double[] first)
        {
            int length = first.Length;
            double[] lineSearchDirection = new double[length];
            for (int dimensionIndex = 0; dimensionIndex < length; dimensionIndex++)
            {
                lineSearchDirection[dimensionIndex] = -first[dimensionIndex];
            }

            return lineSearchDirection;
        }
    }
}
