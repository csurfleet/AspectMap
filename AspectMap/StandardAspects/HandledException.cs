using System;

namespace AspectMap.StandardAspects
{
    public class HandledException : Exception
    {
        public HandledException(Exception innerException) : base(innerException.Message, innerException)
        { }
    }
}
