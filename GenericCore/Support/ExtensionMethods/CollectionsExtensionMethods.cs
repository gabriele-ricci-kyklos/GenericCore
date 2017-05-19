using GenericCore.Collections.Dictionaries;
using GenericCore.Support.Collections;
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
            if (list.IsNullOrEmptyList())
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
            if (sequence.IsNullOrEmptyList())
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
            if (sequence.IsNullOrEmptyList())
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

        public static TResult SelectLast<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            source.AssertNotNull("source");
            selector.AssertNotNull("selector");

            return source.Select(selector).LastOrDefault();
        }

        public static bool IsIn<T>(this T item, params T[] list)
        {
            return IsIn(item, list.AsEnumerable());
        }

        public static bool IsIn<T>(this T item, Func<T, T, bool> eqFx, params T[] list)
        {
            return IsIn(item, list.AsEnumerable(), eqFx);
        }

        public static bool IsIn<T>(this T item, IEnumerable<T> list)
        {
            return IsIn(item, list, (x, y) => x.Equals(y));
        }

        public static bool IsIn<T>(this T item, IEnumerable<T> enumerablesList, Func<T, T, bool> eqFx)
        {
            item.AssertNotNull("item");
            eqFx.AssertNotNull("eqFx");

            if (enumerablesList.IsNullOrEmptyList())
            {
                return false;
            }

            return enumerablesList.Any(listItem => eqFx(item, listItem));
        }

        public static IList<T> AsList<T>(this T s, int size = 1)
        {
            return s.AsArray(size).ToList();
        }

        public static T[] AsArray<T>(this T item, int size = 1)
        {
            T[] array = new T[size];
            for (int i = 0; i < size; ++i)
            {
                array[i] = item;
            }
            return array;
        }

        public static T[] AsArrayOrNull<T>(this T item, int size = 1)
        {
            return item.IsNull() ? null : item.AsArray(size);
        }

        public static IList<T> AsListOrNull<T>(this T item, int size = 1)
        {
            return item.IsNull() ? null : item.AsList(size);
        }

        public static ReadOnlyDictionary<TKey, TValue> AsReadOnly<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            return new ReadOnlyDictionary<TKey, TValue>(dictionary);
        }

        public static bool IsNullOrEmptyList<T>(this IEnumerable<T> list)
        {
            return (list.IsNull() || list.Count() == 0);
        }

        public static IDictionary<TKey, TResult> ToDictionary<TKey, TResult>(this IEnumerable<KeyValuePair<TKey, TResult>> itemList)
        {
            itemList.AssertNotNull("itemList");

            if (itemList.IsNullOrEmptyList())
            {
                return new Dictionary<TKey, TResult>();
            }

            return itemList.ToDictionary(x => x.Key, x => x.Value);
        }

        public static IEnumerable<OuterLinQJoinResult<TOuter, TInner>> LeftJoin<TOuter, TInner, TKey>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector)
        {
            return LeftJoin(outer, inner, outerKeySelector, innerKeySelector, EqualityComparer<TKey>.Default);
        }

        public static IEnumerable<TResult> LeftJoin<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<OuterLinQJoinResult<TOuter, TInner>, TResult> resultSelector)
        {
            return
                LeftJoin(outer, inner, outerKeySelector, innerKeySelector)
                    .Select(x => resultSelector(x));
        }

        public static IEnumerable<TResult> LeftJoin<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<OuterLinQJoinResult<TOuter, TInner>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
        {
            return
                LeftJoin(outer, inner, outerKeySelector, innerKeySelector, comparer)
                    .Select(x => resultSelector(x));
        }

        public static IEnumerable<OuterLinQJoinResult<TOuter, TInner>> LeftJoin<TOuter, TInner, TKey>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, IEqualityComparer<TKey> comparer)
        {
            inner.AssertNotNull("outer");
            outer.AssertNotNull("outer");

            return
                outer
                    .GroupJoin
                    (
                        inner,
                        outerKeySelector,
                        innerKeySelector,
                        (x, y) => 
                            new
                            {
                                LeftPart = x,
                                RightPart = y
                            },
                        comparer
                    )
                    .SelectMany
                    (
                        x => x.RightPart.DefaultIfEmpty(),
                        (x, y) => 
                            new OuterLinQJoinResult<TOuter, TInner>
                            {
                                LeftPart = x.LeftPart,
                                RightPart = y
                            }
                    );
        }
    }
}
