using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCore.DataAccess
{
    public interface IDAOHelper
    {
        string ParameterStartPrefix { get; }
        string EscapeField(string fieldName);
    }
}
