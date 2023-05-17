using System;

namespace Common.Structure.NamingStructures
{
    /// <summary>
    /// Comparisons for <see cref="NamedRecord{T}"/>
    /// </summary>
    public static class NamedRecordComparers
    {
        /// <summary>
        /// Compare based upon the value.
        /// </summary>
        public static Comparison<NamedRecord<T>> ValueCompare<T>() where T : IComparable<T>
        {
            return (a, b) => b.Value.CompareTo(a.Value);
        }
    }
}
