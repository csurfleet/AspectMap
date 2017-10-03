using Castle.DynamicProxy;
using System.Collections.Generic;
using static AspectMap.AspectsRegistry;

namespace AspectMap
{
    /// <summary>
    /// Intended to be a replacement for <see cref="AspectsRegistry"/> which removes the requirement for <see cref="StructureMap"/>.
    /// TODO: Update this comment once functionality is locked down.
    /// </summary>
    public class AspectContainer
    {
        private readonly List<AttributeMap> attributeMap = new List<AttributeMap>();

        /// <summary>Adds any aspect declarations defined in the registry to an instance. Should be used within the EnrichWith method.</summary>
        /// <typeparam name="T">The interface to add aspect declarations to.</typeparam>
        /// <param name="concreteObject">A concrete implementation to add aspect declarations to.</param>
        /// <returns>A concrete facade class containing aspect declarations.</returns>
        /// <example>
        /// <code>For&lt;ITestClass>().Use&lt;MyTestClass>().EnrichWith(AddAspectsTo&lt;ITestClass>);</code>
        /// </example>
        public T AddAspectsTo<T>(T concreteObject)
        {
            var dynamicProxy = new ProxyGenerator();
            return (T)dynamicProxy.CreateInterfaceProxyWithTargetInterface(typeof(T), concreteObject,
                new[] { (IInterceptor)new AspectInterceptor(attributeMap) });
        }

        public Aspect ForAspect<T>()
        {
            return new Aspect(typeof(T), attributeMap);
        }
    }
}
