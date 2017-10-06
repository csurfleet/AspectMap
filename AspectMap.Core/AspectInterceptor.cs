using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AspectMap.Core
{
    internal class AspectInterceptor : IInterceptor
    {
        public AspectInterceptor(List<AttributeMap> attributeMap)
        {
            this.attributeMap = attributeMap;
        }

        private readonly List<AttributeMap> attributeMap;

        public void Intercept(IInvocation invocation)
        {
            Action<IInvocation> surrounds = i => i.Proceed();

            #region Caching code
            /*
            string stub = invocation.GetConcreteMethodInvocationTarget().ReflectedType.FullName + "." + invocation.GetConcreteMethodInvocationTarget();
            
            IAttributeCache attributeCache = ObjectFactory.GetInstance<IAttributeCache>();

            AttributeCache cachedInfo = attributeCache.FirstOrDefault(c => c.Method == stub);

            if (cachedInfo == null)
            {
                cachedInfo = new AttributeCache(stub, null);
                List<Attribute> attributes = new List<Attribute>();
                foreach (AttributeMap map in attributeMap.OrderByDescending(a => a.Priority))
                {
                    Attribute attributeOnMethod = Attribute.GetCustomAttribute(invocation.GetConcreteMethodInvocationTarget(), map.Attribute);

                    if (attributeOnMethod == null)
                        attributeOnMethod = Attribute.GetCustomAttribute(invocation.GetConcreteMethodInvocationTarget().ReflectedType, map.Attribute);

                    if (attributeOnMethod != null)
                        attributes.Add(attributeOnMethod);
                }
                if (attributes.Any())
                    cachedInfo.Attributes = attributes;

                attributeCache.Add(cachedInfo);
            }

            if (cachedInfo.Attributes != null)
            {
                foreach (Attribute attribute in cachedInfo.Attributes)
                {
                    Action<IInvocation> previous = surrounds;
                    AttributeHandler handler = (AttributeHandler)ObjectFactory.Container.GetInstance(map.AttributeHandler);
                    surrounds = handler.Surround(previous, attribute);
                }
            }
            */
            #endregion

            // TODO: This section could be improved with the addition of a singleton caching mechanism so we're not using reflection
            // each time a method is called
            foreach (AttributeMap map in attributeMap.OrderByDescending(a => a.Priority))
            {
                // First get the attribute assigned to the method itself
                var attribute = invocation.GetConcreteMethodInvocationTarget().GetCustomAttribute(map.Attribute);

                // If not found, we try for the attribute assigned to the whole type
                if (attribute == null)
                    attribute = invocation.GetConcreteMethodInvocationTarget().DeclaringType.GetTypeInfo().GetCustomAttribute(map.Attribute);

                // TODO: We should also try and check any interfaces for attributes on the method/type, so that we can assign the AoP behaviour
                // there instead of in each implementing class

                if (attribute != null)
                {
                    Action<IInvocation> previous = surrounds;
                    surrounds = map.AttributeHandler.Surround(previous, attribute);
                }
            }

            surrounds(invocation);
        }
    }
}
