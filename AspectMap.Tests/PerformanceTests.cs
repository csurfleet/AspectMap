using System;
using System.Diagnostics;
using System.Timers;
using Castle.DynamicProxy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace AspectMap.Tests
{
    [TestClass]
    public class PerformanceTests
    {
        [TestMethod]
        public void TimeNonCachedLoadEachTime()
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            ObjectFactory.Initialize(x => x.AddRegistry(new PerformanceRegistry()));

            for (int i = 0; i < 500; i++)
            {
                ITestClass testClass = ObjectFactory.GetInstance<ITestClass>();
                ITestClassTwo testClassTwo = ObjectFactory.GetInstance<ITestClassTwo>();

                testClass.DoSomething();
                testClassTwo.DoSomething();
                testClassTwo.GetSomething();
                testClass.GetSomething();
            }

            timer.Stop();
            Console.WriteLine(timer.ElapsedMilliseconds.ToString());
        }

        [TestMethod]
        public void TimeNonCachedLoadOnce()
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            ObjectFactory.Initialize(x => x.AddRegistry(new PerformanceRegistry()));

            ITestClass testClass = ObjectFactory.GetInstance<ITestClass>();
            ITestClassTwo testClassTwo = ObjectFactory.GetInstance<ITestClassTwo>();

            for (int i = 0; i < 500; i++)
            {
                testClass.DoSomething();
                testClassTwo.DoSomething();
                testClassTwo.GetSomething();
                testClass.GetSomething();
            }

            timer.Stop();
            Console.WriteLine(timer.ElapsedMilliseconds.ToString());
        }
    }

    public class PerformanceAttribute : Attribute {}

    public class PerformanceTwoAttribute : Attribute { public int Level { get; set; } }

    public class ItemForPerformanceTestsOne : ITestClass
    {
        [PerformanceTwo(Level = 2)]
        public void DoSomething()
        {
            int i = 5*5;
        }

        public int GetSomething()
        {
            return 5;
        }
    }

    public interface ITestClassTwo : ITestClass { }

    [Performance]
    public class ItemForPerformanceTestsTwo : ITestClassTwo
    {
        public void DoSomething()
        {
            int i = 15*15;
        }

        public int GetSomething()
        {
            return 10;
        }
    }

    public class PerformanceHandler : AttributeHandler<PerformanceAttribute>
    {
        public override string HandlerName => throw new NotImplementedException();

        protected override void HandleInvocation(Action<IInvocation> invocation, IInvocation sourceInvocation)
        {
            int n = 150 * 27;
            invocation(sourceInvocation);
        }
    }

    public class PerformanceTwoHandler : AttributeHandler<PerformanceTwoAttribute>
    {
        public override string HandlerName => throw new NotImplementedException();

        protected override void HandleInvocation(Action<IInvocation> invocation, IInvocation sourceInvocation)
        {
            int n = 150 * attribute.Level;
            invocation(sourceInvocation);
        }
    }

    public class PerformanceRegistry : AspectsRegistry
    {
        public PerformanceRegistry()
        {
            ForAspect<PerformanceAttribute>().HandleWith(new PerformanceHandler());
            ForAspect<PerformanceTwoAttribute>().HandleWith(new PerformanceTwoHandler());
            For<ITestClass>().Use<ItemForPerformanceTestsOne>().EnrichWith(AddAspectsTo<ITestClass>);
            For<ITestClassTwo>().Use<ItemForPerformanceTestsTwo>().EnrichWith(AddAspectsTo<ITestClassTwo>);
        }
    }
}
