using System;
using System.Runtime.Serialization;

namespace EoDData.Net
{
    [Serializable]
    public class EoDDataHttpException : Exception
    {
        public EoDDataHttpException(string message)
        : base(message)
        {
        }

        protected EoDDataHttpException(SerializationInfo info, StreamingContext context)
        : base(info, context)
        {
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}