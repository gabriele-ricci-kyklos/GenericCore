using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCore.Support
{
    public static class DictionaryExtensionMethods
    {
        public static IDictionary<TKey, TValue> Find<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, IList<TKey> keyList)
        {
            dictionary.AssertNotNull("dictionary");
            keyList.AssertNotNullAndHasElementsNotNull("keyList");

            IDictionary<TKey, TValue> returnDict = new Dictionary<TKey, TValue>();

            foreach (TKey key in keyList)
            {
                if (dictionary.TryGetValue(key, out TValue value))
                {
                    returnDict.Add(key, value);
                }
            }

            return returnDict;
        }

        public static ReadOnlyDictionary<TKey, TValue> AsReadOnly<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            return new ReadOnlyDictionary<TKey, TValue>(dictionary);
        }

        public static Dictionary<TKey, TResult> ToDictionary<TKey, TResult>(this IEnumerable<KeyValuePair<TKey, TResult>> itemList)
        {
            itemList.AssertNotNull("itemList");

            if (itemList.IsNullOrEmptyList())
            {
                return new Dictionary<TKey, TResult>();
            }

            return itemList.ToDictionary(x => x.Key, x => x.Value);
        }

        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            dict.AssertNotNull(nameof(dict));

            if (dict.TryGetValue(key, out TValue existingValue))
            {
                return existingValue;
            }
            else
            {
                return value;
            }
        }

        public static TValue GetValueOrDefault<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> items, TKey key, TValue value)
        {
            items.AssertNotNull(nameof(items));

            IDictionary<TKey, TValue> dict = items as IDictionary<TKey, TValue>;

            if (dict.IsNull())
            {
                throw new InvalidCastException("Unable to cast the collection 'items' to IDictionary<TKey, TValue>");
            }

            if (dict.TryGetValue(key, out TValue existingValue))
            {
                return existingValue;
            }
            else
            {
                return value;
            }
        }

        public static ReadOnlyDictionary<TKey, TSource> ToReadOnlyDictionary<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            Dictionary<TKey, TSource> dict = source.ToDictionary(keySelector);
            return dict.AsReadOnly();
        }

        public static ReadOnlyDictionary<TKey, TSource> ToReadOnlyDictionary<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            Dictionary<TKey, TSource> dict = source.ToDictionary(keySelector, comparer);
            return dict.AsReadOnly();
        }

        public static ReadOnlyDictionary<TKey, TElement> ToReadOnlyDictionary<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
        {
            Dictionary<TKey, TElement> dict = source.ToDictionary(keySelector, elementSelector);
            return dict.AsReadOnly();
        }

        public static ReadOnlyDictionary<TKey, TElement> ToReadOnlyDictionary<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
        {
            Dictionary<TKey, TElement> dict = source.ToDictionary(keySelector, elementSelector, comparer);
            return dict.AsReadOnly();
        }

        public static ReadOnlyDictionary<TKey, TResult> ToReadOnlyDictionary<TKey, TResult>(this IEnumerable<KeyValuePair<TKey, TResult>> itemList)
        {
            if (itemList.IsNullOrEmptyList())
            {
                return new ReadOnlyDictionary<TKey, TResult>(new Dictionary<TKey, TResult>());
            }

            return itemList.ToReadOnlyDictionary(x => x.Key, x => x.Value);
        }

        public static ReadOnlyDictionary<TKey, TElement> ToReadOnlyDictionary<TSource, TKey, TElement>(this IEnumerable<KeyValuePair<TKey, TElement>> itemList, IEqualityComparer<TKey> comparer)
        {
            if (itemList.IsNullOrEmptyList())
            {
                return new ReadOnlyDictionary<TKey, TElement>(new Dictionary<TKey, TElement>(comparer));
            }

            return itemList.ToReadOnlyDictionary(x => x.Key, x => x.Value, comparer);
        }
    }
}
