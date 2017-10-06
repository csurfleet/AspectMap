using Castle.DynamicProxy;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;

namespace AspectMap.Core.Tests
{
    /// <summary>A set of tests verifying the behaviour of a single aspect being defined differently at class and method level.</summary>
    [TestFixture]
    public class MulticastTests
    {
        public static string MethodOutput;

        public static int ValOutput;

        /// <summary>An attribute applied at class level should propagate its behaviour to every method definied in it's interface.</summary>
        [Test]
        public void AttibuteOnClassHandledByAllMethods()
        {
            var container = new AspectContainer();
            container.ForAspect<AttributeForMultiCast>().HandleWith(new MultiCastHandler());

            var testClass = container.AddAspectsTo<ITestClass>(new ItemWithClassLevelMultiCast());

            MethodOutput = "";

            testClass.DoSomething();

            MethodOutput.Should().Be("DoSomething enteredDoing somethingDoSomething exited");

            MethodOutput = "";
            testClass.GetSomething();
            MethodOutput.Should().Be("GetSomething enteredGetSomething exited");
        }

        /// <summary>An attribute defined at method level should override the behavior of the same attribute defined at class level.</summary>
        [Test]
        public void AttributesNestCorrectly()
        {
            var container = new AspectContainer();
            container.ForAspect<AttributeForMultiCast>().HandleWith(new MultiCastHandler());

            var testClass = container.AddAspectsTo<ITestClass>(new ItemWithClassLevelMultiCastAndMethodOverride());

            ValOutput = 0;

            testClass.DoSomething();

            ValOutput.Should().Be(2, "because DoSomething should use the value defined in the class level attribute");

            testClass.GetSomething();

            ValOutput.Should().Be(15, "because GetSomething should use the value defined at the method level");
        }
    }

    public class AttributeForMultiCast : Attribute
    {
        public AttributeForMultiCast() { Val = 2; }

        public AttributeForMultiCast(int val) { Val = val; }

        public int Val { get; set; }
    }

    [AttributeForMultiCast]
    public class ItemWithClassLevelMultiCast : ITestClass
    {
        public void DoSomething()
        {
            Console.WriteLine("Doing something");
            MulticastTests.MethodOutput += "Doing something";
        }

        public int GetSomething() => 5;
    }

    [AttributeForMultiCast]
    public class ItemWithClassLevelMultiCastAndMethodOverride : ITestClass
    {
        public void DoSomething() => Console.WriteLine("Done something");

        [AttributeForMultiCast(15)]
        public int GetSomething() => 5;
    }

    public class MultiCastHandler : AttributeHandler<AttributeForMultiCast>
    {
        protected override void HandleInvocation(Action<IInvocation> invocation, IInvocation sourceInvocation)
        {
            Console.WriteLine(sourceInvocation.Method.Name + " entered");
            MulticastTests.MethodOutput += sourceInvocation.Method.Name + " entered";
            invocation(sourceInvocation);
            Console.WriteLine(sourceInvocation.Method.Name + " exited");
            MulticastTests.MethodOutput += sourceInvocation.Method.Name + " exited";
            MulticastTests.ValOutput = attribute.Val;
        }

        public override string HandlerName => nameof(MultiCastHandler);
    }
}
