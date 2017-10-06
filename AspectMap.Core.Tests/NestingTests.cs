using Castle.DynamicProxy;
using FluentAssertions;
using NUnit.Framework;
using System;

namespace AspectMap.Core.Tests
{
    [TestFixture]
    public class NestingTests
    {
        private static string _output;

        [Test]
        public void NestingUsesCorrectOrder()
        {
            var container = new AspectContainer();
            container.ForAspect<NestingLevel1Attribute>().HandleWith(new Level1Handler());
            container.ForAspect<NestingLevel2Attribute>().HandleWith(new Level2Handler());

            var testClass = container.AddAspectsTo<ITestClass>(new ItemWithMultipleAttributes());
            _output = "";

            testClass.DoSomething();

            _output.Should().Be("Level2 Entered\nLevel1 Entered\nDoSomething\nLevel1 Exited\nLevel2 Exited\n");
        }

        [Test]
        public void NestingWithPriorityUsesCorrectOrder()
        {
            var container = new AspectContainer();
            container.ForAspect<NestingLevel1Attribute>().WithPriority(1).HandleWith(new Level1Handler());
            container.ForAspect<NestingLevel2Attribute>().WithPriority(2).HandleWith(new Level2Handler());

            var testClass = container.AddAspectsTo<ITestClass>(new ItemWithMultipleAttributes());
            _output = "";

            testClass.DoSomething();

            _output.Should().Be("Level1 Entered\nLevel2 Entered\nDoSomething\nLevel2 Exited\nLevel1 Exited\n");
        }

        public class NestingLevel1Attribute : Attribute { }
        public class NestingLevel2Attribute : Attribute { }

        public class ItemWithMultipleAttributes : ITestClass
        {
            [NestingLevel1]
            [NestingLevel2]
            public void DoSomething()
            {
                _output += "DoSomething\n";
                Console.WriteLine("DoSomething");
            }

            public int GetSomething() { throw new NotImplementedException(); }
        }

        public class Level1Handler : AttributeHandler<NestingLevel1Attribute>
        {
            protected override void HandleInvocation(Action<IInvocation> invocation, IInvocation sourceInvocation)
            {
                _output += "Level1 Entered\n";
                Console.WriteLine("Level1 Entered");
                invocation(sourceInvocation);
                _output += "Level1 Exited\n";
                Console.WriteLine("Level1 Exited");
            }

            public override string HandlerName => nameof(Level1Handler);
        }

        public class Level2Handler : AttributeHandler<NestingLevel2Attribute>
        {
            public override string HandlerName => throw new NotImplementedException();

            protected override void HandleInvocation(Action<IInvocation> invocation, IInvocation sourceInvocation)
            {
                _output += "Level2 Entered\n";
                Console.WriteLine("Level2 Entered");
                invocation(sourceInvocation);
                _output += "Level2 Exited\n";
                Console.WriteLine("Level2 Exited");
            }
        }
    }
}
