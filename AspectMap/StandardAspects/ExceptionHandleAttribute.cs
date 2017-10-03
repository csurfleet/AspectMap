using System;

namespace AspectMap.StandardAspects
{
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Method, AllowMultiple = false)]
    public class ExceptionHandleAttribute : Attribute
    {}
}