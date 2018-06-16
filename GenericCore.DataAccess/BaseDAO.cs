using GenericCore.DataAccess.DAOHelper;
using GenericCore.DataAccess.Factory;
using GenericCore.DataAccess.QueryBuilder;
using GenericCore.DataAccess.SqlParameters;
using GenericCore.Support;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCore.DataAccess
{
    public abstract class BaseDAO
    {
        protected IGenericDatabaseFactory Factory;
        protected ISqlParametersManager SqlParametersManager;

        protected IDAOHelper DAOHelper
        {
            get
            {
                return Factory.DAOHelper;
            }
        }

        public BaseDAO(IGenericDatabaseFactory factory)
        {
            factory.AssertNotNull(nameof(factory));

            Factory = factory;
            SqlParametersManager = new SqlParametersManager(Factory.DAOHelper);
        }

        protected SqlQueryBuilder NewSqlQueryBuilder()
        {
            return new SqlQueryBuilder(Factory.DAOHelper);
        }
        
        protected object ExecuteScalar(string query, IList<SqlParameter> parameters = null)
        {
            query.AssertHasText(nameof(query));

            using (var connection = Factory.GetGenericDbConnection())
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = query;

                AddParameters(command, parameters);

                return command.ExecuteScalar();
            }
        }

        protected int ExecuteNonQuery(string query, IList<SqlParameter> parameters = null)
        {
            query.AssertHasText(nameof(query));

            using (var connection = Factory.GetGenericDbConnection())
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = query;

                AddParameters(command, parameters);

                return command.ExecuteNonQuery();
            }
        }

        private void AddParameters(DbCommand command, IList<SqlParameter> parameters)
        {
            command.AssertNotNull(nameof(command));

            if(parameters.IsNullOrEmptyList())
            {
                return;
            }

            foreach (SqlParameter queryParam in parameters)
            {
                DbParameter param = command.CreateParameter();
                param.DbType = DAOHelper.MapTypeToDbType(queryParam.Type);
                param.ParameterName = queryParam.Name;
                param.Value = queryParam.Value;
                command.Parameters.Add(param);
            }
        }
    }
}
