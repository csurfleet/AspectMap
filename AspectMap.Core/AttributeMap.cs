using System;
using System.Reflection;

namespace AspectMap.Core
{
    internal class AttributeMap
    {
        public AttributeMap(Type attribute, IAttributeHandler attributeHandler, int priority)
        {
            this.attribute = attribute;
            this.attributeHandler = attributeHandler;
            Priority = priority;
        }

        private Type attribute;
        private IAttributeHandler attributeHandler;

        public Type Attribute
        {
            get { return attribute; }
            set
            {
                if (!typeof(Attribute).IsAssignableFrom(value))
                    throw new Exception("Aspect's attribute must inherit from Attribute.");
                attribute = value;
            }
        }

        public IAttributeHandler AttributeHandler
        {
            get { return attributeHandler; }
            set { attributeHandler = value; }
        }

        public int Priority { get; set; }
    }
}
