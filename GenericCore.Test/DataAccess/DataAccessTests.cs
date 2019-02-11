using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GenericCore.DataAccess.QueryBuilder;
using GenericCore.DataAccess;
using GenericCore.Support;
using System.Data.Common;
using GenericCore.DataAccess.DAOHelper;
using GenericCore.DataAccess.Factory;
using GenericCore.DataAccess.SqlParameters;

namespace GenericCore.Test.DataAccess
{
    [TestClass]
    public class DataAccessTests
    {
        [TestMethod]
        public void OracleTestSimpleQuery()
        {
            long id = -1;
            SqlQueryBuilder builder = new SqlQueryBuilder(new OracleDAOHelper("Oracle.ManagedDataAccess.Client"));
            builder
                .Select(new string[] { "IDINSTANCE" })
                .From("BTINSTANCES", "B")
                .Where()
                .Condition("B", "BARCODE", WhereOperator.EqualTo, "0003517");

            string query = builder.ToString();
            string oracleConnectionStr = @"Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.100.42)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SID=ORCL)));User Id=QUALITYX_INDUSTRIES_DEV;Password=QUALITYX_INDUSTRIES_DEV;enlist=false";

            var factory = DbProviderFactories.GetFactory("Oracle.ManagedDataAccess.Client");
            using (var conn = factory.CreateConnection())
            {
                conn.ConnectionString = oracleConnectionStr;
                conn.Open();
                
                var command = conn.CreateCommand();
                command.CommandText = query;

                foreach (var builderParam in builder.Parameters)
                {
                    var param = command.CreateParameter();
                    param.ParameterName = builderParam.Name;
                    param.Value = builderParam.Value;
                    command.Parameters.Add(param);
                }

                using (DbDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        id = dr.GetInt64(0);
                    }
                }

                conn.Close();
            }

            Assert.IsTrue(id != -1);
        }

        [TestMethod]
        public void SqlServerTestSimpleQuery()
        {
            long id = -1;
            SqlQueryBuilder builder = new SqlQueryBuilder(new SqlServerDAOHelper("System.Data.SqlClient"));
            builder
                .Select(new string[] { "IDINSTANCE" })
                .From("BTINSTANCES", "B")
                .Where()
                .Condition("B", "BARCODE", WhereOperator.EqualTo, "5050925927567");

            string query = builder.ToString();
            string sqlServerConnectionStr = @"Data Source=192.168.100.42,9433\DEV2016;Initial Catalog=WmX_Kostelia_AH_Test;Persist Security Info=True;User Id=sa;Password=Sql2016$";

            var factory = DbProviderFactories.GetFactory("System.Data.SqlClient");
            using (var conn = factory.CreateConnection())
            {
                conn.ConnectionString = sqlServerConnectionStr;
                conn.Open();

                var command = conn.CreateCommand();
                command.CommandText = query;

                foreach (var builderParam in builder.Parameters)
                {
                    var param = command.CreateParameter();
                    param.ParameterName = builderParam.Name;
                    param.Value = builderParam.Value;
                    command.Parameters.Add(param);
                }

                using (DbDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        id = dr.GetInt64(0);
                    }
                }

                conn.Close();
            }

            Assert.IsTrue(id != -1);
        }

        [TestMethod]
        public void ExecuteScalarTest()
        {
            //changing the connection string to different providers will work
            var dao = new MyDAO();
            object a = dao.MyQuery(4679991);
        }

        [TestMethod]
        public void JoinTestQuery()
        {
            SqlQueryBuilder builder = new SqlQueryBuilder(new SqlServerDAOHelper("System.Data.SqlClient"));
            builder
                .Select(new SelectField("I", "IDINSTANCE").AsArray())
                .From("I", "BTINSTANCES")
                .InnerJoin("S", "BTWAREHOUSESTOCKS")
                .OnCondition("I", "IDINSTANCE", OnOperator.EqualTo, "S", "IDINSTANCE")
                .Where()
                .Condition("I", "BARCODE", WhereOperator.EqualTo, "5050925927567");
        }
    }

    class MyDAO : BaseDAO
    {
        public MyDAO() : base()
        {
        }

        public object MyQuery(long idInstance)
        {
            SqlParameter idInstanceParam = SqlParametersManager.BuildSqlParameter("IDINSTANCE", idInstance);
            return ExecuteScalar($"SELECT IDINSTANCE FROM BTINSTANCES WHERE IDINSTANCE = {idInstanceParam.Name}", idInstanceParam.AsArray());
        }
    }
}
