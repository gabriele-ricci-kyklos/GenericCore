using GenericCore.DataAccess.DAOHelper;
using GenericCore.DataAccess.Factory;
using GenericCore.DataAccess.QueryBuilder;
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
        protected IGenericDatabaseFactory _factory;

        protected IDAOHelper DAOHelper
        {
            get
            {
                return _factory.DAOHelper;
            }
        }

        public BaseDAO(IGenericDatabaseFactory factory)
        {
            factory.AssertNotNull(nameof(factory));
            _factory = factory;
        }

        protected SqlQueryBuilder NewSqlQueryBuilder()
        {
            return new SqlQueryBuilder(_factory.DAOHelper);
        }
        
        protected object ExecuteScalar(string query, IList<QueryDbParameter> parameters = null)
        {
            query.AssertHasText(nameof(query));

            using (var connection = _factory.GetGenericDbConnection())
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = query;

                AddParameters(command, parameters);

                return command.ExecuteScalar();
            }
        }

        protected int ExecuteNonQuery(string query, IList<QueryDbParameter> parameters = null)
        {
            query.AssertHasText(nameof(query));

            using (var connection = _factory.GetGenericDbConnection())
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = query;

                AddParameters(command, parameters);

                return command.ExecuteNonQuery();
            }
        }

        private void AddParameters(DbCommand command, IList<QueryDbParameter> parameters)
        {
            command.AssertNotNull(nameof(command));

            if(parameters.IsNullOrEmptyList())
            {
                return;
            }

            foreach (var builderParam in parameters)
            {
                var param = command.CreateParameter();
                param.ParameterName = builderParam.Name;
                param.Value = builderParam.Value;
                command.Parameters.Add(param);
            }
        }
    }
}
