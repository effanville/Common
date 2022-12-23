using System;
using System.Collections;
using System.Collections.Generic;

namespace Common.Structure.MathLibrary.Vectors
{
    /// <summary>
    /// Generic implementation of a vector. Wraps an array and provides convenient return methods.
    /// </summary>
    /// <typeparam name="T">The type of the values in the vector.</typeparam>
    public class Vector<T> : IEnumerable, IEnumerable<T> where T : struct, IEquatable<T>, IComparable<T>
    {
        protected readonly T[] _values;

        protected T DefaultValue
        {
            get;
            set;
        } = default(T);

        public T this[int index]
        {
            get => _values[index];
            set => _values[index] = value;
        }

        public T[] Values => _values;

        public int Count => _values.Length;

        public Vector(T[] values, T defaultValue)
        {
            _values = values;
            DefaultValue = defaultValue;
        }

        public Vector(T[] values)
            : this(values, default(T))
        {
        }

        public Vector(int size, T defaultValue)
        {
            _values = new T[size];
            for (int index = 0; index < size; index++)
            {
                _values[index] = defaultValue;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return (IEnumerator<T>)_values.GetEnumerator();
        }

        public void Set(int index, T item)
        {
            _values[index] = item;
        }

        public void RemoveAt(int index)
        {
            _values[index] = default(T);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        /// <summary>
        /// Calculates the maximum value of the subset consisting of the
        /// final number of values of a list of doubles
        /// </summary>
        /// <param name="numberValues">The final number of values to consider.</param>
        /// <returns>The maximum value.</returns>
        public T Max(int numberValues)
        {
            return Max(_values, numberValues, DefaultValue);
        }

        /// <summary>
        /// Calculates the maximum value of the subset consisting of the
        /// final number of values of a list.
        /// </summary>
        /// <param name="values">The list to calculate the max for.</param>
        /// <param name="numberValues">The final number of values to consider.</param>
        /// <returns>The maximum value.</returns>
        public static T Max(IReadOnlyList<T> values, int numberValues, T defaultValue = default(T))
        {
            if (values == null)
            {
                return defaultValue;
            }

            int numberEntries = values.Count;
            if (numberEntries < numberValues)
            {
                return defaultValue;
            }

            if (numberEntries == 0)
            {
                return defaultValue;
            }

            T maximum = values[numberEntries - 1];
            for (int index = 0; index < numberValues; index++)
            {
                T latestVal = values[numberEntries - 1 - index];
                if (maximum.CompareTo(latestVal) < 0)
                {
                    maximum = latestVal;
                }
            }

            return maximum;
        }

        /// <summary>
        /// Calculates the maximum value of the vector.
        /// </summary>
        /// <returns>The maximum value.</returns>
        public T Max()
        {
            return Max(Count);
        }

        /// <summary>
        /// Calculates the minimum value of the subset consisting of the
        /// final number of values of a list of doubles
        /// </summary>
        /// <param name="numberValues">The final number of values to consider.</param>
        /// <returns>The minimum value.</returns>
        public T Min(int numberValues)
        {
            if (_values == null)
            {
                return DefaultValue;
            }

            if (Count < numberValues)
            {
                return DefaultValue;
            }

            if (Count == 0)
            {
                return DefaultValue;
            }

            T minimum = _values[Count - 1];
            for (int index = 0; index < numberValues; index++)
            {
                T latestVal = _values[Count - 1 - index];
                if (minimum.CompareTo(latestVal) > 0)
                {
                    minimum = latestVal;
                }
            }

            return minimum;
        }

        /// <summary>
        /// Calculates the minimum value of the subset consisting of the
        /// final number of values of a list
        /// </summary>
        /// <param name="values">The list to calculate the min for.</param>
        /// <param name="numberValues">The final number of values to consider.</param>
        /// <returns>The minimum value.</returns>
        public static T Min(IReadOnlyList<T> values, int numberValues, T defaultValue = default(T))
        {
            if (values == null)
            {
                return defaultValue;
            }

            int numberEntries = values.Count;
            if (numberEntries < numberValues)
            {
                return defaultValue;
            }

            if (numberEntries == 0)
            {
                return defaultValue;
            }

            T minimum = values[numberEntries - 1];
            for (int index = 0; index < numberValues; index++)
            {
                T latestVal = values[numberEntries - 1 - index];
                if (minimum.CompareTo(latestVal) > 0)
                {
                    minimum = latestVal;
                }
            }

            return minimum;
        }

        /// <summary>
        /// Calculates the minimum value of the vector.
        /// </summary>
        /// <returns>The minimum value.</returns>
        public T Min()
        {
            return Min(Count);
        }
    }
}
