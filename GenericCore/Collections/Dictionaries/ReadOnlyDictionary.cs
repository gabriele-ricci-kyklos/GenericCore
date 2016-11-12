using GenericCore.Exceptions;
using GenericCore.Support;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCore.Collections.Dictionaries
{
    public class ReadOnlyDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly IDictionary<TKey, TValue> _dictionary;

        public ReadOnlyDictionary()
            : this(new Dictionary<TKey, TValue>())
        {
        }

        public ReadOnlyDictionary(IDictionary<TKey, TValue> dictionary)
        {
            dictionary.AssertNotNull("dictionary");
            _dictionary = dictionary;
        }

        public TValue this[TKey key]
        {
            get
            {
                return _dictionary[key];
            }
            set
            {
                throw new ReadOnlyException("The dictionary is read-only");
            }
        }

        public int Count
        {
            get
            {
                return _dictionary.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return true;
            }
        }

        public ICollection<TKey> Keys
        {
            get
            {
                return _dictionary.Keys;
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                return _dictionary.Values;
            }
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            throw new ReadOnlyException("The dictionary is read-only");
        }

        public void Add(TKey key, TValue value)
        {
            throw new ReadOnlyException("The dictionary is read-only");
        }

        public void Clear()
        {
            throw new ReadOnlyException("The dictionary is read-only");
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return _dictionary.Contains(item);
        }

        public bool ContainsKey(TKey key)
        {
            return _dictionary.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            _dictionary.CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            throw new ReadOnlyException("The dictionary is read-only");
        }

        public bool Remove(TKey key)
        {
            throw new ReadOnlyException("The dictionary is read-only");
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _dictionary.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
