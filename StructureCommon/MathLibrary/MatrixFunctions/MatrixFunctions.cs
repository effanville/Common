namespace StructureCommon.MathLibrary.Matrices
{
    /// <summary>
    /// Implementations of matrix manipulations.
    /// </summary>
    public static class MatrixFunctions
    {
        /// <summary>
        /// Returns the Identity matrix of size <paramref name="n"/>.
        /// </summary>
        public static double[,] Identity(int n)
        {
            double[,] Id = new double[n, n];
            for (int index = 0; index < n; index++)
            {
                Id[index, index] = 1.0;
            }
            return Id;
        }

        /// <summary>
        /// Returns the transpose of the provided matrix.
        /// </summary>
        public static double[,] Transpose(this double[,] matrix)
        {
            if (matrix.GetLength(1).Equals(0) || matrix.GetLength(0).Equals(0))
            {
                return new double[0, 0];
            }
            double[,] transpose = new double[matrix.GetLength(1), matrix.GetLength(0)];
            for (int inputRowIndex = 0; inputRowIndex < matrix.GetLength(0); inputRowIndex++)
            {
                for (int inputColumnIndex = 0; inputColumnIndex < matrix.GetLength(1); inputColumnIndex++)
                {
                    transpose[inputColumnIndex, inputRowIndex] = matrix[inputRowIndex, inputColumnIndex];
                }
            }

            return transpose;
        }

        /// <summary>
        /// Multiplies the matrices together, in the order AB.
        /// </summary>
        /// <param name="firstMatrix">The first matrix A</param>
        /// <param name="secondMatrix">The second matrix B.</param>
        public static double[,] Multiply(this double[,] firstMatrix, double[,] secondMatrix)
        {
            if (!firstMatrix.GetLength(1).Equals(secondMatrix.GetLength(0)))
            {
                return new double[0, 0];
            }

            double[,] multiply = new double[firstMatrix.GetLength(0), secondMatrix.GetLength(0)];
            for (int firstMatrixRowIndex = 0; firstMatrixRowIndex < firstMatrix.GetLength(0); firstMatrixRowIndex++)
            {
                for (int secondMatrixColumnIndex = 0; secondMatrixColumnIndex < secondMatrix.GetLength(1); secondMatrixColumnIndex++)
                {
                    double thisIndexSum = 0.0;
                    for (int innerIndex = 0; innerIndex < firstMatrix.GetLength(1); innerIndex++)
                    {
                        thisIndexSum += firstMatrix[firstMatrixRowIndex, innerIndex] * secondMatrix[innerIndex, secondMatrixColumnIndex];
                    }

                    multiply[firstMatrixRowIndex, secondMatrixColumnIndex] = thisIndexSum;
                }
            }

            return multiply;
        }

        /// <summary>
        /// Returns the Matrix post multiplied by the vector.
        /// </summary>
        /// <param name="firstMatrix"></param>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static double[] PostMultiplyVector(this double[,] firstMatrix, double[] vector)
        {
            if (!firstMatrix.GetLength(1).Equals(vector.GetLength(0)))
            {
                return new double[0];
            }

            double[] multiply = new double[firstMatrix.GetLength(0)];
            for (int firstMatrixRowIndex = 0; firstMatrixRowIndex < firstMatrix.GetLength(0); firstMatrixRowIndex++)
            {
                double thisIndexSum = 0.0;
                for (int innerIndex = 0; innerIndex < firstMatrix.GetLength(1); innerIndex++)
                {
                    thisIndexSum += firstMatrix[firstMatrixRowIndex, innerIndex] * vector[innerIndex];
                }

                multiply[firstMatrixRowIndex] = thisIndexSum;
            }

            return multiply;
        }

        /// <summary>
        /// Returns the matrix transpose multiplied by itself, X^TX
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static double[,] XTX(this double[,] matrix)
        {

            double[,] multiply = new double[matrix.GetLength(1), matrix.GetLength(1)];
            for (int firstMatrixRowIndex = 0; firstMatrixRowIndex < matrix.GetLength(1); firstMatrixRowIndex++)
            {
                for (int secondMatrixColumnIndex = 0; secondMatrixColumnIndex < matrix.GetLength(1); secondMatrixColumnIndex++)
                {
                    double thisIndexSum = 0.0;
                    for (int innerIndex = 0; innerIndex < matrix.GetLength(0); innerIndex++)
                    {
                        thisIndexSum += matrix[innerIndex, firstMatrixRowIndex] * matrix[innerIndex, secondMatrixColumnIndex];
                    }

                    multiply[firstMatrixRowIndex, secondMatrixColumnIndex] = thisIndexSum;
                }
            }

            return multiply;
        }

        /// <summary>
        /// Returns the inverse of the matrix.
        /// </summary>
        /// <param name="matrix">The matrix to calculate the inverse of.</param>
        /// <returns></returns>
        public static double[,] Inverse(this double[,] matrix)
        {
            LUDecomposition decomp = new LUDecomposition(matrix);
            return decomp.Inverse();
        }
    }
}
