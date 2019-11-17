using GenericCore.DataAccess.DAOHelper;
using System.Configuration;
using System.Data.Common;

namespace GenericCore.DataAccess.Factory
{
    public interface IGenericDatabaseFactory
    {
        IDAOHelper DAOHelper { get; }
        ConnectionStringSettings ConnectionString { get; }

        DbConnection GetGenericDbConnection();
    }
}
