using System;
using Castle.DynamicProxy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace AspectMap.Tests
{
    [TestClass]
    public class AttibutePropertyTests
    {
        public static int NumberValue;

        [TestMethod]
        public void GetInstanceGetsCorrectInstance()
        {
            ObjectFactory.Initialize(x => x.AddRegistry(new ItemWithPropertiesRegistry()));

            ITestClass testClass = ObjectFactory.GetInstance<ITestClass>();

            NumberValue = 0;
            testClass.DoSomething();
            Assert.AreEqual(10, NumberValue);

            NumberValue = 0;
            testClass.GetSomething();
            Assert.AreEqual(15, NumberValue);
        }
    }

    public class AttributeWithPropertiesAttribute : Attribute
    {
        public AttributeWithPropertiesAttribute(int someNumber)
        {
            SomeNumber = someNumber;
        }
        public int SomeNumber { get; set; }
    }

    public class ItemWithProperites : ITestClass
    {
        [AttributeWithProperties(10)]
        public void DoSomething()
        {
            Console.WriteLine("Doing something");
        }

        [AttributeWithProperties(15)]
        public int GetSomething()
        {
            return 5;
        }
    }

    public class ItemWithPropertiesRegistry : AspectsRegistry
    {
        public ItemWithPropertiesRegistry()
        {
            ForAspect<AttributeWithPropertiesAttribute>().HandleWith(new AttributeWithPropertiesHandler());
            For<ITestClass>().Use<ItemWithProperites>().EnrichWith(AddAspectsTo<ITestClass>);
        }
    }

    public class AttributeWithPropertiesHandler : AttributeHandler<AttributeWithPropertiesAttribute>
    {
        public override Action<IInvocation> Surround(Action<IInvocation> invocation, Attribute sourceAttribute)
        {
            if (sourceAttribute is AttributeWithPropertiesAttribute)
                AttibutePropertyTests.NumberValue = ((AttributeWithPropertiesAttribute) sourceAttribute).SomeNumber;
            else
                throw new Exception("Not the right kind of attribute");

            return invocation;
        }

        protected override void HandleInvocation(Action<IInvocation> invocation, IInvocation sourceInvocation)
        {
            invocation(sourceInvocation);
        }

        public override string HandlerName => nameof(AttributeWithPropertiesHandler);
    }
}

