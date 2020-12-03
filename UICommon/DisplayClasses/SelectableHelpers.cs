﻿using System.Collections.Generic;

namespace UICommon.DisplayClasses
{
    public static class SelectableHelpers
    {
        /// <summary>
        /// Returns the boolean associated to a specific instance <paramref name="nameToSearch"/>
        /// out of the list <paramref name="names"/>. Returns false if it is not found.
        /// </summary>
        public static bool GetData<T>(List<Selectable<T>> names, T nameToSearch)
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
