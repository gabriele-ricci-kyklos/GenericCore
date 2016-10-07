using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCore.Support
{
    public static class CommonExtensionMethods
    {
        public static bool IsNull(this object o)
        {
            return ReferenceEquals(o, null);
        }

        public static bool IsNotNull(this object o)
        {
            return !o.IsNull();
        }

        public static bool IsNullOrEmpty(this string s)
        {
            return string.IsNullOrEmpty(s);
        }

        public static bool IsNullOrEmptyList<T>(this IList<T> list)
        {
            return list.IsNull() || list.Count == 0;
        }

        public static void AssertNotNull(this object o, string varName)
        {
            if (o.IsNull())
            {
                throw new ArgumentNullException(varName);
            }
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

        public static bool IsNullOrBlankString(this string str)
        {
            return str.IsNull() || str == string.Empty;
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

        public static string ToSafeString(this object item, string nullValueReplacement = "")
        {
            if (item == null)
            {
                return nullValueReplacement;
            }

            return item.ToString();
        }

        public static string FormatWith(this string format, params object[] args)
        {
            return format.IsNullOrEmpty() ? string.Empty : string.Format(format, args);
        }

        public static ReadOnlyCollection<T> AsReadOnly<T>(this IList<T> list)
        {
            if(list.IsNull())
            {
                return null;
            }

            return 
                list
                    .ToList()
                    .AsReadOnly();
        }
    }
}
