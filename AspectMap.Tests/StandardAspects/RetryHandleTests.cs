using System;
using AspectMap.StandardAspects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace AspectMap.Tests.StandardAspects
{
    [TestClass]
    public class RetryHandleTests
    {
        [TestMethod]
        public void GetInstanceGetsCorrectInstance()
        {
            ObjectFactory.Initialize(x => x.AddRegistry(new RetryRegistry()));
            ITestClass testClass = ObjectFactory.GetInstance<ITestClass>();

            Assert.AreEqual(12, testClass.GetSomething());

            try
            {
                testClass.DoSomething();
                Assert.Fail("NotImplementedException was NOT thrown");
            }
            catch(NotImplementedException)
            {}
        }

        [TestMethod]
        public void GetInstanceFromAspectContainer()
        {
            var aspectContainer = new AspectContainer();
            aspectContainer.ForAspect<RetryAttribute>().HandleWith(new RetryAttributeHandler());

            var classForRetries = new RetryClass();
            var classWithAspects = aspectContainer.AddAspectsTo<ITestClass>(classForRetries);

            Assert.AreEqual(12, classWithAspects.GetSomething());
        }

        public class RetryRegistry: AspectsRegistry
        {
            public RetryRegistry()
            {
                ForAspect<RetryAttribute>().HandleWith(new RetryAttributeHandler());
                For<ITestClass>().Use<RetryClass>().EnrichWith(AddAspectsTo<ITestClass>);
            }
        }

        public class RetryClass : ITestClass
        {
            private int failedDoSomethings;
            private int failedGetSomethings;

            [Retry(3, typeof(NotImplementedException))]
            public void DoSomething()
            {
                if (failedDoSomethings < 3)
                {
                    Console.WriteLine("Throwing NotImplementedException");
                    failedDoSomethings++;
                    throw new NotImplementedException();
                }
            }

            [Retry(7, typeof(ArgumentException))]
            public int GetSomething()
            {
                if (failedGetSomethings < 6)
                {
                    Console.WriteLine("Throwing ArgumentException");
                    failedGetSomethings++;
                    throw new ArgumentException();
                }

                return 12;
            }
        }
    }

    
}
