using System;
using System.Collections.Generic;
using System.Linq;

namespace FinalYearProject.Extensions
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> collection, int batchSize)
        {
            if (collection is null)
                throw new ArgumentNullException(nameof(collection));

            if (batchSize is 0)
                throw new ArgumentException(nameof(batchSize), "Batch size cannot be 0.");

            var list = collection.ToList();
            for (int i = 0; i < list.Count; i += batchSize)
            {
                yield return list.GetRange(i, Math.Min(batchSize, list.Count - i));
            }
        }

        public static int TotalCount(this IEnumerable<IEnumerable<object>> collection)
        {
            return collection.Sum(x => x.Count());
        }
    }
}
