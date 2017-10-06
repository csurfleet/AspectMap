using Castle.DynamicProxy;
using System;

namespace AspectMap.Core
{
    /// <summary>When implementing, please use <see cref="AttributeHandler{TAttributeType}"/> instead.</summary>
    public interface IAttributeHandler
    {
        /// <summary>A human-readable name for a handler. Mainly used for error messages.</summary>
        string HandlerName { get; }

        /// <summary>
        /// Takes an existing Action and surounds it with any required custom code. The implementation at
        /// <see cref="AttributeHandler{TAttributeType}.Surround(Action{IInvocation}, Attribute)"/> is recommended.
        /// </summary>
        /// <param name="invocation">The existing invocation.</param>
        /// <param name="sourceAttribute">The concrete attribute to assign</param>
        /// <returns>An action ready for the next step of the chain.</returns>
        Action<IInvocation> Surround(Action<IInvocation> invocation, Attribute sourceAttribute);
    }
}
