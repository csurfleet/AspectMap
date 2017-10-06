using Castle.DynamicProxy;
using NUnit.Framework;
using System;
using System.Diagnostics;

#pragma warning disable CS0219 // Variable is assigned but its value is never used
namespace AspectMap.Core.Tests
{
    [TestFixture]
    public class PerformanceTests
    {
        [Test]
        public void TimeNonCachedLoad500Times()
        {
            var timer = new Stopwatch();
            timer.Start();
            var container = new AspectContainer();
            container.ForAspect<PerformanceAttribute>().HandleWith(new PerformanceHandler());
            container.ForAspect<PerformanceTwoAttribute>().HandleWith(new PerformanceTwoHandler());

            for (int i = 0; i < 500; i++)
            {
                var testClass = container.AddAspectsTo<ITestClass>(new ItemForPerformanceTestsOne());
                var testClassTwo = container.AddAspectsTo<ITestClassTwo>(new ItemForPerformanceTestsTwo());

                testClass.DoSomething();
                testClassTwo.DoSomething();
                testClassTwo.GetSomething();
                testClass.GetSomething();
            }

            timer.Stop();
            Console.WriteLine(timer.ElapsedMilliseconds.ToString());
        }

        [Test]
        public void TimeNonCachedLoadOnce()
        {
            var timer = new Stopwatch();
            timer.Start();
            var container = new AspectContainer();
            container.ForAspect<PerformanceAttribute>().HandleWith(new PerformanceHandler());
            container.ForAspect<PerformanceTwoAttribute>().HandleWith(new PerformanceTwoHandler());

            var testClass = container.AddAspectsTo<ITestClass>(new ItemForPerformanceTestsOne());
            var testClassTwo = container.AddAspectsTo<ITestClassTwo>(new ItemForPerformanceTestsTwo());

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

        public class PerformanceAttribute : Attribute { }

        public class PerformanceTwoAttribute : Attribute { public int Level { get; set; } }

        public class ItemForPerformanceTestsOne : ITestClass
        {
            [PerformanceTwo(Level = 2)]
            public void DoSomething() { int i = 5 * 5; }

            public int GetSomething() => 5;
        }

        public interface ITestClassTwo : ITestClass { }

        [Performance]
        public class ItemForPerformanceTestsTwo : ITestClassTwo
        {
            public void DoSomething() { int i = 15 * 15; }

            public int GetSomething() => 10;
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
    }
}
#pragma warning restore CS0219 // Variable is assigned but its value is never used