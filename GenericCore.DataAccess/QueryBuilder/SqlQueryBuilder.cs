﻿using GenericCore.DataAccess.DAOHelper;
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

        public IDAOHelper DAOHelper { get; private set; }
        public IList<QueryDbParameter> Parameters { get; private set; }

        public SqlQueryBuilder(IDAOHelper daoHelper)
        {
            daoHelper.AssertNotNull(nameof(daoHelper));

            _query = new StringBuilder();
            Parameters = new List<QueryDbParameter>();
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

            fields = fields.Select(x => new SelectField(DAOHelper.EscapeField(x.Name), x.Alias));

            _query.Append($"SELECT {fields.Select(x => x.ToString()).StringJoin(",")}");
            return this;
        }

        public SqlQueryBuilder From(string tableName, string alias)
        {
            tableName.AssertHasText(nameof(tableName));
            alias.AssertHasText(nameof(alias));

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

        private string WhereCondition(string tableAlias, string fieldName, WhereOperator whereOperator, object value, bool skipIfNull = true)
        {
            tableAlias.AssertHasText(nameof(tableAlias));

            if(skipIfNull && IsNullOrEmptyString(value) && (whereOperator != WhereOperator.EqualTo && whereOperator != WhereOperator.EqualTo))
            {
                return string.Empty;
            }

            string originalFieldName = fieldName;
            fieldName = DAOHelper.EscapeField(fieldName);

            string paramName;
            switch (whereOperator)
            {
                case WhereOperator.EqualTo:
                case WhereOperator.NotEqualTo:

                    if (!(IsNullOrEmptyString(value) && skipIfNull))
                    {
                        paramName = BuildParameterName(originalFieldName);
                        Parameters.Add(new QueryDbParameter(paramName, value));
                        if(IsNullOrEmptyString(value))
                        {
                            return $"{fieldName} IS NULL";
                        }

                        return $"{fieldName} {WhereOperatorToSql(whereOperator)} {paramName}";
                    }

                    break;

                case WhereOperator.GreaterThan:
                case WhereOperator.GreatherEqualThan:
                case WhereOperator.LessEqualThan:
                case WhereOperator.LessThan:

                    paramName = BuildParameterName(originalFieldName);
                    Parameters.Add(new QueryDbParameter(paramName, value));
                    return $"{fieldName} {WhereOperatorToSql(whereOperator)} {paramName}";
                    
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
                        paramName = BuildParameterName(originalFieldName);
                        Parameters.Add(new QueryDbParameter(paramName, valueItem));
                        chunkList.Add($"{paramName}");
                    }

                    chunk.Append(chunkList.StringJoin(","));
                    chunk.Append(")");

                    return chunk.ToString();
            }

            return string.Empty;
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

        private string BuildParameterName(string fieldName)
        {
            string pname =
                _paramNamePrefix +
                fieldName
                .ToEmptyIfNull()
                .Replace(' ', '_')
                .SafeGetLeftPart(8)
                .ToLowerInvariant();

            return $"{DAOHelper.ParameterStartPrefix}{pname}_{Parameters.Count}";
        }

        public SqlQueryBuilder CustomSql(string sql)
        {
            sql.AssertHasText(sql);
            _query.Append(sql);
            return this;
        }

        public override string ToString()
        {
            return _query.ToString();
        }

        private string WhereOperatorToSql(WhereOperator oper)
        {
            switch(oper)
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
    }
}