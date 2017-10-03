using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AspectMap.StandardAspects;

namespace AspectMap.Tests.StandardAspects
{
    [TestClass]
    public class TrackMethodTelemetryHandlerTests
    {
        [TestMethod]
        public void GetInstanceFromAspectContainer()
        {
            var aspectContainer = new AspectContainer();
            aspectContainer.ForAspect<TrackMethodTelemetryAttribute>().HandleWith(new TrackMethodTelemetryHandler());

            var doSomething = new DoSomethingClass();

            var classWithAspects = aspectContainer.AddAspectsTo<IDoesSomethingInterface>(doSomething);

            Assert.AreEqual(12, classWithAspects.GetSomething("A string"));
        }

        public class DoSomethingClass : IDoesSomethingInterface
        {
            [TrackMethodTelemetry]
            public void DoSomething(int i, string s)
            {
            }

            [TrackMethodTelemetry]
            public int GetSomething(object o)
            {
                return 12;
            }
        }

        public interface IDoesSomethingInterface
        {
            void DoSomething(int i, string s);
            int GetSomething(object o);
        }
    }
}
