using System;
using System.Runtime.Serialization;

namespace Heus
{
    /// <summary>
    /// 业务异常
    /// </summary>
    public class BussinessException : Exception
    {
       

        public BussinessException(string message)
            : base(message)
        {

        }

        public BussinessException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        public BussinessException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {

        }
    }
}
