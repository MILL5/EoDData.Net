using System;
using System.Runtime.Serialization;

namespace EoDData.Net
{
    [Serializable]
    public class EoDDataException : Exception
    {
        public EoDDataException(string message)
        : base(message)
        {
        }

        protected EoDDataException(SerializationInfo info, StreamingContext context)
        : base(info, context)
        {
        }
    }
}