using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
