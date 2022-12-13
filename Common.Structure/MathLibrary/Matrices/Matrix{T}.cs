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

        public Matrix(T[,] values)
        {
            _values = values;
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

        public Matrix<T> Transpose()
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
    }
}
