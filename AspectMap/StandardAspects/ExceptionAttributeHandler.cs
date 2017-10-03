using System;
using Castle.DynamicProxy;

namespace AspectMap.StandardAspects
{
    public class ExceptionAttributeHandler : AttributeHandler<ExceptionHandleAttribute>
    {
        public ExceptionAttributeHandler(IExceptionHandler exceptionHandler)
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
                if (!(ex is HandledException))
                {
                    ExceptionHandler.HandleException(ex);
                    throw new HandledException(ex);
                }
                throw;
            }
        }
    }
}
