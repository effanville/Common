using System;

using Common.Structure.MathLibrary.Vectors;

namespace Common.Structure.MathLibrary.Matrices
{
    /// <summary>
    /// Contains a representation of a matrix of double values.
    /// </summary>
    public sealed class DoubleMatrix : Matrix<double>
    {
        /// <summary>
        /// Construct an instance
        /// </summary>
        public DoubleMatrix(double[,] values, double defaultValue)
            : base(values, defaultValue)
        {
        }

        /// <summary>
        /// Construct an instance
        /// </summary>
        public DoubleMatrix(double[,] values)
            : base(values)
        {
        }

        /// <summary>
        /// Construct an instance
        /// </summary>
        public DoubleMatrix(int size1, int size2, double defaultValue)
            : base(size1, size2, defaultValue)
        {
        }

        /// <summary>
        /// Transpose the matrix
        /// </summary>
        public override DoubleMatrix Transpose()
        {
            double[,] transpose = Transpose(_values);
            return new DoubleMatrix(transpose);
        }

        /// <summary>
        /// Creates an identity matrix of the given size.
        /// </summary>
        public static DoubleMatrix IdentityMatrix(int n)
        {
            return new DoubleMatrix(Identity(n));
        }

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
        /// Adds two matrices together. They must be of the same size.
        /// </summary>
        public static DoubleMatrix operator +(DoubleMatrix a, DoubleMatrix b)
        {
            if (a.Count(0) != b.Count(0) || a.Count(1) != b.Count(1))
            {
                throw new Exception("Cannot add matrices of differing size");
            }

            return new DoubleMatrix(Add(a._values, b._values));
        }

        /// <summary>
        /// Adds the matrices together.
        /// </summary>
        /// <param name="a">The first matrix A</param>
        /// <param name="b">The second matrix B.</param>
        public static double[,] Add(double[,] a, double[,] b)
        {
            if (!a.GetLength(0).Equals(b.GetLength(0))
                || !a.GetLength(1).Equals(b.GetLength(1)))
            {
                return new double[0, 0];
            }

            double[,] multiply = new double[a.GetLength(0), b.GetLength(1)];
            for (int aRowIndex = 0; aRowIndex < a.GetLength(0); aRowIndex++)
            {
                for (int aColumnIndex = 0; aColumnIndex < b.GetLength(1); aColumnIndex++)
                {
                    multiply[aRowIndex, aColumnIndex] = a[aRowIndex, aColumnIndex] + b[aRowIndex, aColumnIndex];
                }
            }

            return multiply;
        }

        /// <summary>
        /// Subtracts two matrices. They must be of the same size.
        /// </summary>
        public static DoubleMatrix operator -(DoubleMatrix a, DoubleMatrix b)
        {
            if (a.Count(0) != b.Count(0) || a.Count(1) != b.Count(1))
            {
                throw new Exception("Cannot subtract matrices of differing size");
            }

            return new DoubleMatrix(Subtract(a._values, b._values));
        }

        /// <summary>
        /// Adds the matrices together.
        /// </summary>
        /// <param name="a">The first matrix A</param>
        /// <param name="b">The second matrix B.</param>
        public static double[,] Subtract(double[,] a, double[,] b)
        {
            if (!a.GetLength(0).Equals(b.GetLength(0))
                || !a.GetLength(1).Equals(b.GetLength(1)))
            {
                return new double[0, 0];
            }

            double[,] multiply = new double[a.GetLength(0), b.GetLength(1)];
            for (int aRowIndex = 0; aRowIndex < a.GetLength(0); aRowIndex++)
            {
                for (int aColumnIndex = 0; aColumnIndex < b.GetLength(1); aColumnIndex++)
                {
                    multiply[aRowIndex, aColumnIndex] = a[aRowIndex, aColumnIndex] - b[aRowIndex, aColumnIndex];
                }
            }

            return multiply;
        }

        /// <summary>
        /// Multiplies the matrices together, in the order AB.
        /// </summary>
        public static DoubleMatrix operator *(DoubleMatrix a, DoubleMatrix b)
        {
            int bLength0 = b.Count(0);
            int aLength1 = a.Count(1);
            if (!aLength1.Equals(bLength0))
            {
                return new DoubleMatrix(0, 0, a.DefaultValue);
            }

            double[,] multiply = Multiply(a._values, b._values);

            return new DoubleMatrix(multiply);
        }

        /// <summary>
        /// Multiplies the matrices together, in the order AB.
        /// </summary>
        /// <param name="a">The first matrix A</param>
        /// <param name="b">The second matrix B.</param>
        public static double[,] Multiply(double[,] a, double[,] b)
        {
            int aLength0 = a.GetLength(0);
            int bLength0 = b.GetLength(0);
            int aLength1 = a.GetLength(1);
            int bLength1 = b.GetLength(1);
            if (!aLength1.Equals(bLength0))
            {
                return new double[0, 0];
            }

            double[,] multiply = new double[aLength0, bLength0];
            for (int aRowIndex = 0; aRowIndex < aLength0; aRowIndex++)
            {
                for (int bColumnIndex = 0; bColumnIndex < bLength1; bColumnIndex++)
                {
                    double thisIndexSum = 0.0;
                    for (int innerIndex = 0; innerIndex < aLength1; innerIndex++)
                    {
                        thisIndexSum += a[aRowIndex, innerIndex] * b[innerIndex, bColumnIndex];
                    }

                    multiply[aRowIndex, bColumnIndex] = thisIndexSum;
                }
            }

            return multiply;
        }

        /// <summary>
        /// Returns the Matrix post multiplied by the vector.
        /// </summary>
        public static DoubleVector operator *(DoubleMatrix a, DoubleVector b)
        {
            int bLength = b.Count;
            int aLength1 = a.Count(1);
            if (!aLength1.Equals(bLength))
            {
                return new DoubleVector(0, a.DefaultValue);
            }

            double[] multiply = PostMultiplyVector(a._values, b.Values);
            return new DoubleVector(multiply);
        }

        /// <summary>
        /// Returns the Matrix post multiplied by the vector.
        /// </summary>
        public static double[] PostMultiplyVector(double[,] firstMatrix, double[] vector)
        {
            if (!firstMatrix.GetLength(1).Equals(vector.GetLength(0)))
            {
                return Array.Empty<double>();
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
        /// Multiplies a matrix by a scalar value.
        /// </summary>
        public static DoubleMatrix operator *(DoubleMatrix matrix, double scalar)
        {
            double[,] multiply = ScalarMult(matrix._values, scalar);
            return new DoubleMatrix(multiply);
        }

        /// <summary>
        /// Multiplies a matrix by a scalar value.
        /// </summary>
        public static double[,] ScalarMult(double[,] matrix, double scalar)
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
        /// Multiplies a matrix by a scalar value.
        /// </summary>
        public static DoubleMatrix operator *(double scalar, DoubleMatrix matrix)
        {
            return matrix * scalar;
        }

        /// <summary>
        /// Multiplies a specific row in a matrix by a vector.
        /// </summary>
        public double VectorMatrixRowMult(DoubleVector vector, int rowIndex)
        {
            return VectorMatrixRowMult(_values, vector.Values, rowIndex);
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
        /// Calculates X^T*X for the given matrix X.
        /// </summary>
        public DoubleMatrix XTX()
        {
            return new DoubleMatrix(XTX(_values));
        }

        /// <summary>
        /// Returns the matrix transpose multiplied by itself, X^TX
        /// </summary>
        public static double[,] XTX(double[,] matrix)
        {
            double[,] multiply = new double[matrix.GetLength(1), matrix.GetLength(1)];
            for (int rowIndex = 0; rowIndex < matrix.GetLength(1); rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < matrix.GetLength(1); columnIndex++)
                {
                    double thisIndexSum = 0.0;
                    for (int innerIndex = 0; innerIndex < matrix.GetLength(0); innerIndex++)
                    {
                        thisIndexSum += matrix[innerIndex, rowIndex] * matrix[innerIndex, columnIndex];
                    }

                    multiply[rowIndex, columnIndex] = thisIndexSum;
                }
            }

            return multiply;
        }

        /// <summary>
        /// Calculates X^TX + lI in a manner requiring one iteration through all matrix 
        /// values.
        /// </summary>
        public DoubleMatrix XTXPlusI(double lambda)
        {
            return new DoubleMatrix(XTXPlusI(_values, lambda));
        }

        /// <summary>
        /// Calculates X^TX + lI in a manner requiring one iteration through all matrix 
        /// values.
        /// </summary>
        /// <param name="matrix">The matrix X</param>
        /// <param name="lambda">The value to add to the diagonal.</param>
        public static double[,] XTXPlusI(double[,] matrix, double lambda)
        {
            double[,] output = new double[matrix.GetLength(1), matrix.GetLength(1)];
            for (int rowIndex = 0; rowIndex < matrix.GetLength(1); rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < matrix.GetLength(1); columnIndex++)
                {
                    for (int innerIndex = 0; innerIndex < matrix.GetLength(0); innerIndex++)
                    {
                        output[rowIndex, columnIndex] += matrix[innerIndex, rowIndex] * matrix[innerIndex, columnIndex];
                    }

                    // now add identity element to diagonal
                    if (rowIndex.Equals(columnIndex))
                    {
                        output[rowIndex, columnIndex] += lambda;
                    }
                }
            }

            return output;
        }

        /// <summary>
        /// Returns the inverse of the matrix.
        /// </summary>
        public DoubleMatrix Inverse()
        {
            double[,] decomp = Inverse(_values);
            if (decomp == null)
            {
                return null;
            }

            return new DoubleMatrix(decomp);
        }

        /// <summary>
        /// Returns the inverse of the matrix.
        /// </summary>
        /// <param name="matrix">The matrix to calculate the inverse of.</param>
        /// <returns></returns>
        public static double[,] Inverse(double[,] matrix)
        {
            var decomp = LUDecomposition.Generate(matrix);
            if (decomp.IsError())
            {
                return null;
            }

            return decomp.Value.Inverse();
        }
    }
}
