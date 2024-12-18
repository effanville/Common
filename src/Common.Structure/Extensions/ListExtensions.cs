﻿using System.Collections.Generic;

using Effanville.Common.Structure.Validation;

namespace Effanville.Common.Structure.Extensions
{
    /// <summary>
    /// Contains extensions for lists.
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// Adds an element to the list only if it is not null.
        /// </summary>
        public static void AddIfNotNull<T>(this List<T> list, T elementToAdd) where T : class
        {
            if (elementToAdd != null)
            {
                list.Add(elementToAdd);
            }
        }

        /// <summary>
        /// Adds an element to the list only if it is not null.
        /// </summary>
        public static void AddValidations(this List<ValidationResult> list, List<ValidationResult> elementsToAdd, string location)
        {
            if (elementsToAdd != null)
            {
                elementsToAdd.ForEach(result => result.AmendLocation(location));
                list.AddRange(elementsToAdd);
            }
        }
    }
}
