using System;

namespace AspectMap.Core.StandardAspects
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class TrackMethodTelemetryAttribute : Attribute
    {}
}
