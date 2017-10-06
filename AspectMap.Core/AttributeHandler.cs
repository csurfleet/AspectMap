using Castle.DynamicProxy;
using System;

namespace AspectMap.Core
{
    /// <summary>Implement this to define logic to run around methods marked with <see cref="TAttributeType"/>.</summary>
    public abstract class AttributeHandler<TAttributeType> : IAttributeHandler where TAttributeType : Attribute
    {
        protected TAttributeType attribute;

        /// <summary>
        /// Takes an existing Action and surounds it with any required custom code. Only override when you need to do something
        /// special, the default implementation is fine for 99% of cases.
        /// </summary>
        /// <param name="invocation">The existing invocation.</param>
        /// <param name="sourceAttribute">The concrete attribute to assign</param>
        /// <returns>An action ready for the next step of the chain.</returns>
        public virtual Action<IInvocation> Surround(Action<IInvocation> invocation, Attribute sourceAttribute)
        {
            if (!(sourceAttribute is TAttributeType))
                throw new ArgumentException($"Unable to assign {HandlerName} to attribute '" + sourceAttribute.GetType() + "'.");

            attribute = (TAttributeType)sourceAttribute;

            return i => HandleInvocation(invocation, i);
        }

        /// <summary>In implementing classes, applies logic to a given method invocation.</summary>
        /// <param name="invocationChain">
        /// The chain of executing logic. This can be just the original call, or it may be a chain of <see cref="Aspect"/> implementations
        /// just like this one. Execute the inner chain by calling incovationChain(sourceInvocation); with your own code wrapped around it.
        /// </param>
        /// <param name="sourceInvocation">
        /// The original method which was called. You can access information about the running method using the various helper methods in here.
        /// </param>
        /// <example>
        /// This example will hide all exceptions from a method marked with a certain aspect. This is not a recommended implementation, it is shown here
        /// only because of it's simplicity - hiding exceptions is terrible practice!
        /// <code>
        /// protected override void HandleInvocation(Action&lt;IInvocation> invocationChain, IInvocation sourceInvocation)
        /// {
        ///   try
        ///   {
        ///     invocationChain(sourceInvocation);
        ///   }
        ///   catch { /* Never do this! */ }
        /// }
        /// </code>
        /// 
        /// Do not worry about the return values of <paramref name="sourceInvocation"/> - it will return even though you execute it as an action!
        /// </example>
        protected abstract void HandleInvocation(Action<IInvocation> invocationChain, IInvocation sourceInvocation);

        /// <summary>A human-readable name for a handler. Mainly used for error messages.</summary>
        public abstract string HandlerName { get; }
    }
}
