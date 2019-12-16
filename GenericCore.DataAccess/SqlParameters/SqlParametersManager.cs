using GenericCore.DataAccess.DAOHelper;
using GenericCore.Support;

namespace GenericCore.DataAccess.SqlParameters
{
    public class SqlParametersManager : ISqlParametersManager
    {
        private int _count = 0;
        private const string _paramNamePrefix = "pq";
        private IDAOHelper _daoHelper;

        public SqlParametersManager(IDAOHelper daoHelper)
        {
            daoHelper.AssertNotNull(nameof(daoHelper));
            _daoHelper = daoHelper;
        }

        public SqlParameter BuildSqlParameter(string fieldName, object value)
        {
            string paramName = BuildSqlParameterName(fieldName);
            return new SqlParameter(paramName, value);
        }

        private string BuildSqlParameterName(string fieldName)
        {
            string pname =
                _paramNamePrefix +
                fieldName
                .ToEmptyIfNull()
                .Replace(' ', '_')
                .SafeGetLeftPart(8)
                .ToLowerInvariant();

            return $"{_daoHelper.ParameterStartPrefix}{pname}_{++_count}";
        }
    }
}
