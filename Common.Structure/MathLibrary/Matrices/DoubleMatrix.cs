using System;

using Common.Structure.MathLibrary.Vectors;

namespace Common.Structure.MathLibrary.Matrices
{
    public sealed class DoubleMatrix : Matrix<double>
    {
        public DoubleMatrix(double[,] values)
            : base(values)
        {
        }

        public DoubleMatrix(int size1, int size2, double defaultValue)
            : base(size1, size2, defaultValue)
        {
        }

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

        public static DoubleMatrix operator +(DoubleMatrix a, DoubleMatrix b)
        {
            if (a.Count(0) != b.Count(0) || a.Count(1) != b.Count(1))
            {
                throw new Exception("Cannot add matrices of differing size");
            }

            double[,] resultArray = new double[a.Count(0), a.Count(1)];
            for (int index = 0; index < a.Count(0); index++)
            {
                for (int index2 = 0; index2 < a.Count(1); index2++)
                {
                    resultArray[index, index2] = a[index, index2] + b[index, index2];
                }
            }

            return new DoubleMatrix(resultArray);
        }

        public static DoubleMatrix operator -(DoubleMatrix a, DoubleMatrix b)
        {
            if (a.Count(0) != b.Count(0) || a.Count(1) != b.Count(1))
            {
                throw new Exception("Cannot subtract matrices of differing size");
            }

            double[,] resultArray = new double[a.Count(0), a.Count(1)];
            for (int index = 0; index < a.Count(0); index++)
            {
                for (int index2 = 0; index2 < a.Count(1); index2++)
                {
                    resultArray[index, index2] = a[index, index2] - b[index, index2];
                }
            }

            return new DoubleMatrix(resultArray);
        }

        /// <summary>
        /// Multiplies the matrices together, in the order AB.
        /// </summary>
        public static DoubleMatrix operator *(DoubleMatrix a, DoubleMatrix b)
        {
            int aLength0 = a.Count(0);
            int bLength0 = b.Count(0);
            int aLength1 = a.Count(1);
            int bLength1 = b.Count(1);
            if (!aLength1.Equals(bLength0))
            {
                return new DoubleMatrix(0, 0, a.DefaultValue);
            }

            double[,] multiply = new double[aLength0, bLength0];
            for (int firstMatrixRowIndex = 0; firstMatrixRowIndex < aLength0; firstMatrixRowIndex++)
            {
                for (int secondMatrixColumnIndex = 0; secondMatrixColumnIndex < bLength1; secondMatrixColumnIndex++)
                {
                    double thisIndexSum = 0.0;
                    for (int innerIndex = 0; innerIndex < aLength1; innerIndex++)
                    {
                        thisIndexSum += a[firstMatrixRowIndex, innerIndex] * b[innerIndex, secondMatrixColumnIndex];
                    }

                    multiply[firstMatrixRowIndex, secondMatrixColumnIndex] = thisIndexSum;
                }
            }

            return new DoubleMatrix(multiply);
        }

        /// <summary>
        /// Returns the Matrix post multiplied by the vector.
        /// </summary>
        public static DoubleVector operator *(DoubleMatrix a, DoubleVector b)
        {
            int aLength0 = a.Count(0);
            int bLength = b.Count;
            int aLength1 = a.Count(1);
            if (!aLength1.Equals(bLength))
            {
                return new DoubleVector(0, a.DefaultValue);
            }

            double[] multiply = new double[aLength0];
            for (int firstMatrixRowIndex = 0; firstMatrixRowIndex < aLength0; firstMatrixRowIndex++)
            {
                double thisIndexSum = 0.0;
                for (int innerIndex = 0; innerIndex < aLength1; innerIndex++)
                {
                    thisIndexSum += a[firstMatrixRowIndex, innerIndex] * b[innerIndex];
                }

                multiply[firstMatrixRowIndex] = thisIndexSum;

            }

            return new DoubleVector(multiply);
        }

        /// <summary>
        /// Multiplies a matrix by a scalar value.
        /// </summary>
        public static DoubleMatrix operator *(DoubleMatrix matrix, double scalar)
        {
            double[,] multiply = new double[matrix.Count(0), matrix.Count(1)];
            for (int matrixRowIndex = 0; matrixRowIndex < matrix.Count(0); matrixRowIndex++)
            {
                for (int matrixColumnIndex = 0; matrixColumnIndex < matrix.Count(1); matrixColumnIndex++)
                {
                    multiply[matrixRowIndex, matrixColumnIndex] = matrix[matrixRowIndex, matrixColumnIndex] * scalar;
                }
            }

            return new DoubleMatrix(multiply);
        }

        /// <summary>
        /// Multiplies a matrix by a scalar value.
        /// </summary>
        public static DoubleMatrix operator *(double scalar, DoubleMatrix matrix)
        {
            return matrix * scalar;
        }
    }
}
