using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericCore.Support;

namespace GenericCore.DataAccess.QueryBuilder
{
    public class TableItem
    {
        public TableRole Role { get; }
        public string TableAlias { get; }
        public string TableName { get; }

        public TableItem(TableRole role, string tableAlias, string tableName)
        {
            tableAlias.AssertHasText(nameof(tableAlias));

            Role = role;
            TableAlias = tableAlias;
            TableName = tableName;
        }
    }
}
