using System;
using System.Collections.Generic;
using Castle.DynamicProxy;
using StructureMap.Configuration.DSL;

namespace AspectMap
{
    /// <summary>A <see cref="Registry"/> with added functionality to handle aspect declarations.</summary>
    public abstract class AspectsRegistry : Registry
    {
        private readonly List<AttributeMap> attributeMap = new List<AttributeMap>();

        protected AspectsRegistry()
        {
            For<IAttributeCache>().Singleton().Use<AttributeCacheList>();
        }

        /// <summary>Adds any aspect declarations defined in the registry to an instance. Should be used within the EnrichWith method.</summary>
        /// <typeparam name="T">The interface to add aspect declarations to.</typeparam>
        /// <param name="concreteObject">A concrete implementation to add aspect declarations to.</param>
        /// <returns>A concrete facade class containing aspect declarations.</returns>
        /// <example>
        /// <code>For&lt;ITestClass>().Use&lt;MyTestClass>().EnrichWith(AddAspectsTo&lt;ITestClass>);</code>
        /// </example>
        public T AddAspectsTo<T>(T concreteObject)
        {
            ProxyGenerator dynamicProxy = new ProxyGenerator();
            return (T)dynamicProxy.CreateInterfaceProxyWithTargetInterface(typeof(T), concreteObject,
                new[] { (IInterceptor)new AspectInterceptor(attributeMap) });
        }

        protected Aspect ForAspect<T>()
        {
            return new Aspect(typeof(T), attributeMap);
        }

        public class Aspect
        {
            private readonly Type attribute;
            private readonly List<AttributeMap> attributeMap;
            private int aspectPriority;

            internal Aspect(Type attribute, List<AttributeMap> attributeMap)
            {
                if (attribute.BaseType != typeof(Attribute))
                    throw new Exception("Aspect's attribute must inherit directly from Attribute.");
                
                this.attribute = attribute;
                this.attributeMap = attributeMap;   
            }

            public Aspect WithPriority(int priority)
            {
                aspectPriority = priority;
                return this;
            }

            public void HandleWith<T>(T item) where T : IAttributeHandler
            {
                attributeMap.Add(new AttributeMap(attribute, item, aspectPriority));
            }
        }
    }
}
