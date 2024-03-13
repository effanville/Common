using System;

namespace Effanville.Common.Structure.MathLibrary.Matrices
{
    /// <summary>
    /// Implementations of matrix manipulations.
    /// These consider the matrices to be dense, and generally loop 
    /// through rows and columns. Not performant.
    /// </summary>
    public static class MatrixExtensions
    {
        /// <summary>
        /// Returns the Identity matrix of size <paramref name="n"/>.
        /// </summary>
        [Obsolete("Should use other method in DoubleMatrix instead")]
        public static double[,] Identity(int n)
        {
            return DoubleMatrix.Identity(n);
        }

        /// <summary>
        /// Multiplies a specific row in a matrix by a vector.
        /// </summary>
        [Obsolete("Should use other method in DoubleMatrix instead")]
        public static double VectorMatrixRowMult(double[,] matrix, double[] vector, int rowIndex)
        {
            return DoubleMatrix.VectorMatrixRowMult(matrix, vector, rowIndex);
        }

        /// <summary>
        /// Returns the transpose of the provided matrix.
        /// </summary>
        public static double[,] Transpose(this double[,] matrix)
        {
            return Matrix<double>.Transpose(matrix);
        }

        /// <summary>
        /// Adds the matrices together.
        /// </summary>
        /// <param name="firstMatrix">The first matrix A</param>
        /// <param name="secondMatrix">The second matrix B.</param>
        public static double[,] Add(this double[,] firstMatrix, double[,] secondMatrix)
        {
            return DoubleMatrix.Add(firstMatrix, secondMatrix);
        }

        /// <summary>
        /// Multiplies the matrices together, in the order AB.
        /// </summary>
        /// <param name="firstMatrix">The first matrix A</param>
        /// <param name="secondMatrix">The second matrix B.</param>
        public static double[,] Multiply(this double[,] firstMatrix, double[,] secondMatrix)
        {
            return DoubleMatrix.Multiply(firstMatrix, secondMatrix);
        }

        /// <summary>
        /// Returns the Matrix post multiplied by the vector.
        /// </summary>
        public static double[] PostMultiplyVector(this double[,] firstMatrix, double[] vector)
        {
            return DoubleMatrix.PostMultiplyVector(firstMatrix, vector);
        }

        /// <summary>
        /// Multiplies a matrix by a scalar value.
        /// </summary>
        public static double[,] ScalarMult(this double[,] matrix, double scalar)
        {
            return DoubleMatrix.ScalarMult(matrix, scalar);
        }

        /// <summary>
        /// Returns the matrix transpose multiplied by itself, X^TX
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static double[,] XTX(this double[,] matrix)
        {
            return DoubleMatrix.XTX(matrix);
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
            return DoubleMatrix.XTXPlusI(matrix, lambda);
        }

        /// <summary>
        /// Returns the inverse of the matrix.
        /// </summary>
        /// <param name="matrix">The matrix to calculate the inverse of.</param>
        /// <returns></returns>
        public static double[,] Inverse(this double[,] matrix)
        {
            return DoubleMatrix.Inverse(matrix);
        }
    }
}
