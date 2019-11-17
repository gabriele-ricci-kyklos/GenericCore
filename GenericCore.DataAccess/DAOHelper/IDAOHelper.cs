using System;
using System.Data;

namespace GenericCore.DataAccess.DAOHelper
{
    public interface IDAOHelper
    {
        string ProviderInvariantName { get; }
        string ParameterStartPrefix { get; }
        string EscapeField(string fieldName);
        DbType MapTypeToDbType(Type type);
    }
}
