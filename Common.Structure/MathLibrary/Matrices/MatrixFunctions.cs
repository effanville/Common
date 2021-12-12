namespace Common.Structure.MathLibrary.Matrices
{
    /// <summary>
    /// Implementations of matrix manipulations.
    /// These consider the matrices to be dense, and generally loop 
    /// through rows and columns. Not performant.
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
        /// Adds the matrices together.
        /// </summary>
        /// <param name="firstMatrix">The first matrix A</param>
        /// <param name="secondMatrix">The second matrix B.</param>
        public static double[,] Add(this double[,] firstMatrix, double[,] secondMatrix)
        {
            if (!firstMatrix.GetLength(1).Equals(secondMatrix.GetLength(0)))
            {
                return new double[0, 0];
            }

            double[,] multiply = new double[firstMatrix.GetLength(0), secondMatrix.GetLength(0)];
            for (int firstMatrixRowIndex = 0; firstMatrixRowIndex < firstMatrix.GetLength(0); firstMatrixRowIndex++)
            {
                for (int firstMatrixColumnIndex = 0; firstMatrixColumnIndex < secondMatrix.GetLength(1); firstMatrixColumnIndex++)
                {
                    multiply[firstMatrixRowIndex, firstMatrixColumnIndex] = firstMatrix[firstMatrixRowIndex, firstMatrixColumnIndex] + secondMatrix[firstMatrixRowIndex, firstMatrixColumnIndex];
                }
            }

            return multiply;
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

        public static double[,] ScalarMult(this double[,] matrix, double scalar)
        {
            double[,] multiply = new double[matrix.GetLength(0), matrix.GetLength(1)];
            for (int matrixRowIndex = 0; matrixRowIndex < matrix.GetLength(0); matrixRowIndex++)
            {
                for (int matrixColumnIndex = 0; matrixColumnIndex < matrix.GetLength(1); matrixColumnIndex++)
                {
                    multiply[matrixRowIndex, matrixColumnIndex] = matrix[matrixRowIndex, matrixColumnIndex] * scalar;
                }
            }

            return multiply;
        }

        /// <summary>
        /// Multiplies a specific row in a matrix by a vector.
        /// </summary>
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

        /// <summary>
        /// Multiples a specific column in a matrix by a vector.
        /// </summary>
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
        /// Calculates X^TX + lI in a manner requiring one iteration through all matrix 
        /// values.
        /// </summary>
        /// <param name="matrix">The matrix X</param>
        /// <param name="lambda">The value to add to the diagonal.</param>
        /// <returns></returns>
        public static double[,] XTXPlusI(this double[,] matrix, double lambda)
        {
            double[,] output = new double[matrix.GetLength(1), matrix.GetLength(1)];
            for (int i = 0; i < matrix.GetLength(1); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    for (int k = 0; k < matrix.GetLength(0); k++)
                    {
                        output[i, j] += matrix[k, i] * matrix[k, j];
                    }
                    // now add identity element to diagonal
                    if (i.Equals(j))
                    {
                        output[i, j] += lambda;
                    }
                }
            }

            return output;
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

        /// <summary>
        /// Routine to check whether the input is symmetric.
        /// </summary>
        public static bool IsSymmetric(double[,] matrix)
        {
            if (!matrix.GetLength(0).Equals(matrix.GetLength(1)))
            {
                return false;
            }

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < i; j++)
                {
                    if (matrix[i, j] != matrix[j, i])
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
