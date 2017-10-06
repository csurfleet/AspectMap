using Castle.DynamicProxy;
using FluentAssertions;
using NUnit.Framework;
using System;

namespace AspectMap.Core.Tests
{
    [TestFixture]
    public class AttibutePropertyTests
    {
        public static int NumberValue;

        [Test]
        public void AfterAspectIsAttached_HandlerIsCalled()
        {
            var container = new AspectContainer();
            container.ForAspect<AttributeWithPropertiesAttribute>().HandleWith(new AttributeWithPropertiesHandler());

            var testClass = container.AddAspectsTo<ITestClass>(new ItemWithProperites());

            NumberValue = 0;
            testClass.DoSomething();
            NumberValue.Should().Be(10, "because AttributeWithPropertiesAttribute should have set it");

            NumberValue = 0;
            testClass.GetSomething().Should().Be(5, "because that is the returned value from ItemWithProperties.GetSomething()");
            NumberValue.Should().Be(15, "because AttributeWithPropertiesAttribute should have set it");
        }
    }

    public class AttributeWithPropertiesAttribute : Attribute
    {
        public AttributeWithPropertiesAttribute(int someNumber) { SomeNumber = someNumber; }
        public int SomeNumber { get; set; }
    }

    public class ItemWithProperites : ITestClass
    {
        [AttributeWithProperties(10)]
        public void DoSomething() => Console.WriteLine("Doing something");

        [AttributeWithProperties(15)]
        public int GetSomething() => 5;
    }

    public class AttributeWithPropertiesHandler : AttributeHandler<AttributeWithPropertiesAttribute>
    {
        protected override void HandleInvocation(Action<IInvocation> invocation, IInvocation sourceInvocation)
        {
            AttibutePropertyTests.NumberValue = attribute.SomeNumber;
            invocation(sourceInvocation);
        }

        public override string HandlerName => nameof(AttributeWithPropertiesHandler);
    }
}
