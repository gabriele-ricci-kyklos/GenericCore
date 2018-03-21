using GenericCore.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCore.DataAccess.QueryBuilder
{
    public class QueryDbParameter
    {
        public string Name { get; }
        public object Value { get; }

        public QueryDbParameter(string name, object value)
        {
            name.AssertHasText(nameof(name));
            value.AssertNotNull(nameof(value));

            Name = name;
            Value = value;
        }
    }
}
