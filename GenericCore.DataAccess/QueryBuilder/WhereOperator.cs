using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCore.DataAccess.QueryBuilder
{
    public enum WhereOperator
    {
        EqualTo,
        NotEqualTo,
        GreaterThan,
        GreatherEqualThan,
        LessThan,
        LessEqualThan,
        Like,
        In,
        NotIn
    }
}
