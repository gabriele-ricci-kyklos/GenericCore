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
        public string Name { get; }
        public string Alias { get; }

        public SelectField(string name, string alias = null)
        {
            name.AssertNotNull(nameof(name));

            Name = name;
            Alias = alias;
        }

        public override string ToString()
        {
            return $"{Name}{((!Alias.IsNullOrEmpty()) ? $" AS {Alias}" : string.Empty)}";
        }
    }
}
