// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using YADA.Core.Analyser;

namespace YADA.Test
{
    public class TypeLoaderTests
    {
        [Test]
        public void TypeLoader_Recognizes_Private_NonGenericType_Member_As_Dependency() 
        {
            var type = FetchType<Example.Example1>();
           
             DependencyAssertion
            .ForType(type)
            .Expected<Example.Dependency1>()
            .EnsureNoOtherReferences()
            .Ensure();
        }

           [Test]
        public void TypeLoader_Recognizes_Private_GenericEnumerable_Member_As_Dependency() 
        {
            var type = FetchType<Example.Example2>();
            
            DependencyAssertion
            .ForType(type)
            .Expected<Example.Dependency1>()
            .Expected(typeof(IEnumerable<>))
            .EnsureNoOtherReferences()
            .Ensure();
        }

        [Test]
        public void TypeLoader_Recognizes_Private_GenericType_Member_As_Dependency() 
        {
            var type = FetchType<Example.Example3>();
            
            DependencyAssertion
            .ForType(type)
            .Expected<Example.Dependency1>()
            .Expected(typeof(Example.GenericDependency<>))
            .EnsureNoOtherReferences()
            .Ensure();
        }

        [Test]
        public void  TypeLoader_Recognizes_Private_Array_Member_As_Dependency() 
        {
            var type = FetchType<Example.Example4>();
         
            DependencyAssertion
            .ForType(type)
            .Expected<Example.Dependency1>()
            .EnsureNoOtherReferences()
            .Ensure();
        }

        [Test]
        public void TypeLoader_Recognizes_Private_Tuple_Member_As_Dependency() 
        {
            var type = FetchType<Example.Example5>();
       
            DependencyAssertion
            .ForType(type)
            .Expected<Example.Dependency1>()
            .Expected<Example.Dependency2>()
            .Expected(typeof(Tuple<,>))
            .EnsureNoOtherReferences()
            .Ensure();
        }

        [Test]
        public void TypeLoader_Recognizes_Private_ComplexTuple_Member_As_Dependency() 
        {
            var type = FetchType<Example.Example6>();
            
            DependencyAssertion
            .ForType(type)
            .Expected<Example.Dependency1>()
            .Expected<Example.Dependency2>()
            .Expected(typeof(Example.GenericDependency<>))
            .Expected(typeof(Tuple<,>))
            .EnsureNoOtherReferences()
            .Ensure();

     
        }

        [Test]
        public void TypeLoader_Recognizes_Two_Private_ComplexTuple_Member_As_Dependency() 
        {
            var type = FetchType<Example.Example7>();
         
            DependencyAssertion
                .ForType(type)
                .Expected<Example.Dependency1>(2)
                .Expected<Example.Dependency2>(2)
                .Expected(typeof(Example.GenericDependency<>),2)
                .Expected(typeof(Tuple<,>),2)
                .EnsureNoOtherReferences(true)
                .Ensure();
        }
     
        [Test]
        public void TypeLoader_Recognizes_ImplementedInterface_As_Dependency() 
        {
             var type = FetchType<Example.ClassWithInterfaceDependency>();

            DependencyAssertion
            .ForType(type)
            .Expected<Example.IDependencyInterface>()
            .EnsureNoOtherReferences()
            .Ensure();
        }

        [Test]
        public void TypeLoader_Recognizes_ImplementedInterfaces_As_Dependencies() 
        {
            var type = FetchType<Example.ClassWithSeveralInterfaceDependencies>();

            PrintType(type);
            DependencyAssertion
            .ForType(type)
            .Expected<Example.IDependencyInterface>()
            .Expected<Example.IDependencyInterface2>()
            .EnsureNoOtherReferences()
            .Ensure();
        }

        
        [Test]
        public void TypeLoader_Recognizes_ImplementedGenericInterfaces_As_Dependencies() 
        {
            var type = FetchType<Example.ClassWithGenericInterfaceDependencies>();
            PrintType(type);
            DependencyAssertion
            .ForType(type)
            .Expected<Example.IDependencyInterface>()
            .Expected<Example.IDependencyInterface2>()
            .Expected(typeof(Example.IGenericDependency<>))
            .EnsureNoOtherReferences()
            .Ensure();
        }

        [Test]
        public void TypeLoader_Recognizes_BaseClass_As_Dependency()
        {
            var type = FetchType<Example.DerivedClass>();
                       
            DependencyAssertion
                .ForType(type)
                .Expected<Example.AbstractBaseClass>()
                .EnsureNoOtherReferences()
                .Ensure();
        }

        [Test]
        public void TypeLoader_Recognizes_GenericMethodArgument_As_Dependencies() 
        {
            var type = FetchType<Example.ClassMethodArgumentDependencies>();
                       
            DependencyAssertion
                .ForType(type)
                .Expected<Example.Dependency1>()
                .Expected<Example.Dependency2>()
                .Expected(typeof(Tuple<,>))
                .Expected(typeof(Example.GenericDependency<>))
                .EnsureNoOtherReferences()
                .Ensure();
        }
        
        [Test]
        public void TypeLoader_Recognizes_SeveralMethodArguments_As_Dependencies() 
        {
            var type = FetchType<Example.ClassMethod2ArgumentDependencies>();
            
            DependencyAssertion
                .ForType(type)
                .Expected<Example.Dependency1>()
                .Expected<Example.Dependency2>(2)
                .Expected(typeof(Tuple<,>))
                .Expected(typeof(Example.GenericDependency<>))
                .EnsureNoOtherReferences(true)
                .Ensure();
        }

        [Test]
        public void TypeLoader_Recognizes_Arguments_Of_SeveralMethod_As_Dependencies()
        {
            var type = FetchType<Example.Class2MethodArgumentDependencies>();
            
            DependencyAssertion
                .ForType(type)
                .Expected<Example.Dependency1>(2)
                .Expected<Example.Dependency2>(2)
                .Expected(typeof(Tuple<,>),2)
                .Expected(typeof(Example.GenericDependency<>),2)
                .Expected<string>()
                .EnsureNoOtherReferences(true)
                .Ensure();
        }

        [Test]
        public void TypeLoader_Recognizes_ResultValue_As_Dependency()
        {
            var type = FetchType<Example.ClassWithResultValue>();

            DependencyAssertion
                .ForType(type)
                .Expected<Example.IDependencyInterface>(1)
                .EnsureNoOtherReferences()
                .Ensure();
        }

        [Test]
        public void TypeLoader_Recognizes_GenericResultValue_As_Dependencies()
        {
            var type = FetchType<Example.ClassWithGenericResultValue>();
         
            DependencyAssertion
                .ForType(type)
                .Expected<Example.Dependency1>(1)
                .Expected<Example.Dependency2>(1)
                .Expected(typeof(Tuple<,>),1)
                .Expected(typeof(Example.GenericDependency<>),1)
                .EnsureNoOtherReferences()
                .Ensure();
        }

        [Test]
        public void TypeLoader_Recognizes_Method_LocalVariables_As_Dependencies()
        {
            var type = FetchType<Example.ClassWithMethodContainingLocals>();
            
            DependencyAssertion
                .ForType(type)
                .Expected<Example.Dependency1>(1)
                .Expected<Example.Dependency2>(1)
                .Expected(typeof(Tuple<,>),1)
                .Expected(typeof(Example.GenericDependency<>),1)
                .EnsureNoOtherReferences()
                .Ensure();
        }

        [Test]
        public void TypeLoader_Recognizes_Methods_LocalVariables_As_Dependencies()
        {
            var type = FetchType<Example.ClassWithMethodContainingSeveralLocal>();
            
            DependencyAssertion
                .ForType(type)
                .Expected<Example.Dependency1>(1)
                .Expected<Example.Dependency2>(1)
                .Expected(typeof(Tuple<,>),1)
                .Expected(typeof(Example.GenericDependency<>),1)
                .Expected<string>()
                .Expected<int>()
                .EnsureNoOtherReferences()
                .Ensure();
        }

        
        [Test]
        public void TypeLoader_Recognizes_New_Created_Object_In_Methods_As_Dependencies()
        {
            var type = FetchType<Example.ClassWithMethodUsingNew>();
            
            DependencyAssertion
                .ForType(type)
                .Expected<Example.Dependency1>(1)
                .EnsureNoOtherReferences()
                .Ensure();
        }
        
        [Test]
        public void TypeLoader_Recognizes_Casts_To_Specific_Type_In_Methods_As_Dependencies()
        {
            var type = FetchType<Example.ClassWithMethodUsingCast>();
            
            DependencyAssertion
                .ForType(type)
                .Expected<Example.IDependencyInterface>()
                .Expected<Example.ClassWithInterfaceDependency>()
                .EnsureNoOtherReferences()
                .Ensure();
        }

        [Test]
        public void TypeLoader_Recognizes_References_To_Static_Properties_In_Methods_As_Dependencies()
        {
            var type = FetchType<Example.ClassWithMethodUsingStaticReferences>();
            
            DependencyAssertion
                .ForType(type)
                
                .Expected<Example.IDependencyInterface>()
                .Expected<Example.StaticProvider>()
                .EnsureNoOtherReferences()
                .Ensure();
        }
        
        [Test]
        public void TypeLoader_Recognizes_References_To_Static_Instances_In_Methods_As_Dependencies_But_Not_The_Static_ClassType()
        {
            var type = FetchType<Example.ClassWithMethodUsingDirectStaticReferences>();
            PrintType(type);
            DependencyAssertion
                .ForType(type)
                //Direct used public fields of static classes are not tracable in il code
                //.Expected<Example.ClassWithMethodUsingStaticReferences>(1)
                .Expected<int>()
                .EnsureNoOtherReferences()
                .Ensure();
        }

        [Test]
        public void TypeLoader_Can_Load_Assembly_Specified_By_FileName() 
        {
            var sut = new TypeLoader(new[] { @"./YADA.Example.dll" });
            var types = sut.GetTypes();
            Assert.NotNull(types);
        }


        [Test]
        public void TypeLoader_Can_Analyse_Assembly() 
        {
            var sut = new TypeLoader(new[] { @"./YADA.Example.dll" });
            var types = sut.GetTypes();
            
            foreach(var type in types) 
            {
                PrintType(type);
                TestContext.WriteLine("--------------------------------------------------------------");
            }
        }
        
        [Test]
        public void TypeLoader_GetTypes_Does_Not_Contain_Module() 
        {
            var sut = new TypeLoader(new[] { @"./YADA.Example.dll" });
            var types = sut.GetTypes();
            var t = types.Select(r => r.FullName).ToList();
            Assert.That(types.Select(r => r.FullName), Does.Not.Contain("<Module>"));

        }

        private void PrintType(ITypeDescription type)
        {
            TestContext.WriteLine(type.FullName);
            foreach (var dependency in type.Dependencies)
            {
                TestContext.WriteLine($"    ->{dependency.Type.FullName} ({dependency.Occurrence}) :");
                foreach(var context in dependency.Contexts) {
                    TestContext.WriteLine($"        {context}");
                }
            }
        }


        class DependencyAssertion 
        {
            private readonly ITypeDescription m_Type;
            private int m_ExpectedReferences;
            private bool m_IgnoreSystemDotObject;
            private bool m_CountOccurrences = true;
            private readonly List<Action> m_Assertions = new List<Action>();

            public static DependencyAssertion ForType(ITypeDescription type) 
            {
                return new DependencyAssertion(type);
            }
            private DependencyAssertion(ITypeDescription type) 
            {
                m_Type = type;
            }

            public DependencyAssertion Expected<T>() 
            {
                return Expected(typeof(T));
            }

            public DependencyAssertion Expected(Type type)
            {
                return Expected(type, 1);
            }
            public DependencyAssertion Expected<T>(int times) 
            {
                return Expected(typeof(T), times);
            }

            public DependencyAssertion Expected(Type type, int times) 
            {
                m_ExpectedReferences += times;

                m_Assertions.Add(() => InternalAssertDependsOn(type, times));

                return this;
            }

            public DependencyAssertion EnsureNoOtherReferences(bool countOccurences = false, bool ignoreSystemDotObject = true) 
            {
                m_CountOccurrences = countOccurences;
                m_IgnoreSystemDotObject = ignoreSystemDotObject;
                m_Assertions.Add(InternalEnsureNoOtherReferences);

                return this;
            }

            public void Ensure()
            {
                foreach(var action in m_Assertions) 
                {
                    action();
                }
            }
            
            private void InternalEnsureNoOtherReferences() 
            {

                var count = 0;
                foreach(var d in m_Type.Dependencies) 
                {
                    if (!(d.Type.FullName == typeof(System.Object).FullName && m_IgnoreSystemDotObject))
                    {
                        count += m_CountOccurrences ? d.Occurrence : 1;
                    }
                }

                Assert.That(count, Is.EqualTo(m_ExpectedReferences), $"At type {m_Type.FullName} are {m_ExpectedReferences} expected but {count} are available");
            }

         
            private void InternalAssertDependsOn( Type dependency, int? times = null) 
            {
                Assert.That(DependsOn(dependency), Is.True, $"The type {m_Type.FullName} should depend on {dependency.FullName}", times);
            }

            private bool DependsOn(Type dependency, int? times = null)
            {
                bool result;

                var dependencyHolder = m_Type.Dependencies.FirstOrDefault(t=>t.Type.FullName == dependency.FullName);
                
                result = dependencyHolder != null;
                
                if ( result && times != null )
                {
                    result = dependencyHolder.Occurrence == times;
                }

                return result;
            }

        }

        private ITypeDescription FetchType<T>() 
        {
            return TypeLoader.GetType(typeof(T).FullName, typeof(T).Assembly.Location);
        }
    }


}