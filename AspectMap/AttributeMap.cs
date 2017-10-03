using System;
using System.Linq;

namespace AspectMap
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
                if (value.BaseType != typeof(Attribute))
                    throw new Exception("Aspect's attribute must inherit directly from Attribute.");
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
