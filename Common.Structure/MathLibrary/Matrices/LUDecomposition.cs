namespace Common.Structure.MathLibrary.Matrices
{
    /// <summary>
    /// Contains the lower-upper decomposition of a given matrix.
    /// </summary>
    public sealed class LUDecomposition
    {
        private int Size => Lower.GetLength(0);

        /// <summary>
        /// The upper diagonal matrix in this decomposition.
        /// </summary>
        public double[,] Upper
        {
            get;
        }

        /// <summary>
        /// The lower diagonal matrix in this decomposition.
        /// </summary>
        public double[,] Lower
        {
            get;
        }

        /// <summary>
        /// The pivot values for this decomposition.
        /// </summary>
        public int[] PivotValues
        {
            get;
        }

        /// <summary>
        /// Constructor given a matrix. This calculates the lower-upper decomposition.
        /// </summary>
        private LUDecomposition(double[,] lower, double[,] upper, int[] pivots)
        {
            Lower = lower;
            Upper = upper;
            PivotValues = pivots;
        }

        /// <summary>
        /// Generate the LU Decomposition of a matrix.
        /// </summary>
        public static Result<LUDecomposition> Generate(double[,] matrix)
        {
            return GenerateLUDecomp(matrix);
        }

        /// <summary>
        /// Routine to solve the exquations Ax = b, where <paramref name="b"/> is provided here
        /// and A is the original matrix used in the constructor.
        /// </summary>
        public double[] LinearSolve(double[] b)
        {
            // This proceeds by first solving the equation Ly = b,
            // then solve Ux = y
            // x is then the output.
            double[] y = new double[Size];
            double[] x = new double[Size];
            int vectorRowIndex, firstNonZeroIndex = 0, pivotIndex, matrixColumnIndex;
            double sum;
            for (vectorRowIndex = 0; vectorRowIndex < Size; vectorRowIndex++)
            {
                pivotIndex = PivotValues[vectorRowIndex];
                sum = b[pivotIndex];
                if (firstNonZeroIndex > -1)
                {
                    for (matrixColumnIndex = firstNonZeroIndex; matrixColumnIndex < vectorRowIndex; matrixColumnIndex++)
                    {
                        sum -= Lower[vectorRowIndex, matrixColumnIndex] * y[matrixColumnIndex];
                    }
                }
                else if (sum > 0)
                {
                    firstNonZeroIndex = vectorRowIndex;
                }

                y[vectorRowIndex] = sum;
            }

            for (vectorRowIndex = Size - 1; vectorRowIndex > -1; vectorRowIndex--)
            {
                sum = y[vectorRowIndex];
                for (matrixColumnIndex = vectorRowIndex + 1; matrixColumnIndex < Size; matrixColumnIndex++)
                {
                    sum -= Upper[vectorRowIndex, matrixColumnIndex] * x[matrixColumnIndex];
                }

                x[vectorRowIndex] = sum / Upper[vectorRowIndex, vectorRowIndex];
            }

            return x;
        }

        /// <summary>
        /// Calculates the inverse of the matrix in the constructor.
        /// </summary>
        public double[,] Inverse()
        {
            double[,] inverse = new double[Size, Size];
            double[] col = new double[Size];
            for (int j = 0; j < Size; j++)
            {
                for (int i = 0; i < Size; i++)
                {
                    col[i] = 0;
                }

                col[j] = 1;
                col = LinearSolve(col);
                for (int i = 0; i < Size; i++)
                {
                    inverse[i, j] = col[i];
                }
            }

            return inverse;
        }

        /// <summary>
        /// Routine that generates the LU decomposition.
        /// Can only be called in the constructor.
        /// </summary>
        private static Result<LUDecomposition> GenerateLUDecomp(double[,] matrix)
        {
            if (!MatrixFunctions.IsSquare(matrix))
            {
                return Result.ErrorResult<LUDecomposition>("Matrix is not square.");
            }

            int size = matrix.GetLength(0);
            int[] pivotValues = new int[size];
            double[,] upper = new double[size, size];
            double[,] lower = new double[size, size];

            // Initialising the pivot values to the identity permutation
            for (int n = 0; n < size; n++)
            {
                lower[n, n] = 1;
                pivotValues[n] = n;
            }

            double sum;

            for (int columnIndex = 0; columnIndex < size; columnIndex++)
            {
                for (int upperRowIndex = 0; upperRowIndex < columnIndex + 1; upperRowIndex++)
                {
                    sum = matrix[upperRowIndex, columnIndex];
                    for (int k = 0; k < upperRowIndex; k++)
                    {
                        sum -= lower[upperRowIndex, k] * upper[k, columnIndex];
                    }

                    upper[upperRowIndex, columnIndex] = sum;
                }

                if (upper[columnIndex, columnIndex].Equals(0.0))
                {
                    return Result.ErrorResult<LUDecomposition>("Matrix is not invertible.");
                }

                for (int lowerRowIndex = columnIndex + 1; lowerRowIndex < size; lowerRowIndex++)
                {
                    sum = matrix[lowerRowIndex, columnIndex];
                    for (int k = 0; k < columnIndex; k++)
                    {
                        sum -= lower[lowerRowIndex, k] * upper[k, columnIndex];
                    }

                    lower[lowerRowIndex, columnIndex] = sum / upper[columnIndex, columnIndex];
                }
            }

            return new LUDecomposition(lower, upper, pivotValues);
        }
    }
}
