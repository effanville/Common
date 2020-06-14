namespace StructureCommon.MathLibrary.ParameterEstimation
{
    public static class LassoStatsCalculation
    {
        public static double VectorMatrixRowMult(double[,] matrix, double[] vector, int rowIndex)
        {
            if (matrix.GetLength(1) != vector.Length)
            {
                return double.NaN;
            }

            double sum = 0;
            for (int columnIndex = 0; columnIndex < vector.Length; columnIndex++)
            {
                sum += matrix[rowIndex, columnIndex] * vector[columnIndex];
            }

            return sum;
        }

        public static double VectorMatrixColumnMult(double[,] matrix, double[] vector, int columnIndex)
        {
            if (matrix.GetLength(0) != vector.Length)
            {
                return double.NaN;
            }

            double sum = 0;
            for (int rowIndex = 0; rowIndex < vector.Length; rowIndex++)
            {
                sum += matrix[rowIndex, columnIndex] * vector[rowIndex];
            }

            return sum;
        }
    }
}
