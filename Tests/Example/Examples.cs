// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System;
using System.Collections.Generic;

namespace Example
{
    #pragma warning disable CS0169
    public class Example1
    {
        private Dependency1 m_Dependency;

    }

    public class Dependency1 {

    }

    public class Dependency2 {}

    public class GenericDependency<T> {}

    public class Example2 {
        private IEnumerable<Dependency1> m_Dependencies;
    }

    public class Example3 
    {

        private readonly GenericDependency<Dependency1> m_Depenendency;

    }

    public class Example4
    {
        private readonly Dependency1[] m_Dependencies = new Dependency1[0];
    }

    public class Example5
    {
        private readonly Tuple<Dependency1,Dependency2> m_Depenendency;
    }


    public class Example6
    {
        private readonly Tuple<GenericDependency<Dependency1>,Dependency2> m_Depenendency;
    }

    public class Example7 
    {
       private readonly Tuple<GenericDependency<Dependency1[]>,Dependency2> m_Depenendency;
        private readonly Tuple<GenericDependency<Dependency1[]>,Dependency2> m_Depenendency2;
    }

    public interface IDependencyInterface {}

    public interface IDependencyInterface2 {}
    
    public interface IGenericDependency<T> {}

    public class ClassWithInterfaceDependency : IDependencyInterface{ }

    

    public class ClassWithSeveralInterfaceDependencies : IDependencyInterface, IDependencyInterface2 {}
    public class ClassWithGenericInterfaceDependencies : IDependencyInterface, IGenericDependency<IDependencyInterface2> {}

    public abstract class AbstractSuperClass {}
    public abstract class AbstractBaseClass : AbstractSuperClass{}

    public class DerivedClass :AbstractBaseClass {}

    public class ClassMethodArgumentDependencies {
        public void Test(Tuple<GenericDependency<Dependency1[]>,Dependency2> argument) {}
    }

    public class ClassMethod2ArgumentDependencies {
        public static void Test(Tuple<GenericDependency<Dependency1[]>,Dependency2> argument, Dependency1 argument2) {}
    }

    
    public class Class2MethodArgumentDependencies {
        public static void Test(Tuple<GenericDependency<Dependency1[]>,Dependency2> argument) {}
        public static void Test(Tuple<GenericDependency<Dependency1[]>,Dependency2> argument, string text) {}
    }

    public class ClassWithResultValue {
        public static IDependencyInterface Method() { return null; }
    }

    public class ClassWithGenericResultValue {
       public Tuple<GenericDependency<Dependency1[]>,Dependency2> Method() { return null; }
    }

    public class ClassWithMethodContainingLocals 
    {
        public void Test() 
        {
            Tuple<GenericDependency<Dependency1[]>, Dependency2> localVariable = null;
            Console.WriteLine(localVariable);

        }
    }

    public class ClassWithMethodContainingSeveralLocal
    {
        public void Test() 
        {
            Tuple<GenericDependency<Dependency1[]>, Dependency2> localVariable = null;
            string hallo = "Hallo";
            int t = 15;
            Console.WriteLine(localVariable);

            Console.WriteLine(hallo + t);

        }
    }

    public class ClassWithMethodUsingNew 
    {
        public void Test() 
        {
            Console.WriteLine(new Dependency1());
        }
    }

    public class ClassWithMethodUsingCast 
    {
        private IDependencyInterface m_Dependency;
        
        public void Test() 
        {
            Console.WriteLine((ClassWithInterfaceDependency)m_Dependency);
        }
    }
    public class StaticProvider 
    {
        public static IDependencyInterface StaticReference {get { return new ClassWithInterfaceDependency(); } }
    }

    internal class StaticProvider2 
    {
        internal const int StaticReference = 5;
    }


    public class ClassWithMethodUsingStaticReferences
    {
        
        
        public void Test() 
        {
            Console.WriteLine(StaticProvider.StaticReference);
        }
    }

    
    public class ClassWithMethodUsingDirectStaticReferences
    {
        
        
        public void Test() 
        {
            Console.WriteLine(StaticProvider2.StaticReference);
        }
    }
  
    public class DependencyAttribute : Attribute { }


    public enum EnumDependency
    {
        [Dependency]
        Value1,
        Value2
    }
    public class ClassWithEnumDependency
    {
        public EnumDependency EnumDependency { get; set; }
        private void Test(EnumDependency enumDependency) { }
    }

    [Dependency]
    public class ClassWithAttributeDependency
    {

    }

    public class ClassWithAttributeDependencyAtConstructor
    {
        [Dependency]
        public ClassWithAttributeDependencyAtConstructor([Dependency] string test) { }
    }


    public class ClassWithMethodAttributeDependency
    {
        [Dependency]
        public void MethodWithAttributeDependency([Dependency] string test) { }
    }

    public class ClassWithPropertyAttributeDependency
    {

        public bool  MethodWithAttributeDependency { [Dependency] get; [Dependency] set; }
    }

    public class ClassWithSimplePropertyAttributeDependency
    {
        [Dependency]
        public bool MethodWithAttributeDependency { get; set; }
    }

    public class ClassWithSimplePropertyDependency
    {
        public bool MethodWithAttributeDependency { get; set; }
    }

    public class ClassWithFieldAttributeDependency
    {
        [Dependency]
        private string m_FieldWithAttributeDependency;
    }

    [Dependency]
    public interface IInterfaceWithAttributeDependency
    {

        void MethodWithAttributeDependency();
    }

    public  class  ClassWithMethodAttributeDependency2
    {
        [return: Dependency]
        public  void MethodWithAttributeDependency()
        {
           
        }
    }

    public class ClassWithMethodAttributeDependency3
    {
        
        public void MethodWithAttributeDependency([Dependency]int parameter1, [Dependency]string parameter2)
        {
        }
    }

    public class ClassWithAttributeDependencyAtEvent
    {
        [Dependency]
        public event EventHandler EventWithAttributeDependency;
    }

    public class ClassWithAttributeDependencyAtDelegate
    {
        [Dependency]
        public delegate MyEventHandler MyEventHandler(object sender, EventArgs e);
    }

    public delegate void MyEventHandler(object sender, EventArgs e);

    public class ClassWithDelegateDependencyAtEvent
    {
        
        public event MyEventHandler MyEventHandler;
    }

    public class ClassWithNestedClass
    {
        public class NestedClass
        {
            
        }   
    }



#pragma warning restore CS0169
}
