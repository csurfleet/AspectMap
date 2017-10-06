using System;
using System.Collections.Generic;
using System.Reflection;

namespace AspectMap.Core
{
    /// <summary>An <see cref="Aspect"/> is the result of combining an <see cref="IAttributeHandler"/> with an associated <see cref="Attribute"/>.</summary>
    public class Aspect
    {
        private readonly Type attribute;
        private readonly List<AttributeMap> attributeMap;
        private int aspectPriority;

        internal Aspect(Type attribute, List<AttributeMap> attributeMap)
        {
            if (!typeof(Attribute).IsAssignableFrom(attribute))
                throw new Exception("Aspect's attribute must inherit from Attribute.");

            this.attribute = attribute;
            this.attributeMap = attributeMap;
        }

        /// <summary>
        /// Allows you to set the priority order for when multiple aspects are applied to a single method. An <see cref="Aspect"/>
        /// with a lower priority will be wrapped around one with a higher priority. Aspects default to a priority of zero, so when using
        /// priorities it is recommended to apply a priority to every <see cref="Aspect"/>.
        /// </summary>
        /// <param name="priority">The priority to set.</param>
        /// <returns>This <see cref="Aspect"/> with the priority attached.</returns>
        public Aspect WithPriority(int priority)
        {
            aspectPriority = priority;
            return this;
        }

        /// <summary>Links an <see cref="Attribute"/> to an <see cref="IAttributeHandler"/> which defines the behavior to apply.</summary>
        /// <typeparam name="T">The type of the handler.</typeparam>
        /// <param name="item">The handler to use to apply logic to methods marked with this <see cref="Aspect"/>'s attribute.</param>
        public void HandleWith<T>(T item) where T : IAttributeHandler
        {
            attributeMap.Add(new AttributeMap(attribute, item, aspectPriority));
        }
    }
}
