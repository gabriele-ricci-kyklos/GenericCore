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
                    //keyList.Remove(item.Key); for optimization
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
    }
}
