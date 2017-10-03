using System;

namespace AspectMap.StandardAspects
{
    public interface IExceptionHandler
    {
        void HandleException(Exception ex);
    }
}
