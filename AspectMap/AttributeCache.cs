using System;
using System.Collections.Generic;

namespace AspectMap
{
    internal class AttributeCache
    {
        public AttributeCache(string method, List<Attribute> attributes)
        {
            Method = method;
            Attributes = attributes;
        }

        public string Method { get; set; }
        public List<Attribute> Attributes { get; set; }
    }

    internal interface IAttributeCache : IList<AttributeCache>
    {}

    internal class AttributeCacheList : List<AttributeCache>, IAttributeCache
    {}
}
