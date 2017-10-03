using System;
using AspectMap.StandardAspects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using StructureMap;

namespace AspectMap.Tests.StandardAspects
{
    [TestClass]
    public class ExceptionHandleTests
    {
        [TestMethod]
        public void HandlesException()
        {
            MockRepository repository = new MockRepository();

            IExceptionHandler exceptionHandler = repository.StrictMock<IExceptionHandler>();
            Expect.Call(() => exceptionHandler.HandleException(null)).IgnoreArguments();

            repository.ReplayAll();

            ObjectFactory.Initialize(x => x.AddRegistry(new ExceptionRegistry(exceptionHandler)));
            ITestClass testClass = ObjectFactory.GetInstance<ITestClass>();

            bool exceptionHandled = false;

            try
            {
                testClass.DoSomething();
            }
            catch(Exception ex)
            {
                if (!(ex is HandledException))
                    Assert.Fail("Expected HandledException, recieved " + ex.GetType().Name);

                exceptionHandled = true;
            }

            if (!exceptionHandled)
                Assert.Fail("No exception thrown");

            repository.VerifyAll();
        }

        public class ExceptionRegistry : AspectsRegistry
        {
            public ExceptionRegistry(IExceptionHandler exceptionHandler)
            {
                ForAspect<ExceptionHandleAttribute>().HandleWith(new ExceptionAttributeHandler(exceptionHandler));
                For<IExceptionHandler>().Use(e => exceptionHandler);

                For<ITestClass>().Use<ExceptionTestClass>().EnrichWith(AddAspectsTo<ITestClass>);
            }
        }

        public class ExceptionTestClass : ITestClass
        {
            [ExceptionHandle]
            public void DoSomething()
            {
                throw new Exception("Something went wrong");
            }

            [ExceptionHandle]
            public int GetSomething()
            {
                throw new Exception("Something went wrong again");
            }
        }
    }
}