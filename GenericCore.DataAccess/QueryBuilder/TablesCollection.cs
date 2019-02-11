using GenericCore.Support;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCore.DataAccess.QueryBuilder
{
    public class TablesCollection : KeyedCollection<string, TableItem>
    {
        public TableItem FromTable => this.FirstOrDefault(x => x.Role == TableRole.FromTable);

        protected override string GetKeyForItem(TableItem item)
        {
            return item.TableAlias;
        }

        public void AddIfNecessary(TableItem item)
        {
            item.AssertNotNull(nameof(item));

            if (Contains(GetKeyForItem(item)))
            {
                return;
            }

            Add(item);
        }
    }
}
