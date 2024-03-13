using System;
using System.Linq;

namespace Effanville.Common.Structure.Extensions
{
    /// <summary>
    /// Miscellaneous custom extension functions for the <see cref="Enum"/> type.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Returns the longest length of the enum names.
        /// </summary>
        /// <typeparam name="T">The Enum type to query.</typeparam>
        /// <returns>An int of the longest number of characters in the string name for the Enum.</returns>
        public static int LongestEntry<T>() where T : Enum
        {
            string[] values = Enum.GetValues(typeof(T)).Cast<T>().Select(item => item.ToString()).ToArray();
            return values.Max(item => item.Length);
        }
    }
}
