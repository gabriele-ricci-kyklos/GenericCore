using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCore.Support
{
    public static class StringsExtensionMethods
    {
        public static bool IsNullOrBlankString(this string str)
        {
            return str.IsNull() || str == string.Empty;
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

        public static string StringAppend(this string str, string str2, string separator = "")
        {
            if (separator.IsNull())
            {
                return "{0}{1}".FormatWith(str, str2);
            }

            return "{0}{1}{2}".FormatWith(str, separator, str2);
        }
    }
}
