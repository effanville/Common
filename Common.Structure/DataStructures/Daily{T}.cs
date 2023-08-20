using System;

using Common.Structure.Extensions;

namespace Common.Structure.DataStructures
{
    /// <summary>
    /// Holds a date and a value to act as the value on that day.
    /// </summary>
    public class Daily<T> :
        IComparable,
        IComparable<Daily<T>>,
        IEquatable<Daily<T>> where T : IEquatable<T>
    {
        /// <summary>
        /// The date for the valuation
        /// </summary>
        public DateTime Day
        {
            get;
            set;
        }

        /// <summary>
        /// The specific valuation
        /// </summary>
        public T Value
        {
            get;
            set;
        }

        /// <summary>
        /// Standard constructor.
        /// </summary>
        public Daily(DateTime idealDate, T idealValue)
        {
            Day = idealDate;
            Value = idealValue;
        }

        /// <summary>
        /// Creates a new <see cref="Daily{T}"/> from the given one.
        /// </summary>
        public Daily(Daily<T> dailyValue)
            : this(dailyValue.Day, dailyValue.Value)
        {
        }

        /// <summary>
        /// Appends date in UK format with value, separated by a comma.
        /// </summary>
        public override string ToString() => Day.ToUkDateStringPadded() + ", " + Value.ToString();

        /// <summary>
        /// Method of comparison. Compares dates.
        /// </summary>
        public int CompareTo(object obj)
        {
            if (obj is Daily<T> other)
            {
                return CompareTo(other);
            }

            return 0;
        }

        /// <inheritdoc/>
        public int CompareTo(Daily<T> other) => DateTime.Compare(Day, other.Day);


        /// <summary>
        /// Returns a copy of the specified valuation
        /// </summary>
        /// <returns></returns>
        public Daily<T> Copy() => new Daily<T>(Day, Value);

        /// <summary>
        /// Sets the data in the daily valuation.
        /// </summary>
        public void SetData(DateTime date, T value)
        {
            Day = date;
            Value = value;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj is Daily<T> other)
            {
                return Equals(other);
            }

            return false;
        }

        /// <inheritdoc/>
        public bool Equals(Daily<T> other)
            => Day.Equals(other.Day)
                && Value.Equals(other.Value);

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hashCode = 17;
            hashCode = 23 * hashCode + Day.GetHashCode();
            hashCode = 23 * hashCode + Value.GetHashCode();
            return hashCode;
        }
    }
}
