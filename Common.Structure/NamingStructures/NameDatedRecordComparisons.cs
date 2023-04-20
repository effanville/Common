using System;

namespace Common.Structure.NamingStructures
{
    /// <summary>
    /// Comparison methods for records.
    /// </summary>
    public sealed class NameDatedRecordComparisons
    {
        /// <summary>
        /// Compare based upon the value.
        /// </summary>
        public static Comparison<NameDatedRecord<T>> ValueCompare<T>() where T : IComparable<T>
        {
            return (a, b) => b.Value.CompareTo(a.Value);
        }
    }
}
