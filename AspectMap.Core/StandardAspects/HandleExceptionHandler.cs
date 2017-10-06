using Castle.DynamicProxy;
using System;

namespace AspectMap.Core.StandardAspects
{
    public class HandleExceptionHandler : AttributeHandler<HandleExceptionAttribute>
    {
        public HandleExceptionHandler(IExceptionHandler exceptionHandler)
        {
            ExceptionHandler = exceptionHandler;
        }

        public IExceptionHandler ExceptionHandler { get; set; }

        public override string HandlerName => "Exception Handler";

        protected override void HandleInvocation(Action<IInvocation> invocation, IInvocation sourceInvocation)
        {
            try
            {
                invocation(sourceInvocation);
            }
            catch (Exception ex)
            {
                // By throwing the HandledException, we can ensure that the exception is only handled once as it traverses the call chain
                if (!(ex is HandledException))
                {
                    ExceptionHandler.HandleException(ex);
                    throw new HandledException(ex);
                }
                throw;
            }
        }
    }

    public interface IExceptionHandler
    {
        void HandleException(Exception ex);
    }

    public class HandledException : Exception
    {
        public HandledException(Exception innerException) : base(innerException.Message, innerException)
        { }
    }
}
