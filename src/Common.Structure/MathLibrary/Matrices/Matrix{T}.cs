using System;

namespace Common.Structure.MathLibrary.Matrices
{
    /// <summary>
    /// Contains a representation of a matrix of values of type <see paramref="T"/>.
    /// </summary>
    public class Matrix<T> where T : struct, IEquatable<T>, IComparable<T>
    {
        /// <summary>
        /// The values stored in this matrix.
        /// </summary>
        protected readonly T[,] _values;

        /// <summary>
        /// The default value for values in the matrix.
        /// </summary>
        protected T DefaultValue
        {
            get;
            set;
        } = default(T);

        /// <summary>
        /// Return a value at the specified indexes.
        /// </summary>
        public T this[int index, int index2]
        {
            get => _values[index, index2];
            set => _values[index, index2] = value;
        }

        /// <summary>
        /// Construct an instance
        /// </summary>
        public Matrix(T[,] values, T defaultValue)
        {
            _values = values;
            DefaultValue = defaultValue;
        }

        /// <summary>
        /// Construct an instance
        /// </summary>
        public Matrix(T[,] values)
            : this(values, default(T))
        {
        }

        /// <summary>
        /// Construct an instance
        /// </summary>
        public Matrix(int size1, int size2, T defaultValue)
        {
            _values = new T[size1, size2];
            for (int index1 = 0; index1 < size1; index1++)
            {
                for (int index2 = 0; index2 < size2; index2++)
                {
                    _values[index1, index2] = defaultValue;
                }
            }
        }

        /// <summary>
        /// Returns the length of the dimension requested.
        /// </summary>
        public int Count(int dimension)
        {
            return _values.GetLength(dimension);
        }

        /// <summary>
        /// Set the value at the given point to have the item value.
        /// </summary>
        public void Set(int index1, int index2, T item)
        {
            _values[index1, index2] = item;
        }

        /// <summary>
        /// Set the value at the indices to be the default value.
        /// </summary>
        public void RemoveAt(int index1, int index2)
        {
            _values[index1, index2] = default(T);
        }

        /// <summary>
        /// Transpose the matrix
        /// </summary>
        public virtual Matrix<T> Transpose()
        {
            T[,] transpose = Transpose(_values);
            return new Matrix<T>(transpose);
        }

        /// <summary>
        /// Transpose the matrix
        /// </summary>
        public static T[,] Transpose(T[,] matrix)
        {
            if (matrix.GetLength(1).Equals(0) || matrix.GetLength(0).Equals(0))
            {
                return new T[0, 0];
            }

            T[,] transpose = new T[matrix.GetLength(1), matrix.GetLength(0)];
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
        /// Checks whether input matrix is square.
        /// </summary>
        public bool IsSquare()
        {
            return IsSquare();
        }

        /// <summary>
        /// Checks whether input matrix is square.
        /// </summary>
        public static bool IsSquare(T[,] matrix)
        {
            if (!matrix.GetLength(0).Equals(matrix.GetLength(1)))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Is the matrix a symmetric matrix.
        /// </summary>
        public bool IsSymmetric()
        {
            return IsSymmetric();
        }

        /// <summary>
        /// Is the matrix a symmetric matrix.
        /// </summary>
        public static bool IsSymmetric(T[,] matrix)
        {
            if (!IsSquare(matrix))
            {
                return false;
            }

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < i; j++)
                {
                    if (!matrix[i, j].Equals(matrix[j, i]))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            var mat = obj as Matrix<T>;
            if (!DefaultValue.Equals(mat.DefaultValue))
            {
                return false;
            }

            int rowNum = Count(0);
            int colNum = Count(1);
            int matRowNum = mat.Count(0);
            int matColNum = mat.Count(1);
            if (rowNum != matRowNum || colNum != matColNum)
            {
                return false;
            }
            for (int rowIndex = 0; rowIndex < rowNum; rowIndex++)
            {
                for (int colIndex = 0; colIndex < colNum; colIndex++)
                {
                    if (!_values[rowIndex, colIndex].Equals(mat._values[rowIndex, colIndex]))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return HashCode.Combine(_values, DefaultValue);
        }
    }
}
