using System;
using System.Collections.Generic;

namespace ImageToPDF
{
    static class EnumerableExtension
    {
        public static IEnumerable<Tuple<T, int>> WithIndex<T>(this IEnumerable<T> sequence)
        {
            var index = 0;
            foreach (var item in sequence) yield return Tuple.Create(item, index++);
        }
    }
}
