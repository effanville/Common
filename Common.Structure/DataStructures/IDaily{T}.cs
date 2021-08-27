using System;

namespace Common.Structure.DataStructures
{
    public interface IDaily<T> : IComparable where T : IEquatable<T>
    {
        /// <summary>
        /// The date for the valuation
        /// </summary>
        DateTime Day
        {
            get;
            set;
        }

        /// <summary>
        /// The specific valuation
        /// </summary>
        T Value
        {
            get;
            set;
        }

        /// <summary>
        /// Returns a copy of the specified valuation
        /// </summary>
        /// <returns></returns>
        IDaily<T> Copy();

        /// <summary>
        /// Sets the data in the daily valuation.
        /// </summary>
        void SetData(DateTime date, T value);
    }
}
