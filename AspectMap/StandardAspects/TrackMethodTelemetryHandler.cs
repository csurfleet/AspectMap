using Castle.DynamicProxy;
using System;

namespace AspectMap.StandardAspects
{
    public class TrackMethodTelemetryHandler : AttributeHandler<TrackMethodTelemetryAttribute>
    {
        public override string HandlerName => "Track Method Telemetry";

        protected override void HandleInvocation(Action<IInvocation> invocation, IInvocation sourceInvocation)
        {
            var methodFullName = sourceInvocation.Method.Name;
            var parameters = sourceInvocation.Method.GetParameters();

            Console.WriteLine($"Method '{methodFullName}' was invoked with {parameters.Length} parameters");

            invocation(sourceInvocation);
        }
    }
}
