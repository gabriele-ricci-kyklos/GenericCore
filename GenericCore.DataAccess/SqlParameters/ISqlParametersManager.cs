namespace GenericCore.DataAccess.SqlParameters
{
    public interface ISqlParametersManager
    {
        SqlParameter BuildSqlParameter(string fieldName, object value);
        //string BuildSqlParameterName(string fieldName);
    }
}