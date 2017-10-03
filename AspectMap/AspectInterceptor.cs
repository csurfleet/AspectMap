using System;
using System.Collections.Generic;
using System.Linq;
using Castle.DynamicProxy;
using StructureMap;

namespace AspectMap
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
                Attribute attributeOnMethod = Attribute.GetCustomAttribute(invocation.GetConcreteMethodInvocationTarget(), map.Attribute);

                if (attributeOnMethod == null)
                    attributeOnMethod = Attribute.GetCustomAttribute(invocation.GetConcreteMethodInvocationTarget().ReflectedType, map.Attribute);

                if (attributeOnMethod != null)
                {
                    Action<IInvocation> previous = surrounds;
                    surrounds = map.AttributeHandler.Surround(previous, attributeOnMethod);
                }
            }

            surrounds(invocation);
        }
    }
}