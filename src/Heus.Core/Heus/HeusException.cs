using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Heus
{
    /// <summary>
    /// Base exception type for those are thrown by Abp system for Heus specific exceptions.
    /// </summary>
    class HeusException : Exception
    {
        public HeusException()
        {

        }

        public HeusException(string message)
            : base(message)
        {

        }

        public HeusException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        public HeusException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {

        }
    }
    
    
}
