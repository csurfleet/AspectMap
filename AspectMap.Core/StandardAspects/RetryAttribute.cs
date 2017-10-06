using System;

namespace AspectMap.Core.StandardAspects
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class RetryAttribute : Attribute
    {
        public RetryAttribute(int maxTries, Type targetException)
        {
            MaxTries = maxTries;
            TargetException = targetException;
        }

        public int MaxTries { get; set; }
        public Type TargetException { get; set; }
    }
}
