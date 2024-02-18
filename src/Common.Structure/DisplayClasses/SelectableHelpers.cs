using System;
using System.Collections.Generic;

namespace Effanville.Common.Structure.DisplayClasses
{
    /// <summary>
    /// Helper methods for lists of <see cref="Selectable{T}"/> objects.
    /// </summary>
    public static class SelectableHelpers
    {
        /// <summary>
        /// Returns the boolean associated to a specific instance <paramref name="nameToSearch"/>
        /// out of the list <paramref name="names"/>. Returns false if it is not found.
        /// </summary>
        public static bool GetSelectableEquatableData<T>(List<SelectableEquatable<T>> names, T nameToSearch)
            where T : IEquatable<T>, IComparable
        {
            foreach (var name in names)
            {
                if (name.Instance.Equals(nameToSearch))
                {
                    return name.Selected;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns the boolean associated to a specific instance <paramref name="nameToSearch"/>
        /// out of the list <paramref name="names"/>. Returns false if it is not found.
        /// </summary>
        public static bool GetData<T>(List<Selectable<T>> names, T nameToSearch)
            where T : IEquatable<T>, IComparable
        {
            foreach (var name in names)
            {
                if (name.Instance.Equals(nameToSearch))
                {
                    return name.Selected;
                }
            }

            return false;
        }
    }
}
