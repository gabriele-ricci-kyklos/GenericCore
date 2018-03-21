using GenericCore.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCore.DataAccess.DAOHelper
{
    public class OracleDAOHelper : IDAOHelper
    {
        public string ParameterStartPrefix
        {
            get
            {
                return ":";
            }
        }

        public string EscapeField(string fieldName)
        {
            fieldName.AssertNotNull(nameof(fieldName));
            return $"\"{fieldName}\"";
        }
    }
}
