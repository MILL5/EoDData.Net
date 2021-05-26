using System;

namespace EoDData.Net
{
    public class EoDDataHttpException : Exception
    {
        public EoDDataHttpException(string message)
        : base(message)
        {
        }
    }
}