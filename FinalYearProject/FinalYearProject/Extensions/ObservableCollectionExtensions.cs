using System;
using System.Collections.Generic;
using System.Linq;

namespace FinalYearProject.Extensions
{
    public static class ObservableCollectionExtensions
    {
        public static int RemoveAll<T>(this ICollection<T> coll, Func<T, bool> condition)
        {
            var itemsToRemove = coll.Where(condition).ToList();

            foreach (var itemToRemove in itemsToRemove)
            {
                coll.Remove(itemToRemove);
            }

            return itemsToRemove.Count;
        }
    }
}
