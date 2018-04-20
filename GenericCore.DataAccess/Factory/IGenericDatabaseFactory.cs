using GenericCore.DataAccess.DAOHelper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCore.DataAccess.Factory
{
    public interface IGenericDatabaseFactory
    {
        IDAOHelper DAOHelper { get; }
        ConnectionStringSettings ConnectionString { get; }

        DbConnection GetGenericDbConnection();
    }
}
