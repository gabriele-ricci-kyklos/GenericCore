using System;
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

        public static IList<IList<T>> ToEntityMatrix<T>(this DataSet dataSet)
            where T : new()
        {
            return ConvertDataSetToListImpl<T>(dataSet, false, StringComparison.InvariantCultureIgnoreCase, null, null);
        }

        public static IList<IList<T>> ToEntityMatrix<T>(this DataSet dataSet, bool throwIfPropertyNotFound)
            where T : new()
        {
            return ConvertDataSetToListImpl<T>(dataSet, throwIfPropertyNotFound, StringComparison.InvariantCultureIgnoreCase, null, null);
        }

        public static IList<IList<T>> ToEntityMatrix<T>(this DataSet dataSet, bool throwIfPropertyNotFound, StringComparison propertyNameComparison)
            where T : new()
        {
            return ConvertDataSetToListImpl<T>(dataSet, throwIfPropertyNotFound, propertyNameComparison, null, null);
        }

        public static IList<IList<T>> ToEntityMatrix<T>(this DataSet dataSet, bool throwIfPropertyNotFound, StringComparison propertyNameComparison, IDictionary<Type, Func<object, object>> typesMap)
            where T : new()
        {
            return ConvertDataSetToListImpl<T>(dataSet, throwIfPropertyNotFound, propertyNameComparison, typesMap, null);
        }

        public static IList<IList<T>> ToEntityMatrix<T>(this DataSet dataSet, bool throwIfPropertyNotFound, StringComparison propertyNameComparison, IDictionary<string, string> propertyNamesMap)
            where T : new()
        {
            return ConvertDataSetToListImpl<T>(dataSet, throwIfPropertyNotFound, propertyNameComparison, null, propertyNamesMap);
        }

        public static IList<IList<T>> ToEntityMatrix<T>(this DataSet dataSet, bool throwIfPropertyNotFound, StringComparison propertyNameComparison, IDictionary<Type, Func<object, object>> typesMap, IDictionary<string, string> propertyNamesMap)
            where T : new()
        {
            return ConvertDataSetToListImpl<T>(dataSet, throwIfPropertyNotFound, propertyNameComparison, typesMap, propertyNamesMap);
        }

        private static IList<IList<T>> ConvertDataSetToListImpl<T>(DataSet ds, bool throwIfPropertyNotFound, StringComparison propertyNameComparison, IDictionary<Type, Func<object, object>> typesMap, IDictionary<string, string> propertyNamesMap)
            where T : new()
        {
            ds.AssertNotNull("ds");

            IList<IList<T>> resultMatrix = new List<IList<T>>();

            foreach (DataTable dt in ds.Tables)
            {
                IList<T> resultList = ConvertDataTableToListImpl<T>(dt, throwIfPropertyNotFound, propertyNameComparison, typesMap, propertyNamesMap);
                resultMatrix.Add(resultList);
            }

            return resultMatrix;
        }

        public static IList<T> ToEntityList<T>(this DataTable dataTable)
            where T : new()
        {
            return ConvertDataTableToListImpl<T>(dataTable, false, StringComparison.InvariantCultureIgnoreCase, null, null);
        }

        public static IList<T> ToEntityList<T>(this DataTable dataTable, bool throwIfPropertyNotFound)
            where T : new()
        {
            return ConvertDataTableToListImpl<T>(dataTable, throwIfPropertyNotFound, StringComparison.InvariantCultureIgnoreCase, null, null);
        }

        public static IList<T> ToEntityList<T>(this DataTable dataTable, bool throwIfPropertyNotFound, StringComparison propertyNameComparison)
            where T : new()
        {
            return ConvertDataTableToListImpl<T>(dataTable, throwIfPropertyNotFound, propertyNameComparison, null, null);
        }

        public static IList<T> ToEntityList<T>(this DataTable dataTable, bool throwIfPropertyNotFound, StringComparison propertyNameComparison, IDictionary<Type, Func<object, object>> typesMap)
            where T : new()
        {
            return ConvertDataTableToListImpl<T>(dataTable, throwIfPropertyNotFound, propertyNameComparison, typesMap, null);
        }

        public static IList<T> ToEntityList<T>(this DataTable dataTable, bool throwIfPropertyNotFound, StringComparison propertyNameComparison, IDictionary<string, string> propertyNamesMap)
            where T : new()
        {
            return ConvertDataTableToListImpl<T>(dataTable, throwIfPropertyNotFound, propertyNameComparison, null, propertyNamesMap);
        }

        public static IList<T> ToEntityList<T>(this DataTable dataTable, bool throwIfPropertyNotFound, StringComparison propertyNameComparison, IDictionary<Type, Func<object, object>> typesMap, IDictionary<string, string> propertyNamesMap)
            where T : new()
        {
            return ConvertDataTableToListImpl<T>(dataTable, throwIfPropertyNotFound, propertyNameComparison, typesMap, propertyNamesMap);
        }

        private static IList<T> ConvertDataTableToListImpl<T>(DataTable dataTable, bool throwIfPropertyNotFound, StringComparison propertyNameComparison, IDictionary<Type, Func<object, object>> typesMap, IDictionary<string, string> propertyNamesMap)
            where T : new()
        {
            dataTable.AssertNotNull("dataTable");

            PropertyInfo[] properties = typeof(T).GetProperties();

            if (properties.IsNullOrEmptyList())
            {
                return new List<T>();
            }

            IList<T> returnList = new List<T>();

            foreach (DataRow row in dataTable.Rows)
            {
                T returnObj = new T();

                foreach (DataColumn col in dataTable.Columns)
                {
                    PropertyInfo propertyInfo = null;
                    string mappedPropertyName = null;

                    if (!propertyNamesMap.IsNullOrEmptyList() && propertyNamesMap.ContainsKey(col.ColumnName))
                    {
                        mappedPropertyName = propertyNamesMap[col.ColumnName];
                    }

                    propertyInfo = properties.FirstOrDefault(x => x.Name.Equals((mappedPropertyName ?? col.ColumnName), propertyNameComparison));

                    if (!propertyInfo.IsNull())
                    {
                        Func<object, object> converter = (typesMap.IsNullOrEmptyList()) ? (obj) => obj.ConvertTo(propertyInfo.PropertyType) : typesMap[propertyInfo.PropertyType];
                        object value = converter(row[col]);
                        returnObj.SetPropertyValue(propertyInfo.Name, value);
                    }
                    else if (throwIfPropertyNotFound)
                    {
                        throw new ArgumentException($"The property '{col.ColumnName}' has not been found using the comparison '{propertyNameComparison}'");
                    }
                }

                returnList.Add(returnObj);
            }

            return returnList;
        }
    }
}
