using Castle.DynamicProxy;
using System;
using System.Collections.Generic;

namespace AspectMap.Core
{
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
            return (T)dynamicProxy.CreateInterfaceProxyWithTargetInterface(typeof(T), concreteObject, new[] { (IInterceptor)new AspectInterceptor(attributeMap) });
        }

        /// <summary>Define an <see cref="Attribute"/> to attach behavior to.</summary>
        /// <typeparam name="T">The attribute which will be applied throughout your code to specify where to attach aspects.</typeparam>
        /// <returns>An <see cref="Aspect"/> ready for behavior to be attached to.</returns>
        public Aspect ForAspect<T>() where T : Attribute => new Aspect(typeof(T), attributeMap);
    }
}
