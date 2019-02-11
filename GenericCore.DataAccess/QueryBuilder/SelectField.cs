using GenericCore.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCore.DataAccess.QueryBuilder
{
    public class SelectField
    {
        public string Alias { get; }
        public string Field { get; }
        public string Name { get; }
        

        public SelectField(string alias, string field, string name)
        {
            field.AssertNotNull(nameof(field));
            
            Alias = alias;
            Field = field;
            Name = name;
        }

        public SelectField(string alias, string field)
            : this(alias, field, null)
        {
        }

        public SelectField(string field)
            : this(null, field, null)
        {
        }

        public override string ToString()
        {
            return $"{Field}.{Alias}{((!Name.IsNullOrEmpty()) ? $" AS {Name}" : string.Empty)}";
        }
    }
}
