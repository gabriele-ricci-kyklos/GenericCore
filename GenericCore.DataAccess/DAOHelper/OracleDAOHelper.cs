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
        private string _providerInvariantName;

        public string ParameterStartPrefix
        {
            get
            {
                return ":";
            }
        }

        public string ProviderInvariantName
        {
            get
            {
                return _providerInvariantName;
            }
        }

        public string EscapeField(string fieldName)
        {
            fieldName.AssertNotNull(nameof(fieldName));
            return $"\"{fieldName}\"";
        }

        public OracleDAOHelper(string providerName)
        {
            providerName.AssertHasText("providerName");
            _providerInvariantName = providerName;
        }
    }
}
