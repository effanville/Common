namespace Common.Structure.MathLibrary.Matrices
{
    /// <summary>
    /// Contains the lower-upper decomposition of a given matrix.
    /// </summary>
    public class LUDecomposition
    {
        private int Size;
        public double[,] UpperDecomp;

        public double[,] LowerDecomp;
        private int[] PivotValues;

        public bool Invertible = true;
        public bool Square = true;

        /// <summary>
        /// Constructor given a matrix. This calculates the lower-upper decomposition.
        /// </summary>
        public LUDecomposition(double[,] matrix)
        {
            GenerateLUDecomp(matrix);
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
                        sum -= LowerDecomp[vectorRowIndex, matrixColumnIndex] * y[matrixColumnIndex];
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
                    sum -= UpperDecomp[vectorRowIndex, matrixColumnIndex] * x[matrixColumnIndex];
                }

                x[vectorRowIndex] = sum / UpperDecomp[vectorRowIndex, vectorRowIndex];
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
        private void GenerateLUDecomp(double[,] matrix)
        {
            if (!matrix.GetLength(0).Equals(matrix.GetLength(1)))
            {
                Invertible = false;
                Square = false;
                return;
            }

            Size = matrix.GetLength(0);
            PivotValues = new int[Size];
            UpperDecomp = new double[Size, Size];
            LowerDecomp = new double[Size, Size];

            // Initialising the pivot values to the identity permutation
            for (int n = 0; n < Size; n++)
            {
                LowerDecomp[n, n] = 1;
                PivotValues[n] = n;
            }

            double sum = 0.0;

            for (int columnIndex = 0; columnIndex < Size; columnIndex++)
            {
                for (int upperRowIndex = 0; upperRowIndex < columnIndex + 1; upperRowIndex++)
                {
                    sum = matrix[upperRowIndex, columnIndex];
                    for (int k = 0; k < upperRowIndex; k++)
                    {
                        sum -= LowerDecomp[upperRowIndex, k] * UpperDecomp[k, columnIndex];
                    }

                    UpperDecomp[upperRowIndex, columnIndex] = sum;
                }

                if (UpperDecomp[columnIndex, columnIndex].Equals(0.0))
                {
                    Invertible = false;
                    return;
                }

                for (int lowerRowIndex = columnIndex + 1; lowerRowIndex < Size; lowerRowIndex++)
                {
                    sum = matrix[lowerRowIndex, columnIndex];
                    for (int k = 0; k < columnIndex; k++)
                    {
                        sum -= LowerDecomp[lowerRowIndex, k] * UpperDecomp[k, columnIndex];
                    }

                    LowerDecomp[lowerRowIndex, columnIndex] = sum / UpperDecomp[columnIndex, columnIndex];
                }
            }
        }
    }
}
