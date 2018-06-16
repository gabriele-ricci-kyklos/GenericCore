using GenericCore.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCore.DataAccess.SqlParameters
{
    public class SqlParameter
    {
        public Type Type { get; }
        public string Name { get; }
        public object Value { get; }

        public SqlParameter(string name, object value)
        {
            name.AssertHasText(nameof(name));
            value.AssertNotNull(nameof(value));

            Name = name;
            Value = value;
            Type = value.GetType();
        }
    }
}
