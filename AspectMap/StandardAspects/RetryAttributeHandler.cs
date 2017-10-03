using System;
using Castle.DynamicProxy;

namespace AspectMap.StandardAspects
{
    public class RetryAttributeHandler : AttributeHandler<RetryAttribute>
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
