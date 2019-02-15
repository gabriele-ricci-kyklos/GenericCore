using GenericCore.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCore.DataAccess.QueryBuilder
{
    public class JoinInfo
    {
        public TableItem LeftTable { get; }
        public TableItem RightTable { get; set; }

        public JoinInfo(TableItem leftTable)
        {
            leftTable.AssertNotNull(nameof(leftTable));
            LeftTable = leftTable;
        }
    }
}
