using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace GenericCore.Support.ExtensionMethods
{
    public static class DataExtensionMethods
    {
        public static bool IsNullOrEmpty(this DataTable table)
        {
            return table.IsNull() || table.Rows.Count == 0;
        }
    }
}
