using System;
using System.Collections.Generic;
using System.Linq;

namespace ImageToPDF
{
    static class EnumerableExtension
    {
        public static IEnumerable<T> Slice<T>(this IEnumerable<T> sequence, int startIndex, int length)
        {
            return sequence.Skip(startIndex).Take(length);
        }
        public static IEnumerable<T> FromTo<T>(this IEnumerable<T> sequence, int from, int to)
        {
            var length = to - from;
            return sequence.Slice(from, length);
        }
        public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> sequence)
        {
            var index = 0;
            foreach (var item in sequence) yield return (item, index++);
        }
        public static IEnumerable<int> AllIndexesOf<T>(this IEnumerable<T> sequence, Predicate<T> predicate)
        {
            foreach (var (item, index) in sequence.WithIndex())
            {
                if (predicate(item)) yield return index;
            }
        }
        /// <summary>
        /// コレクションの要素を次の要素と共に返します．
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sequence"></param>
        /// <returns></returns>
        public static IEnumerable<(T current, bool hasNext, T next)> WithNext<T>(this IEnumerable<T> sequence)
        {
            var isFirst = true;
            var before = default(T);
            foreach (var item in sequence)
            {
                if (isFirst)
                {
                    isFirst = false;
                    before = item;
                    continue;
                }
                yield return (before, true, item);
                before = item;
            }
            //最後の要素を返す
            yield return (before, false, default(T));
        }
    }
}
