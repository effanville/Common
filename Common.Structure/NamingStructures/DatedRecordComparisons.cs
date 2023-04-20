using System;

namespace Common.Structure.NamingStructures
{
    /// <summary>
    /// Record comparisons.
    /// </summary>
    public sealed class DatedRecordComparisons
    {
        /// <summary>
        /// Compare based upon value.
        /// </summary>
        public static Comparison<DatedRecord<T>> ValueCompare<T>() where T : IComparable<T>
        {
            return (a, b) => b.Value.CompareTo(a.Value);
        }

        /// <summary>
        /// Compare based upon date.
        /// </summary>
        public static Comparison<DatedRecord<T>> DateCompare<T>()
        {
            return (a, b) => b.Date.CompareTo(a.Date);
        }

        /// <summary>
        /// Compare based upon date in inverted order.
        /// </summary>
        public static Comparison<DatedRecord<T>> InverseDateCompare<T>()
        {
            return (a, b) => -1 * b.Date.CompareTo(a.Date);
        }
    }
}
