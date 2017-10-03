using System;
using Castle.DynamicProxy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace AspectMap.Tests
{
    [TestClass]
    public class MultiCastTests
    {
        public static string MethodOutput;

        public static int ValOutput;

        [TestMethod]
        public void AttibuteOnClassHandledByAllMethods()
        {
            ObjectFactory.Initialize(x => x.AddRegistry(new MultiCastRegistry()));

            ITestClass testClass = ObjectFactory.GetInstance<ITestClass>();
            MethodOutput = "";

            testClass.DoSomething();

            Assert.AreEqual("DoSomething enteredDoing somethingDoSomething exited", MethodOutput);

            MethodOutput = "";
            testClass.GetSomething();
            Assert.AreEqual("GetSomething enteredGetSomething exited", MethodOutput);

            ObjectFactory.ResetDefaults();
        }

        [TestMethod]
        public void AttributesNestCorrectly()
        {
            ObjectFactory.Initialize(x => x.AddRegistry(new MultiCastNestingRegistry()));

            ITestClass testClass = ObjectFactory.GetInstance<ITestClass>();

            ValOutput = 0;

            testClass.DoSomething();

            Assert.AreEqual(2, ValOutput);

            testClass.GetSomething();

            Assert.AreEqual(15, ValOutput);

            ObjectFactory.ResetDefaults();
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
            MultiCastTests.MethodOutput += "Doing something";
        }

        public int GetSomething()
        {
            return 5;
        }
    }

    [AttributeForMultiCast]
    public class ItemWithClassLevelMultiCastAndMethodOverride : ITestClass
    {
        public void DoSomething()
        {
            Console.WriteLine("Done something");
        }

        [AttributeForMultiCast(15)]
        public int GetSomething()
        {
            return 5;
        }
    }

    public class MultiCastHandler : AttributeHandler<AttributeForMultiCast>
    {
        protected override void HandleInvocation(Action<IInvocation> invocation, IInvocation sourceInvocation)
        {
            Console.WriteLine(invocation.Method.Name + " entered");
            MultiCastTests.MethodOutput += sourceInvocation.Method.Name + " entered";
            invocation(sourceInvocation);
            Console.WriteLine(invocation.Method.Name + " exited");
            MultiCastTests.MethodOutput += sourceInvocation.Method.Name + " exited";
            MultiCastTests.ValOutput = attribute.Val;
        }

        public override string HandlerName => nameof(MultiCastHandler);
    }

    public class MultiCastRegistry : AspectsRegistry
    {
        public MultiCastRegistry()
        {
            ForAspect<AttributeForMultiCast>().HandleWith(new MultiCastHandler());
            For<ITestClass>().Use<ItemWithClassLevelMultiCast>().EnrichWith(AddAspectsTo<ITestClass>);
        }
    }

    public class MultiCastNestingRegistry : AspectsRegistry
    {
        public MultiCastNestingRegistry()
        {
            ForAspect<AttributeForMultiCast>().HandleWith(new MultiCastHandler());
            For<ITestClass>().Use<ItemWithClassLevelMultiCastAndMethodOverride>().EnrichWith(AddAspectsTo<ITestClass>);
        }
    }
}
