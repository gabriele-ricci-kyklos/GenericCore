using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GenericCore.Support
{
    public static class ReflectionExtensionMethods
    {
        public static IEnumerable<PropertyInfo> GetProperties(this object item)
        {
            item.AssertNotNull("item");

            foreach (PropertyInfo property in item.GetType().GetProperties())
            {
                yield return property;
            }
        }
    }
}
