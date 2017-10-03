using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace AspectMap.Tests
{
    [TestClass]
    public class GeneralTests
    {
        [TestMethod]
        public void GetsInterfacesAsUsualFromStandardContainer()
        {
            ObjectFactory.Initialize(x => x.AddRegistry(new SimpleRegistry()));

            ISecondLevelClass secondLevelClass = ObjectFactory.GetInstance<ISecondLevelClass>();

            Assert.IsInstanceOfType(secondLevelClass, typeof(MySecondLevelClass));
            Assert.IsInstanceOfType(secondLevelClass.TestClass, typeof(MyTestClassNoAttributes));
        }

        [TestMethod]
        public void AttributeMultiCastsToClassLevel()
        {
            
        }
    }

    #region Helper Classes

    public class SimpleRegistry : AspectsRegistry
    {
        public SimpleRegistry()
        {
            For<ITestClass>().Use<MyTestClassNoAttributes>();
            For<ISecondLevelClass>().Use<MySecondLevelClass>();
        }
    }

    public interface ITestClass
    {
        void DoSomething();
        int GetSomething();
    }

    public class MyTestClassNoAttributes : ITestClass
    {
        public void DoSomething() { Console.WriteLine("Doing something"); }

        public int GetSomething() { return 10; }
    }

    public interface ISecondLevelClass
    {
        ITestClass TestClass { get; set; }
    }

    public class MySecondLevelClass : ISecondLevelClass
    {
        public MySecondLevelClass(ITestClass testClass)
        {
            TestClass = testClass;
        }

        public ITestClass TestClass { get; set; }
    }

    #endregion
}
