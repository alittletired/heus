using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace System
{
    /// <summary>
    /// Extension methods for all objects.
    /// </summary>
    public static class ObjectExtensions
    {



        /// <summary>
        /// Converts given object to a value type using <see cref="Convert.ChangeType(object,System.Type)"/> method.
        /// </summary>
        /// <param name="obj">Object to be converted</param>
        /// <typeparam name="T">Type of the target object</typeparam>
        /// <returns>Converted object</returns>
        public static T To<T>(this object obj)
            where T : struct
        {

            if (typeof(T) == typeof(Guid))
            {

                return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFrom(obj)!;

            }

            return (T)Convert.ChangeType(obj, typeof(T), CultureInfo.InvariantCulture);
        }
        /// <summary>
        /// Check if an item is in a list.
        /// </summary>
        /// <param name="item">Item to check</param>
        /// <param name="list">List of items</param>
        /// <typeparam name="T">Type of the items</typeparam>
        public static bool IsIn<T>(this T item, params T[] list)
        {
            return list.Contains(item);
        }

        /// <summary>
        /// Check if an item is in the given enumerable.
        /// </summary>
        /// <param name="item">Item to check</param>
        /// <param name="items">Items</param>
        /// <typeparam name="T">Type of the items</typeparam>
        public static bool IsIn<T>(this T item, IEnumerable<T> items)
        {
            return items.Contains(item);
        }

       
    }
}
