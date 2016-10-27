using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace GenericCore.Support
{
    public static class CollectionsExtensionMethods
    {
        public static ReadOnlyCollection<T> AsReadOnly<T>(this IList<T> list)
        {
            if (list.IsNull())
            {
                return null;
            }

            return
                list
                    .ToList()
                    .AsReadOnly();
        }

        public static IDictionary<TKey, TValue> Find<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, IList<TKey> keyList)
        {
            dictionary.AssertNotNull("dictionary");
            keyList.AssertNotNull("keyList");

            IList<KeyValuePair<TKey, TValue>> resultList = new List<KeyValuePair<TKey, TValue>>();

            foreach (KeyValuePair<TKey, TValue> item in dictionary)
            {
                if (keyList.Contains(item.Key))
                {
                    resultList.Add(item);
                }
            }

            return resultList.ToDictionary(x => x.Key, x => x.Value);
        }

        public static bool IsNullOrEmptyList<T>(this IList<T> list)
        {
            return list.IsNull() || list.Count == 0;
        }

        public static IEnumerable<T> ToEmptyIfNull<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.IsNull() ? new List<T>() : enumerable;
        }

        public static IList<T> ToEmptyListIfNull<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.ToEmptyIfNull().ToList();
        }

        public static T[] ToEmptyArrayIfNull<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.ToEmptyIfNull().ToArray();
        }

        public static void ForEach<T>(this IEnumerable<T> sequence, Action<T> action)
        {
            if (sequence == null)
            {
                return;
            }

            foreach (var item in sequence.ToList())
            {
                action(item);
            }
        }

        public static void ForEach<T>(this IEnumerable<T> sequence, Action<T, int> action)
        {
            if (sequence == null)
            {
                return;
            }

            int index = 0;
            foreach (var item in sequence.ToList())
            {
                action(item, index);
                ++index;
            }
        }

        public static IList<Tuple<TResult>> ToTupleList<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            source.AssertNotNull("source");
            selector.AssertNotNull("selector");

            return 
                source
                    .Select(x => new Tuple<TResult>(selector(x)))
                    .ToList();
        }

        public static IList<Tuple<T1, T2>> ToTupleList<TSource, T1, T2>(this IEnumerable<TSource> source, Func<TSource, T1> selector1, Func<TSource, T2> selector2)
        {
            source.AssertNotNull("source");
            selector1.AssertNotNull("selector1");
            selector2.AssertNotNull("selector2");

            return 
                source
                    .Select(x => new Tuple<T1, T2>(selector1(x), selector2(x)))
                    .ToList();
        }

        public static IList<Tuple<T1, T2, T3>> ToTupleList<TSource, T1, T2, T3>(this IEnumerable<TSource> source, Func<TSource, T1> selector1, Func<TSource, T2> selector2, Func<TSource, T3> selector3)
        {
            source.AssertNotNull("source");
            selector1.AssertNotNull("selector1");
            selector2.AssertNotNull("selector2");
            selector3.AssertNotNull("selector3");

            return
                source
                    .Select(x => new Tuple<T1, T2, T3>(selector1(x), selector2(x), selector3(x)))
                    .ToList();
        }

        public static TResult SelectFirst<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            source.AssertNotNull("source");
            selector.AssertNotNull("selector");

            return source.Select(selector).FirstOrDefault();
        }

        public static TResult SelectFirst<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector, Func<TResult, bool> predicate)
        {
            source.AssertNotNull("source");
            selector.AssertNotNull("selector");
            predicate.AssertNotNull("predicate");

            return source.Select(selector).FirstOrDefault(predicate);
        }

        public static TResult SelectLast<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            source.AssertNotNull("source");
            selector.AssertNotNull("selector");

            return source.Select(selector).LastOrDefault();
        }

        public static TResult SelectLast<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector, Func<TResult, bool> predicate)
        {
            source.AssertNotNull("source");
            selector.AssertNotNull("selector");
            predicate.AssertNotNull("predicate");

            return source.Select(selector).LastOrDefault(predicate);
        }
    }
}
