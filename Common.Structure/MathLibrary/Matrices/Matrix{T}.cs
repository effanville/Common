using System;

namespace Common.Structure.MathLibrary.Matrices
{
    public class Matrix<T> where T : struct, IEquatable<T>, IComparable<T>
    {
        protected readonly T[,] _values;

        protected T DefaultValue
        {
            get;
            set;
        } = default(T);


        public T this[int index, int index2]
        {
            get => _values[index, index2];
            set => _values[index, index2] = value;
        }

        public Matrix(T[,] values, T defaultValue)
        {
            _values = values;
            DefaultValue = defaultValue;
        }

        public Matrix(T[,] values)
            : this(values, default(T))
        {
        }

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

        public int Count(int dimension)
        {
            return _values.GetLength(dimension);
        }

        public void Set(int index1, int index2, T item)
        {
            _values[index1, index2] = item;
        }

        public void RemoveAt(int index1, int index2)
        {
            _values[index1, index2] = default(T);
        }

        public virtual Matrix<T> Transpose()
        {
            T[,] transpose = Transpose(_values);
            return new Matrix<T>(transpose);
        }

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

        public bool IsSquare()
        {
            return IsSquare();
        }

        /// <summary>
        /// Checks whether input matrix is square.
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static bool IsSquare(T[,] matrix)
        {
            if (!matrix.GetLength(0).Equals(matrix.GetLength(1)))
            {
                return false;
            }

            return true;
        }

        public bool IsSymmetric()
        {
            return IsSymmetric();
        }

        /// <summary>
        /// Routine to check whether the input is symmetric.
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

        public override int GetHashCode()
        {
            return HashCode.Combine(_values, DefaultValue);
        }
    }
}
