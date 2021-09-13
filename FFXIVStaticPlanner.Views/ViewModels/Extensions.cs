using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace FFXIVStaticPlanner.ViewModels
{
    /// <summary>
    /// Provides additional functionality of objects
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Add a range of items to this collection
        /// </summary>
        /// <param name="items">A enumerable that need to be added</param>
        public static void AddRange<T> ( this ObservableCollection<T> collection , IEnumerable<T> items )
        {
            foreach ( var item in items )
            {
                collection.Add ( item );
            }
        }

        /// <summary>
        /// Removes the first element of the sequence that satisfies a condition
        /// </summary>
        public static void Remove<T> ( this IList<T> list, Func<T , bool> predicate )
        {
            T item = list.FirstOrDefault ( predicate );

            if ( item != null )
            {
                list.Remove ( item );
            }
        }
    }
}