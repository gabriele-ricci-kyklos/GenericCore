using GenericCore.DataAccess.DAOHelper;
using GenericCore.DataAccess.Factory;
using GenericCore.DataAccess.QueryBuilder;
using GenericCore.DataAccess.SqlParameters;
using GenericCore.Support;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace GenericCore.DataAccess
{
    public abstract class BaseDAO
    {
        protected IGenericDatabaseFactory Factory;
        protected ISqlParametersManager SqlParametersManager;
        protected IDAOHelper DAOHelper => Factory.DAOHelper;

        public BaseDAO(IGenericDatabaseFactory factory = null)
        {
            Factory = factory ?? new GenericDatabaseFactory();
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
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = query;

                AddParameters(command, parameters);

                return command.ExecuteScalar();
            }
        }

        protected async Task<object> ExecuteScalarAsync(string query, IList<SqlParameter> parameters = null)
        {
            query.AssertHasText(nameof(query));

            using (var connection = Factory.GetGenericDbConnection())
            using (var command = connection.CreateCommand())
            {
                await connection.OpenAsync();
                command.CommandText = query;

                AddParameters(command, parameters);

                return await command.ExecuteScalarAsync();
            }
        }

        protected object ExecuteScalar(SqlQueryBuilder builder)
        {
            builder.AssertNotNull(nameof(builder));
            return ExecuteScalar(builder.CurrentSQL, builder.Parameters);
        }

        protected async Task<object> ExecuteScalarAsync(SqlQueryBuilder builder)
        {
            builder.AssertNotNull(nameof(builder));
            return await ExecuteScalarAsync(builder.CurrentSQL, builder.Parameters);
        }

        protected int ExecuteNonQuery(string query, IList<SqlParameter> parameters = null)
        {
            query.AssertHasText(nameof(query));

            using (var connection = Factory.GetGenericDbConnection())
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = query;

                AddParameters(command, parameters);

                return command.ExecuteNonQuery();
            }
        }

        protected async Task<int> ExecuteNonQueryAsync(string query, IList<SqlParameter> parameters = null)
        {
            query.AssertHasText(nameof(query));

            using (var connection = Factory.GetGenericDbConnection())
            using (var command = connection.CreateCommand())
            {
                await connection.OpenAsync();
                command.CommandText = query;

                AddParameters(command, parameters);

                return await command.ExecuteNonQueryAsync();
            }
        }

        protected int ExecuteNonQuery(SqlQueryBuilder builder)
        {
            builder.AssertNotNull(nameof(builder));
            return ExecuteNonQuery(builder.CurrentSQL, builder.Parameters);
        }

        protected async Task<int> ExecuteNonQueryAsync(SqlQueryBuilder builder)
        {
            builder.AssertNotNull(nameof(builder));
            return await ExecuteNonQueryAsync(builder.CurrentSQL, builder.Parameters);
        }

        private void AddParameters(DbCommand command, IList<SqlParameter> parameters)
        {
            command.AssertNotNull(nameof(command));

            if (parameters.IsNullOrEmptyList())
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
