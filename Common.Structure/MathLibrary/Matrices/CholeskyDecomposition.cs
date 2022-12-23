using System;

namespace Common.Structure.MathLibrary.Matrices
{
    /// <summary>
    /// Class to generate the CholeskyDecomposition of a matrix.
    /// This is only valid for a symmetric positive definite matrix.
    /// </summary>
    public sealed class CholeskyDecomposition
    {
        /// <summary>
        /// The lower triangular matrix in the decomposition. 
        /// </summary>
        public double[,] LowerDecomp
        {
            get;
        }

        /// <summary>
        /// Constructor given a matrix. This calculates the lower-upper decomposition.
        /// </summary>
        private CholeskyDecomposition(double[,] lower)
        {
            LowerDecomp = lower;
        }

        /// <summary>
        /// Generate the Cholesky decomposition of a matrix.
        /// </summary>
        public static Result<CholeskyDecomposition> Generate(double[,] matrix)
        {
            if (!Matrix<double>.IsSymmetric(matrix))
            {
                return Result.ErrorResult<CholeskyDecomposition>("Matrix is not symmetric");
            }

            return GenerateCholeskyBanachiewiczDecomp(matrix);
        }

        /// <summary>
        /// The upper triangular matrix in the decomposition.
        /// </summary>
        public double[,] LowerTranspose()
        {
            return Matrix<double>.Transpose(LowerDecomp);
        }

        /// <summary>
        /// Routine that generates the Cholesky decomposition.
        /// </summary>
        private static Result<CholeskyDecomposition> GenerateCholeskyBanachiewiczDecomp(double[,] matrix)
        {
            if (!matrix.GetLength(0).Equals(matrix.GetLength(1)))
            {
                return Result.ErrorResult<CholeskyDecomposition>("Matrix not square.");
            }

            int size = matrix.GetLength(0);
            double[,] lowerDecomp = new double[size, size];
            for (int rowIndex = 0; rowIndex < size; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex <= rowIndex; columnIndex++)
                {
                    double sum = 0;
                    for (int innerIndex = 0; innerIndex < columnIndex; innerIndex++)
                    {
                        sum += lowerDecomp[rowIndex, innerIndex] * lowerDecomp[columnIndex, innerIndex];
                    }

                    if (rowIndex == columnIndex)
                    {
                        if (matrix[rowIndex, columnIndex] < sum)
                        {
                            return Result.ErrorResult<CholeskyDecomposition>($"Cannot compute Cholesky decomposition. Sum for row {rowIndex} and column {columnIndex} is negative.");
                        }

                        lowerDecomp[rowIndex, columnIndex] = Math.Sqrt(matrix[rowIndex, columnIndex] - sum);
                    }
                    else
                    {
                        lowerDecomp[rowIndex, columnIndex] = (1.0 / lowerDecomp[columnIndex, columnIndex] * (matrix[rowIndex, columnIndex] - sum));
                    }
                }
            }

            return new CholeskyDecomposition(lowerDecomp);
        }

        /// <summary>
        /// Routine to solve the exquations Ax = b, where <paramref name="b"/> is provided here
        /// and A is the original matrix used in the constructor.
        /// </summary>
        public double[] LinearSolve(double[] b)
        {
            double sum;
            int size = LowerDecomp.GetLength(0);
            double[,] lowerTranspose = LowerTranspose();
            double[] solutionVector = new double[size];

            // solve L * y = b
            for (int rowIndex = 0; rowIndex <= size; rowIndex++)
            {
                sum = b[rowIndex];
                for (int columnIndex = 0; columnIndex < rowIndex; columnIndex++)
                {
                    sum -= LowerDecomp[rowIndex, columnIndex] * solutionVector[columnIndex];
                }

                solutionVector[rowIndex] = sum / LowerDecomp[rowIndex, rowIndex];
            }

            // solve L^T x = y
            for (int columnIndex = size; columnIndex >= 0; columnIndex--)
            {
                sum = solutionVector[columnIndex];
                for (int rowIndex = columnIndex + 1; rowIndex <= size; rowIndex++)
                {
                    sum -= lowerTranspose[rowIndex, columnIndex] * solutionVector[rowIndex];
                }

                solutionVector[columnIndex] = sum / LowerDecomp[columnIndex, columnIndex];
            }

            return solutionVector;
        }

        /// <summary>
        /// Calculates the inverse of the matrix in the generator.
        /// </summary>
        public double[,] Inverse()
        {
            int size = LowerDecomp.GetLength(0);
            double[,] lowerTranspose = LowerTranspose();
            double[,] inverse = new double[size, size];
            for (int columnIndex = 0; columnIndex < size; columnIndex++)
            {
                inverse[columnIndex, columnIndex] = 1.0 / LowerDecomp[columnIndex, columnIndex];
                for (int rowIndex = columnIndex + 1; rowIndex < size; rowIndex++)
                {
                    double sum = 0.0;
                    for (int k = 0; k < rowIndex; k++)
                    {
                        sum -= LowerDecomp[rowIndex, k] * lowerTranspose[k, columnIndex];
                    }

                    inverse[rowIndex, columnIndex] = sum / LowerDecomp[columnIndex, columnIndex];
                }
            }

            return inverse;
        }
    }
}
