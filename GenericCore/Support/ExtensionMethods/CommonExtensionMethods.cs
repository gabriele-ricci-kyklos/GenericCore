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

        public static void AssertNotNull(this object o, string varName)
        {
            if (o.IsNull())
            {
                throw new ArgumentNullException(varName);
            }
        }

        public static T ConvertTo<T>(this object value)
        {
            Type t = typeof(T);
            t = Nullable.GetUnderlyingType(t) ?? t;

            T retValue = (value == null || DBNull.Value.Equals(value)) ? default(T) : (T)Convert.ChangeType(value, t);
            return retValue;
        }

        public static TResult Return<TInput, TResult>(this TInput o, Func<TInput, TResult> evaluator, TResult failureValue) where TInput : class
        {
            return o == null ? failureValue : evaluator(o);
        }

        public static TResult Return<TInput, TResult>(this TInput? o, Func<TInput?, TResult> evaluator, TResult failureValue)
                    where TInput : struct
        {
            return !o.HasValue ? failureValue : evaluator(o.Value);
        }
    }
}
