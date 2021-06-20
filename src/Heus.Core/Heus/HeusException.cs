using System;
using System.Runtime.Serialization;

namespace Heus
{
    /// <summary>
    /// Base exception type for those are thrown by Abp system for Abp specific exceptions.
    /// </summary>
    public class HeusException : Exception
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
