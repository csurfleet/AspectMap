using Castle.DynamicProxy;
using System;

namespace AspectMap.Core.StandardAspects
{
    public class RetryHandler : AttributeHandler<RetryAttribute>
    {
        public override string HandlerName => "Retry Handler";

        protected override void HandleInvocation(Action<IInvocation> invocation, IInvocation sourceInvocation)
        {
            for (int count = 1; count <= attribute.MaxTries; count++)
            {
                try
                {
                    invocation(sourceInvocation);
                    return;
                }
                catch (Exception ex)
                {
                    if (ex.GetType() != attribute.TargetException || count == attribute.MaxTries)
                        throw;
                }
            }
        }
    }
}
