using System;

namespace Common.Structure.NamingStructures
{
    /// <summary>
    /// Comparer methods for a <see cref="NameDurationRecord{T}"/>.
    /// </summary>
    public static class NameDurationComparers
    {
        /// <summary>
        /// Compare two records based upon their value.
        /// </summary>
        public static Comparison<NameDurationRecord<T>> ValueCompare<T>() where T : IComparable<T>
        {
            return (a, b) => b.Value.CompareTo(a.Value);
        }
    }
}
