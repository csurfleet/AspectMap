using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AspectMap.StandardAspects
{
    public class TrackMethodTelemetryAttribute : Attribute
    {
        public int Importance { get; set; }
    }
}
