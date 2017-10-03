using System;

namespace AspectMap.StandardAspects
{
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