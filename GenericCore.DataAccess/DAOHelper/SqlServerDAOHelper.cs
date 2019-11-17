using GenericCore.Support;
using System;
using System.Collections.Generic;
using System.Data;

namespace GenericCore.DataAccess.DAOHelper
{
    public class SqlServerDAOHelper : IDAOHelper
    {
        private readonly static IDictionary<Type, DbType> _typeToDbMapping;

        public string ParameterStartPrefix
        {
            get
            {
                return "@";
            }
        }

        public string ProviderInvariantName { get; }

        public string EscapeField(string fieldName)
        {
            fieldName.AssertNotNull(nameof(fieldName));
            return $"[{fieldName}]";
        }

        public DbType MapTypeToDbType(Type type)
        {
            DbType value;
            if (!_typeToDbMapping.TryGetValue(type, out value))
            {
                throw new ArgumentException($"The type {type.Name} is not mapped to any DbType");
            }

            return value;
        }

        static SqlServerDAOHelper()
        {
            _typeToDbMapping =
                new Dictionary<Type, DbType>
                {
                    { typeof(long), DbType.Int64 },
                    { typeof(ulong), DbType.UInt64 },
                    { typeof(int), DbType.Int32 },
                    { typeof(uint), DbType.UInt32 },
                    { typeof(short), DbType.Int16 },
                    { typeof(ushort), DbType.UInt16 },
                    { typeof(float), DbType.Single },
                    { typeof(double), DbType.Double },
                    { typeof(decimal), DbType.Decimal },
                    { typeof(byte[]), DbType.Binary },
                    { typeof(bool), DbType.Boolean },
                    { typeof(char), DbType.String },
                    { typeof(char[]), DbType.String },
                    { typeof(string), DbType.String },

                    { typeof(long?), DbType.Int64 },
                    { typeof(ulong?), DbType.UInt64 },
                    { typeof(int?), DbType.Int32 },
                    { typeof(uint?), DbType.UInt32 },
                    { typeof(short?), DbType.Int16 },
                    { typeof(ushort?), DbType.UInt16 },
                    { typeof(float?), DbType.Single },
                    { typeof(double?), DbType.Double },
                    { typeof(decimal?), DbType.Decimal },
                    { typeof(bool?), DbType.Boolean },
                    { typeof(char?), DbType.String },

                    { typeof(object), DbType.Object },
                    { typeof(DateTime), DbType.DateTime },
                    { typeof(DateTimeOffset), DbType.DateTimeOffset },
                    { typeof(TimeSpan), DbType.Time },
                    { typeof(Guid), DbType.Guid },
                };
        }

        public SqlServerDAOHelper(string providerName)
        {
            providerName.AssertHasText("providerName");
            ProviderInvariantName = providerName;
        }
    }
}
