using GenericCore.DataAccess.DAOHelper;
using GenericCore.Support;
using System;
using System.Configuration;
using System.Data.Common;

namespace GenericCore.DataAccess.Factory
{
    public abstract class BaseGenericDatabaseFactory : IGenericDatabaseFactory
    {
        private const string _connectionStringName = "GenericCoreDataSource";
        public ConnectionStringSettings ConnectionString { get; private set; }
        public IDAOHelper DAOHelper { get; }

        public BaseGenericDatabaseFactory()
        {
            AcquireConnectionString();
            DAOHelper = GetDAOHelperByProviderName(ConnectionString.ProviderName);
        }

        public DbConnection GetGenericDbConnection()
        {
            DbProviderFactory providerFactory = DbProviderFactories.GetFactory(ConnectionString.ProviderName);
            DbConnection connection = providerFactory.CreateConnection();
            connection.ConnectionString = ConnectionString.ConnectionString;
            return connection;
        }

        private void AcquireConnectionString()
        {
            ConnectionStringSettings connectionStringObj = ConfigurationManager.ConnectionStrings[_connectionStringName];

            if (connectionStringObj.IsNull())
            {
                throw new ArgumentException($"No connection string is configured: expected connection string with name '{_connectionStringName}'");
            }

            ConnectionString = connectionStringObj;
        }

        private IDAOHelper GetDAOHelperByProviderName(string providerName)
        {
            providerName.AssertHasText("providerName");

            IDAOHelper daoHelper = null;

            switch (providerName)
            {
                case SupportedDatabaseProviders.Oracle:
                    daoHelper = new OracleDAOHelper(providerName);
                    break;
                case SupportedDatabaseProviders.SQLServer:
                    daoHelper = new SqlServerDAOHelper(providerName);
                    break;
                default:
                    daoHelper = GetDAOHelperByProviderNameCore(providerName);
                    break;
            }

            if (daoHelper.IsNull())
            {
                throw new ArgumentException($"No configured DAO Helper found for provider name '{providerName}'");
            }

            return daoHelper;
        }

        protected abstract IDAOHelper GetDAOHelperByProviderNameCore(string providerName);
    }
}
