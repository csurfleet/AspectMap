using System;
using Castle.DynamicProxy;

namespace AspectMap
{
    public interface IAttributeHandler
    {
        string HandlerName { get; }

        /// <summary>Takes an existing Action and surounds it with any required custom code.</summary>
        /// <param name="invocation">The existing invocation.</param>
        /// <param name="sourceAttribute">The attribute which caused the  </param>
        /// <returns>An action ready for the next step of the chain.</returns>
        Action<IInvocation> Surround(Action<IInvocation> invocation, Attribute sourceAttribute);
    }
    /// <summary>Classes implementing this will handle custom code for particular aspects.</summary>
    public abstract class AttributeHandler<TAttributeType> : IAttributeHandler where TAttributeType : Attribute
    {
        protected TAttributeType attribute;

        /// <summary>Takes an existing Action and surounds it with any required custom code.</summary>
        /// <param name="invocation">The existing invocation.</param>
        /// <param name="sourceAttribute">The attribute which caused the  </param>
        /// <returns>An action ready for the next step of the chain.</returns>
        public virtual Action<IInvocation> Surround(Action<IInvocation> invocation, Attribute sourceAttribute)
        {
            if (!(sourceAttribute is TAttributeType))
                throw new ArgumentException($"Unable to assign {HandlerName} to attribute '" + sourceAttribute.GetType() + "'.");

            attribute = (TAttributeType)sourceAttribute;

            return i => HandleInvocation(invocation, i);
        }

        protected abstract void HandleInvocation(Action<IInvocation> invocation, IInvocation sourceInvocation);

        public abstract string HandlerName { get; }
    }
}
