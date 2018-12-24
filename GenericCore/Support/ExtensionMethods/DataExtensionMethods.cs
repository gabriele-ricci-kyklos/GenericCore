﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GenericCore.Support
{
    public static class DataExtensionMethods
    {
        public static bool IsNullOrEmpty(this DataTable table)
        {
            return table.IsNull() || table.Rows.Count == 0;
        }

        public static IEnumerable<DataColumn> AsEnumerable(this DataColumnCollection dataColumnCollection)
        {
            if (dataColumnCollection == null)
            {
                throw new ArgumentNullException("dataColumnCollection");
            }

            foreach (DataColumn dc in dataColumnCollection)
            {
                yield return dc;
            }
        }

        #region Convert to entity or list

        // ToEntityList DataRow[]

        public static IEnumerable<T> ToEntityList<T>(this DataRow[] rows, bool throwIfPropertyNotFound = false) where T : new()
        {
            return ToEntityList<T>(rows, throwIfPropertyNotFound, StringComparison.InvariantCultureIgnoreCase, null, null);
        }

        public static IEnumerable<T> ToEntityList<T>(this DataRow[] rows, bool throwIfPropertyNotFound, StringComparison propertyNameComparison) where T : new()
        {
            return ToEntityList<T>(rows, throwIfPropertyNotFound, propertyNameComparison, null, null);
        }

        public static IEnumerable<T> ToEntityList<T>(this DataRow[] rows, bool throwIfPropertyNotFound, StringComparison propertyNameComparison, IDictionary<Type, Func<object, object>> typesMap) where T : new()
        {
            return ToEntityList<T>(rows, throwIfPropertyNotFound, propertyNameComparison, typesMap, null);
        }

        public static IEnumerable<T> ToEntityList<T>(this DataRow[] rows, bool throwIfPropertyNotFound, StringComparison propertyNameComparison, IDictionary<string, string> propertyNamesMap) where T : new()
        {
            return ToEntityList<T>(rows, throwIfPropertyNotFound, propertyNameComparison, null, propertyNamesMap);
        }

        public static IEnumerable<T> ToEntityList<T>(this DataRow[] rows, bool throwIfPropertyNotFound, StringComparison propertyNameComparison, IDictionary<Type, Func<object, object>> typesMap, IDictionary<string, string> propertyNamesMap) where T : new()
        {
            return
                ToEntityList(rows, typeof(T), throwIfPropertyNotFound, propertyNameComparison, typesMap, propertyNamesMap)
                .Cast<T>();
        }

        public static IEnumerable<object> ToEntityList(this DataRow[] rows, Type type, bool throwIfPropertyNotFound = false)
        {
            return ToEntityList(rows, type, throwIfPropertyNotFound, StringComparison.InvariantCultureIgnoreCase, null, null);
        }

        public static IEnumerable<object> ToEntityList(this DataRow[] rows, Type type, bool throwIfPropertyNotFound, StringComparison propertyNameComparison)
        {
            return ToEntityList(rows, type, throwIfPropertyNotFound, propertyNameComparison, null, null);
        }

        public static IEnumerable<object> ToEntityList(this DataRow[] rows, Type type, bool throwIfPropertyNotFound, StringComparison propertyNameComparison, IDictionary<Type, Func<object, object>> typesMap)
        {
            return ToEntityList(rows, type, throwIfPropertyNotFound, propertyNameComparison, typesMap, null);
        }

        public static IEnumerable<object> ToEntityList(this DataRow[] rows, Type type, bool throwIfPropertyNotFound, StringComparison propertyNameComparison, IDictionary<string, string> propertyNamesMap)
        {
            return ToEntityList(rows, type, throwIfPropertyNotFound, propertyNameComparison, null, propertyNamesMap);
        }

        public static IEnumerable<object> ToEntityList(this DataRow[] rows, Type type, bool throwIfPropertyNotFound, StringComparison propertyNameComparison, IDictionary<Type, Func<object, object>> typesMap, IDictionary<string, string> propertyNamesMap)
        {
            foreach (DataRow dr in rows)
            {
                object newRow = ToEntity(dr, type, throwIfPropertyNotFound, propertyNameComparison, typesMap, propertyNamesMap);
                yield return newRow;
            }
        }

        // ToEntityList DataTable

        public static IEnumerable<T> ToEntityList<T>(this DataTable table, bool throwIfPropertyNotFound = false) where T : new()
        {
            return ToEntityList<T>(table, throwIfPropertyNotFound, StringComparison.InvariantCultureIgnoreCase, null, null);
        }

        public static IEnumerable<T> ToEntityList<T>(this DataTable table, bool throwIfPropertyNotFound, StringComparison propertyNameComparison) where T : new()
        {
            return ToEntityList<T>(table, throwIfPropertyNotFound, propertyNameComparison, null, null);
        }

        public static IEnumerable<T> ToEntityList<T>(this DataTable table, bool throwIfPropertyNotFound, StringComparison propertyNameComparison, IDictionary<Type, Func<object, object>> typesMap) where T : new()
        {
            return ToEntityList<T>(table, throwIfPropertyNotFound, propertyNameComparison, typesMap, null);
        }

        public static IEnumerable<T> ToEntityList<T>(this DataTable table, bool throwIfPropertyNotFound, StringComparison propertyNameComparison, IDictionary<string, string> propertyNamesMap) where T : new()
        {
            return ToEntityList<T>(table, throwIfPropertyNotFound, propertyNameComparison, null, propertyNamesMap);
        }

        public static IEnumerable<T> ToEntityList<T>(this DataTable table, bool throwIfPropertyNotFound, StringComparison propertyNameComparison, IDictionary<Type, Func<object, object>> typesMap, IDictionary<string, string> propertyNamesMap) where T : new()
        {
            return
                ToEntityList(table, typeof(T), throwIfPropertyNotFound, propertyNameComparison, typesMap, propertyNamesMap)
                    .Cast<T>();
        }

        public static IEnumerable<object> ToEntityList(this DataTable table, Type type, bool throwIfPropertyNotFound = false)
        {
            return ToEntityList(table, type, throwIfPropertyNotFound, StringComparison.InvariantCultureIgnoreCase, null, null);
        }

        public static IEnumerable<object> ToEntityList(this DataTable table, Type type, bool throwIfPropertyNotFound, StringComparison propertyNameComparison)
        {
            return ToEntityList(table, type, throwIfPropertyNotFound, propertyNameComparison, null, null);
        }

        public static IEnumerable<object> ToEntityList(this DataTable table, Type type, bool throwIfPropertyNotFound, StringComparison propertyNameComparison, IDictionary<Type, Func<object, object>> typesMap)
        {
            return ToEntityList(table, type, throwIfPropertyNotFound, propertyNameComparison, typesMap, null);
        }

        public static IEnumerable<object> ToEntityList(this DataTable table, Type type, bool throwIfPropertyNotFound, StringComparison propertyNameComparison, IDictionary<string, string> propertyNamesMap)
        {
            return ToEntityList(table, type, throwIfPropertyNotFound, propertyNameComparison, null, propertyNamesMap);
        }

        public static IEnumerable<object> ToEntityList(this DataTable table, Type type, bool throwIfPropertyNotFound, StringComparison propertyNameComparison, IDictionary<Type, Func<object, object>> typesMap, IDictionary<string, string> propertyNamesMap)
        {
            table.AssertNotNull("table");

            foreach (DataRow row in table.Rows)
            {
                yield return ToEntity(row, type, throwIfPropertyNotFound, propertyNameComparison, typesMap, propertyNamesMap);
            }
        }

        public static object ToEntity(this DataRow tableRow, Type type, bool throwIfPropertyNotFound, StringComparison propertyNameComparison, IDictionary<Type, Func<object, object>> typesMap, IDictionary<string, string> propertyNamesMap)
        {
            object returnObj = Activator.CreateInstance(type);

            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);

            foreach (DataColumn col in tableRow.Table.Columns)
            {
                PropertyInfo propertyInfo = null;
                FieldInfo fieldInfo = null;

                string mappedPropertyName = null;

                if (!propertyNamesMap.IsNullOrEmptyList() && propertyNamesMap.ContainsKey(col.ColumnName))
                {
                    mappedPropertyName = propertyNamesMap[col.ColumnName];
                }

                propertyInfo = properties.FirstOrDefault(x => x.Name.Equals(mappedPropertyName ?? col.ColumnName, propertyNameComparison));

                Func<object, Type, object> defaultConverter = (obj, t) => obj.ConvertTo(t);
                Func<object, Type, object> typesMapConverter = (obj, t) => typesMap[t];
                Func<object, Type, object> cellConverter = typesMap.IsNullOrEmptyList() ? defaultConverter : typesMapConverter;

                if (!propertyInfo.IsNull())
                {
                    object value = cellConverter(tableRow[col], propertyInfo.PropertyType);
                    returnObj.SetPropertyValue(propertyInfo.Name, value);
                }
                else
                {
                    fieldInfo = fields.FirstOrDefault(x => x.Name.Equals(mappedPropertyName ?? col.ColumnName, propertyNameComparison));

                    if (fieldInfo.IsNotNull())
                    {
                        object value = cellConverter(tableRow[col], fieldInfo.FieldType);
                        returnObj.SetFieldValue(fieldInfo.Name, value);
                    }
                    else if (throwIfPropertyNotFound)
                    {
                        throw new ArgumentException($"The property '{col.ColumnName}' has not been found using the comparison '{propertyNameComparison}'");
                    }
                }
            }

            return returnObj;
        }

        #endregion
    }
}
