using GenericCore.DataAccess.DAOHelper;
using GenericCore.DataAccess.SqlParameters;
using GenericCore.Support;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCore.DataAccess.QueryBuilder
{
    public class SqlQueryBuilder
    {
        private const string _paramNamePrefix = "pq";
        private StringBuilder _query;
        private ISqlParametersManager _parametersManager;

        private IEnumerable<SelectField> _selectFields;
        private TableItem _currentJoinedTable;
        private TablesCollection _tablesMap;

        public IDAOHelper DAOHelper { get; private set; }
        public IList<SqlParameter> Parameters { get; private set; }

        public string CurrentSQL
        {
            get
            {
                return _query.ToString();
            }
        }

        public SqlQueryBuilder(IDAOHelper daoHelper)
        {
            daoHelper.AssertNotNull(nameof(daoHelper));

            _query = new StringBuilder();
            _parametersManager = new SqlParametersManager(daoHelper);
            _tablesMap = new TablesCollection();

            Parameters = new List<SqlParameter>();
            DAOHelper = daoHelper;
        }

        public SqlQueryBuilder Select(string[] fields)
        {
            fields.AssertNotNull(nameof(fields));
            return Select(fields.Select(x => new SelectField(x)));
        }

        public SqlQueryBuilder Select(IEnumerable<SelectField> fields)
        {
            fields.AssertNotNull(nameof(fields));

            fields = fields.Select(x => new SelectField(DAOHelper.EscapeField(x.Field), x.Alias));
            _selectFields = fields;
            return this;
        }

        public SqlQueryBuilder From(string alias, string tableName)
        {
            tableName.AssertHasText(nameof(tableName));
            alias.AssertHasText(nameof(alias));

            
            _tablesMap.AddIfNecessary(new TableItem(TableRole.FromTable, alias, tableName));
            _query.Append($"SELECT {_selectFields.Select(x => x.ToString()).StringJoin(",")}");
            _query.Append($" FROM {tableName} {alias}");

            return this;
        }

        public SqlQueryBuilder Where()
        {
            _query.Append(" WHERE");
            return this;
        }

        public SqlQueryBuilder True()
        {
            _query.Append(" 1 = 1");
            return this;
        }

        public SqlQueryBuilder False()
        {
            _query.Append(" 1 = 0");
            return this;
        }

        public SqlQueryBuilder Condition(string tableAlias, string fieldName, WhereOperator whereOperator, object value, bool skipIfNull = true)
        {
            string condition = WhereCondition(tableAlias, fieldName, whereOperator, value, skipIfNull);
            _query.Append($" ({condition})");
            return this;
        }

        public SqlQueryBuilder AndCondition(string tableAlias, string fieldName, WhereOperator whereOperator, object value, bool skipIfNull = true)
        {
            string condition = WhereCondition(tableAlias, fieldName, whereOperator, value, skipIfNull);
            _query.Append($" AND ({condition})");
            return this;
        }

        public SqlQueryBuilder OrCondition(string tableAlias, string fieldName, WhereOperator whereOperator, object value, bool skipIfNull = true)
        {
            string condition = WhereCondition(tableAlias, fieldName, whereOperator, value, skipIfNull);
            _query.Append($" OR ({condition})");
            return this;
        }

        public SqlQueryBuilder InnerJoin(string tableAlias, string tableName)
        {
            JoinType joinType = JoinType.InnerJoin;
            string joinClause = JoinClause(joinType, tableAlias, tableName);
            _query.Append($"{joinClause}");
            return this;
        }

        public SqlQueryBuilder LeftJoin(string tableAlias, string tableName)
        {
            JoinType joinType = JoinType.LeftJoin;
            string joinClause = JoinClause(joinType, tableAlias, tableName);
            _query.Append($"{joinClause}");
            return this;
        }

        public SqlQueryBuilder RightJoin(string tableAlias, string tableName)
        {
            JoinType joinType = JoinType.RightJoin;
            string joinClause = JoinClause(joinType, tableAlias, tableName);
            _query.Append($"{joinClause}");
            return this;
        }

        public SqlQueryBuilder FullJoin(string tableAlias, string tableName)
        {
            JoinType joinType = JoinType.FullJoin;
            string joinClause = JoinClause(joinType, tableAlias, tableName);
            _query.Append($"{joinClause}");
            return this;
        }

        public SqlQueryBuilder CrossJoin(string tableAlias, string tableName)
        {
            JoinType joinType = JoinType.CrossJoin;
            string joinClause = JoinClause(joinType, tableAlias, tableName);
            _query.Append($"{joinClause}");
            return this;
        }

        public SqlQueryBuilder OnCondition(string leftTableAlias, string leftTableFieldName, OnOperator onOperator, string rightTableAlias, string rightTableFieldName)
        {
            if (_currentJoinedTable.IsNull())
            {
                throw new ArgumentException("No join call has been made");
            }

            string condition = OnConditionImpl(leftTableAlias, leftTableFieldName, onOperator, rightTableAlias, rightTableFieldName);
            _query.Append($" ON {condition}");

            return this;
        }

        public SqlQueryBuilder AndOnCondition(string leftTableAlias, string leftTableFieldName, OnOperator onOperator, string rightTableAlias, string rightTableFieldName)
        {
            string condition = OnConditionImpl(leftTableAlias, leftTableFieldName, onOperator, rightTableAlias, rightTableFieldName);
            _query.Append($" AND ({condition})");
            return this;
        }

        public SqlQueryBuilder OrOnCondition(string leftTableAlias, string leftTableFieldName, OnOperator onOperator, string rightTableAlias, string rightTableFieldName)
        {
            string condition = OnConditionImpl(leftTableAlias, leftTableFieldName, onOperator, rightTableAlias, rightTableFieldName);
            _query.Append($" OR ({condition})");
            return this;
        }

        private string OnConditionImpl(string leftTableAlias, string leftTableFieldName, OnOperator onOperator, string rightTableAlias, string rightTableFieldName)
        {
            leftTableAlias.AssertHasText(nameof(leftTableAlias));
            leftTableFieldName.AssertHasText(nameof(leftTableFieldName));
            rightTableAlias.AssertHasText(nameof(rightTableAlias));
            rightTableFieldName.AssertHasText(nameof(rightTableFieldName));

            switch (onOperator)
            {
                case OnOperator.EqualTo:
                case OnOperator.NotEqualTo:
                case OnOperator.GreaterThan:
                case OnOperator.GreatherEqualThan:
                case OnOperator.LessEqualThan:
                case OnOperator.LessThan:

                    return $"{leftTableAlias}.{leftTableFieldName} {OnOperatorToSql(onOperator)} {rightTableAlias}.{rightTableFieldName}";
            }

            return string.Empty;
        }

        private string WhereCondition(string tableAlias, string fieldName, WhereOperator whereOperator, object value, bool skipIfNull = true)
        {
            tableAlias.AssertHasText(nameof(tableAlias));

            if(skipIfNull && IsNullOrEmptyString(value) && (whereOperator != WhereOperator.EqualTo && whereOperator != WhereOperator.EqualTo))
            {
                return string.Empty;
            }

            string originalFieldName = fieldName;
            fieldName = DAOHelper.EscapeField(fieldName);

            switch (whereOperator)
            {
                case WhereOperator.EqualTo:
                case WhereOperator.NotEqualTo:

                    if (!(IsNullOrEmptyString(value) && skipIfNull))
                    {
                        SqlParameter paramNotEqualTo = _parametersManager.BuildSqlParameter(originalFieldName, value);
                        Parameters.Add(paramNotEqualTo);
                        if (IsNullOrEmptyString(value))
                        {
                            return $"{fieldName} IS {(whereOperator == WhereOperator.EqualTo ? string.Empty : "NOT ")}NULL";
                        }

                        return $"{fieldName} {WhereOperatorToSql(whereOperator)} {paramNotEqualTo.Name}";
                    }

                    break;

                case WhereOperator.GreaterThan:
                case WhereOperator.GreatherEqualThan:
                case WhereOperator.LessEqualThan:
                case WhereOperator.LessThan:

                    SqlParameter paramLessThan = _parametersManager.BuildSqlParameter(originalFieldName, value);
                    Parameters.Add(paramLessThan);
                    return $"{fieldName} {WhereOperatorToSql(whereOperator)} {paramLessThan.Name}";
                    
                case WhereOperator.In:
                case WhereOperator.NotIn:

                    IEnumerable<object> valueList = value as IEnumerable<object>;

                    if (valueList.IsNull())
                    {
                        throw new ArgumentException("Invalid value provided for IN/NOT IN WhereOperator");
                    }

                    StringBuilder chunk = new StringBuilder($"{WhereOperatorToSql(whereOperator)}(");
                    IList<string> chunkList = new List<string>();

                    foreach (object valueItem in valueList)
                    {
                        SqlParameter paramIn = _parametersManager.BuildSqlParameter(originalFieldName, value);
                        Parameters.Add(paramIn);
                        chunkList.Add($"{paramIn.Name}");
                    }

                    chunk.Append(chunkList.StringJoin(","));
                    chunk.Append(")");

                    return chunk.ToString();

                case WhereOperator.Like:
                    SqlParameter paramLike = _parametersManager.BuildSqlParameter(originalFieldName, value);
                    Parameters.Add(paramLike);
                    return $"{fieldName} {WhereOperatorToSql(whereOperator)} %{paramLike.Name}%";
            }

            return string.Empty;
        }

        private string JoinClause(JoinType type, string tableAlias, string tableName)
        {
            tableAlias.AssertHasText(nameof(tableAlias));
            tableName.AssertHasText(nameof(tableName));

            string joinClause = null;
            TableRole role = TableRole.FromTable;

            switch(type)
            {
                case JoinType.CrossJoin:
                    joinClause = $" CROSS JOIN {tableName} {tableAlias}";
                    role = TableRole.CrossJoinTable;
                    break;
                case JoinType.FullJoin:
                    joinClause = $" FULL JOIN {tableName} {tableAlias}";
                    role = TableRole.FullJoinTable;
                    break;
                case JoinType.InnerJoin:
                    joinClause = $" INNER JOIN {tableName} {tableAlias}";
                    role = TableRole.InnerJoinTable;
                    break;
                case JoinType.LeftJoin:
                    joinClause = $" LEFT JOIN {tableName} {tableAlias}";
                    role = TableRole.LeftJoinTable;
                    break;
                case JoinType.RightJoin:
                    joinClause = $" RIGHT JOIN {tableName} {tableAlias}";
                    role = TableRole.RightJoinTable;
                    break;
            }

            TableItem tableItem = new TableItem(role, tableAlias, tableName);
            _currentJoinedTable = tableItem;
            _tablesMap.AddIfNecessary(tableItem);

            return joinClause;
        }

        private bool IsNullOrEmptyString(object value)
        {
            if (value.IsNull())
            {
                return true;
            }

            string strValue = value as string;

            if (value.IsNull())
            {
                return false;
            }

            return strValue.IsNullOrEmpty();
        }

        public SqlQueryBuilder CustomSql(string sql)
        {
            sql.AssertHasText(nameof(sql));
            _query.Append(sql);
            return this;
        }

        public override string ToString()
        {
            return _query.ToString();
        }

        private string WhereOperatorToSql(WhereOperator oper)
        {
            switch (oper)
            {
                case WhereOperator.EqualTo:
                    return "=";
                case WhereOperator.GreaterThan:
                    return ">";
                case WhereOperator.GreatherEqualThan:
                    return ">=";
                case WhereOperator.In:
                    return "IN";
                case WhereOperator.LessEqualThan:
                    return "<=";
                case WhereOperator.LessThan:
                    return "<";
                case WhereOperator.Like:
                    return "LIKE";
                case WhereOperator.NotEqualTo:
                    return "!=";
                case WhereOperator.NotIn:
                    return " NOT IN";
                default:
                    throw new ArgumentException("Invalid WhereOperator");
            }
        }

        private string OnOperatorToSql(OnOperator oper)
        {
            switch (oper)
            {
                case OnOperator.EqualTo:
                    return "=";
                case OnOperator.GreaterThan:
                    return ">";
                case OnOperator.GreatherEqualThan:
                    return ">=";
                //case OnOperator.In:
                //    return "IN";
                case OnOperator.LessEqualThan:
                    return "<=";
                case OnOperator.LessThan:
                    return "<";
                //case OnOperator.Like:
                //    return "LIKE";
                case OnOperator.NotEqualTo:
                    return "!=";
                //case OnOperator.NotIn:
                //    return " NOT IN";
                default:
                    throw new ArgumentException("Invalid WhereOperator");
            }
        }
    }
}
