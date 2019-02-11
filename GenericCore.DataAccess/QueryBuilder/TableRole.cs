using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCore.DataAccess.QueryBuilder
{
    public enum TableRole
    {
        FromTable,
        InnerJoinTable,
        LeftJoinTable,
        RightJoinTable,
        FullJoinTable,
        CrossJoinTable
    }
}
