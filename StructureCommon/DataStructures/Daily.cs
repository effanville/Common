using System;
using StructureCommon.Extensions;

namespace StructureCommon.DataStructures
{
    /// <summary>
    /// Holds a date and a value to act as the value on that day.
    /// </summary>
    public class Daily<T> : IComparable where T : class
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
        /// Creates a new <see cref="Daily"/> from the given one.
        /// </summary>
        public Daily(Daily<T> dailyValue)
            : this(dailyValue.Day, dailyValue.Value)
        {
        }

        /// <summary>
        /// Appends date in UK format with value, separated by a comma.
        /// </summary>
        public override string ToString()
        {
            return Day.ToUkDateStringPadded() + ", " + Value.ToString();
        }

        /// <summary>
        /// Method of comparison. Compares dates.
        /// </summary>
        public virtual int CompareTo(object obj)
        {
            Daily<T> a = (Daily<T>)obj;
            return DateTime.Compare(Day, a.Day);
        }

        /// <summary>
        /// Returns a copy of the specified valuation
        /// </summary>
        /// <returns></returns>
        public Daily<T> Copy()
        {
            return new Daily<T>(Day, Value);
        }

        /// <summary>
        /// Sets the data in the daily valuation.
        /// </summary>
        public void SetData(DateTime date, T value)
        {
            Day = date;
            Value = value;
        }

        /// <summary>
        /// Sets the day field only.
        /// </summary>
        public void SetDay(DateTime date)
        {
            Day = date;
        }

        /// <summary>
        /// Sets the value field.
        /// </summary>
        public void SetValue(T value)
        {
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

        private bool Equals(Daily<T> other)
        {
            return Day.Equals(other.Day) && Value.Equals(other.Value);
        }

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
