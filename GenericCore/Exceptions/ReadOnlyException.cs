using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace GenericCore.Exceptions
{
    public class ReadOnlyException : Exception
    {
        public ReadOnlyException()
            : base()
        {
        }

        public ReadOnlyException(string message)
            : base(message)
        {
        }

        public ReadOnlyException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected ReadOnlyException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
