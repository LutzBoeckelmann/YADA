// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System;
using System.Collections.Generic;
using System.Linq;
using Example;
using NUnit.Framework;
using YADA.Analyzer;

namespace YADA.AnalyzerTests
{
    public class TypeLoaderTests
    {
        [Test]
        public void TypeLoader_Recognizes_Private_NonGenericType_Member_As_Dependency()
        {
            var type = FetchType<Example1>();

            DependencyAssertion
                .ForType(type)
                .Expected<Dependency1>()
                .EnsureNoOtherReferences()
                .Ensure();
        }

        [Test]
        public void TypeLoader_Recognizes_Private_GenericEnumerable_Member_As_Dependency()
        {
            var type = FetchType<Example2>();

            DependencyAssertion
                .ForType(type)
                .Expected<Dependency1>()
                .Expected(typeof(IEnumerable<>))
                .EnsureNoOtherReferences()
                .Ensure();
        }

        [Test]
        public void TypeLoader_Recognizes_Private_GenericType_Member_As_Dependency()
        {
            var type = FetchType<Example3>();

            DependencyAssertion
                .ForType(type)
                .Expected<Dependency1>()
                .Expected(typeof(GenericDependency<>))
                .EnsureNoOtherReferences()
                .Ensure();
        }

        [Test]
        public void TypeLoader_Recognizes_Private_Array_Member_As_Dependency()
        {
            var type = FetchType<Example4>();

            DependencyAssertion
                .ForType(type)
                .Expected<Dependency1>(Times.ThreeTimes)
                .EnsureNoOtherReferences()
                .Ensure();
        }

        [Test]
        public void TypeLoader_Recognizes_Private_Tuple_Member_As_Dependency()
        {
            var type = FetchType<Example5>();

            DependencyAssertion
                .ForType(type)
                .Expected<Dependency1>()
                .Expected<Dependency2>()
                .Expected(typeof(Tuple<,>))
                .EnsureNoOtherReferences()
                .Ensure();
        }

        [Test]
        public void TypeLoader_Recognizes_Private_ComplexTuple_Member_As_Dependency()
        {
            var type = FetchType<Example6>();

            DependencyAssertion
                .ForType(type)
                .Expected<Dependency1>()
                .Expected<Dependency2>()
                .Expected(typeof(GenericDependency<>))
                .Expected(typeof(Tuple<,>))
                .EnsureNoOtherReferences()
                .Ensure();


        }

        [Test]
        public void TypeLoader_Recognizes_Two_Private_ComplexTuple_Member_As_Dependency()
        {
            var type = FetchType<Example7>();

            DependencyAssertion
                .ForType(type)
                .Expected<Dependency1>(Times.Twice)
                .Expected<Dependency2>(Times.Twice)
                .Expected(typeof(GenericDependency<>), Times.Twice)
                .Expected(typeof(Tuple<,>), Times.Twice)
                .EnsureNoOtherReferences(true)
                .Ensure();
        }

        [Test]
        public void TypeLoader_Recognizes_ImplementedInterface_As_Dependency()
        {
            var type = FetchType<ClassWithInterfaceDependency>();

            DependencyAssertion
                .ForType(type)
                .Expected<IDependencyInterface>()
                .EnsureNoOtherReferences()
                .Ensure();
        }

        [Test]
        public void TypeLoader_Recognizes_ImplementedInterfaces_As_Dependencies()
        {
            var type = FetchType<ClassWithSeveralInterfaceDependencies>();

            //            PrintType(type);
            DependencyAssertion
                .ForType(type)
                .Expected<IDependencyInterface>()
                .Expected<IDependencyInterface2>()
                .EnsureNoOtherReferences()
                .Ensure();
        }


        [Test]
        public void TypeLoader_Recognizes_ImplementedGenericInterfaces_As_Dependencies()
        {
            var type = FetchType<ClassWithGenericInterfaceDependencies>();
            // PrintType(type);
            DependencyAssertion
                .ForType(type)
                .Expected<IDependencyInterface>()
                .Expected<IDependencyInterface2>()
                .Expected(typeof(IGenericDependency<>))
                .EnsureNoOtherReferences()
                .Ensure();
        }

        [Test]
        public void TypeLoader_Recognizes_BaseClass_As_Dependency()
        {
            var type = FetchType<DerivedClass>();

            DependencyAssertion
                .ForType(type)
                .Expected<AbstractBaseClass>(Times.NoCounting)
                .EnsureNoOtherReferences()
                .Ensure();
        }

        [Test]
        public void TypeLoader_Recognizes_GenericMethodArgument_As_Dependencies()
        {
            var type = FetchType<ClassMethodArgumentDependencies>();

            DependencyAssertion
                .ForType(type)
                .Expected<Dependency1>()
                .Expected<Dependency2>()
                .Expected(typeof(Tuple<,>))
                .Expected(typeof(GenericDependency<>))
                .EnsureNoOtherReferences()
                .Ensure();
        }

        [Test]
        public void TypeLoader_Recognizes_SeveralMethodArguments_As_Dependencies()
        {
            var type = FetchType<ClassMethod2ArgumentDependencies>();

            DependencyAssertion
                .ForType(type)
                .Expected<Dependency1>(Times.Twice)
                .Expected<Dependency2>()
                .Expected(typeof(Tuple<,>))
                .Expected(typeof(GenericDependency<>))
                .EnsureNoOtherReferences(true)
                .Ensure();
        }

        [Test]
        public void TypeLoader_Recognizes_Arguments_Of_SeveralMethod_As_Dependencies()
        {
            var type = FetchType<Class2MethodArgumentDependencies>();

            DependencyAssertion
                .ForType(type)
                .Expected<Dependency1>(Times.Twice)
                .Expected<Dependency2>(Times.Twice)
                .Expected(typeof(Tuple<,>), Times.Twice)
                .Expected(typeof(GenericDependency<>), Times.Twice)
                .Expected<string>()
                .EnsureNoOtherReferences(true)
                .Ensure();
        }

        [Test]
        public void TypeLoader_Recognizes_ResultValue_As_Dependency()
        {
            var type = FetchType<ClassWithResultValue>();

            DependencyAssertion
                .ForType(type)
                .Expected<IDependencyInterface>(Times.NoCounting)
                .EnsureNoOtherReferences()
                .Ensure();
        }

        [Test]
        public void TypeLoader_Recognizes_GenericResultValue_As_Dependencies()
        {
            var type = FetchType<ClassWithGenericResultValue>();

            DependencyAssertion
                .ForType(type)
                .Expected<Dependency1>(Times.NoCounting)
                .Expected<Dependency2>(Times.NoCounting)
                .Expected(typeof(Tuple<,>), Times.NoCounting)
                .Expected(typeof(GenericDependency<>), Times.NoCounting)
                .EnsureNoOtherReferences()
                .Ensure();
        }

        [Test]
        public void TypeLoader_Recognizes_Method_LocalVariables_As_Dependencies()
        {
            var type = FetchType<ClassWithMethodContainingLocals>();

            DependencyAssertion
                .ForType(type)
                .Expected<Dependency1>(Times.Once)
                .Expected<Dependency2>(Times.Once)
                .Expected(typeof(Tuple<,>), Times.Once)
                .Expected(typeof(GenericDependency<>), Times.Once)
                .EnsureNoOtherReferences()
                .Ensure();
        }

        [Test]
        public void TypeLoader_Recognizes_Methods_LocalVariables_As_Dependencies()
        {
            var type = FetchType<ClassWithMethodContainingSeveralLocal>();

            DependencyAssertion
                .ForType(type)
                .Expected<Dependency1>(Times.Once)
                .Expected<Dependency2>(Times.Once)
                .Expected(typeof(Tuple<,>), Times.Once)
                .Expected(typeof(GenericDependency<>), Times.Once)
                .Expected<string>(Times.NoCounting)
                .Expected<int>()
                .EnsureNoOtherReferences()
                .Ensure();
        }


        [Test]
        public void TypeLoader_Recognizes_New_Created_Object_In_Methods_As_Dependencies()
        {
            var type = FetchType<ClassWithMethodUsingNew>();

            DependencyAssertion
                .ForType(type)
                .Expected<Dependency1>(Times.Once)
                .EnsureNoOtherReferences()
                .Ensure();
        }

        [Test]
        public void TypeLoader_Recognizes_Casts_To_Specific_Type_In_Methods_As_Dependencies()
        {
            var type = FetchType<ClassWithMethodUsingCast>();

            DependencyAssertion
                .ForType(type)
                .Expected<IDependencyInterface>(Times.NoCounting)
                .Expected<ClassWithInterfaceDependency>()
                .EnsureNoOtherReferences()
                .Ensure();
        }

        [Test]
        public void TypeLoader_Recognizes_References_To_Static_Properties_In_Methods_As_Dependencies()
        {
            var type = FetchType<ClassWithMethodUsingStaticReferences>();

            DependencyAssertion
                .ForType(type)

                .Expected<IDependencyInterface>()
                .Expected<StaticProvider>()
                .EnsureNoOtherReferences()
                .Ensure();
        }

        [Test]
        public void
            TypeLoader_Recognizes_References_To_Static_Instances_In_Methods_As_Dependencies_But_Not_The_Static_ClassType()
        {
            var type = FetchType<ClassWithMethodUsingDirectStaticReferences>();
            DependencyAssertion
                .ForType(type)
                //Direct used public fields of static classes are not tracable in il code
                //.Expected<ClassWithMethodUsingStaticReferences>(1)
                .Expected<int>()
                .EnsureNoOtherReferences()
                .Ensure();
        }

        [Test]
        public void TypeLoader_Recognizes_References_To_Attributes_At_Class_Level()
        {
            var type = FetchType<ClassWithAttributeDependency>();
            DependencyAssertion
                .ForType(type)

                .Expected<DependencyAttribute>()
                .ExpectedContext(nameof(IDependencyContextVisitor.ClassAttributeContext), "", "")
                .EnsureNoOtherReferences()
                .Ensure();
        }

        [Test]
        public void TypeLoader_Recognizes_References_To_Attributes_At_Method_Level()
        {
            var type = FetchType<ClassWithMethodAttributeDependency>();
            DependencyAssertion
                .ForType(type)

                .Expected<DependencyAttribute>()
                .Expected<DependencyAttribute>()
                .ExpectedContext(nameof(IDependencyContextVisitor.MethodAttributeContext),
                    nameof(ClassWithMethodAttributeDependency.MethodWithAttributeDependency), "")
                .ExpectedContext(nameof(IDependencyContextVisitor.MethodAttributeContext),
                    nameof(ClassWithMethodAttributeDependency.MethodWithAttributeDependency), "")
                .EnsureNoOtherReferences()
                .Ensure();
        }

        [Test]
        public void TypeLoader_Recognizes_Enum_Dependencies()
        {
            var type = FetchType<ClassWithEnumDependency>();
            DependencyAssertion.ForType(type)

                .Expected<EnumDependency>(Times.NoCounting)
                
                .EnsureNoOtherReferences()
                .Ensure();
        }
        [Test]
        public void TypeLoader_Recognizes_Attribute_Dependencies_AtEnums()
        {
            var type = FetchType<EnumDependency>();
            DependencyAssertion.ForType(type)

                .Expected<DependencyAttribute>(Times.Once)
                .Expected <EnumDependency>(Times.Twice) // any value is a dependency on its own
                .EnsureNoOtherReferences()
                .Ensure();
        }

        [Test]
        public void TypeLoader_Recognizes_AttributeDependencies_At_Method_Level()
        {
            var type = FetchType<ClassWithMethodAttributeDependency>();
            DependencyAssertion
                .ForType(type)

                .Expected<DependencyAttribute>()
                .Expected<DependencyAttribute>()
                .ExpectedContext(nameof(IDependencyContextVisitor.MethodAttributeContext),
                            nameof(ClassWithMethodAttributeDependency.MethodWithAttributeDependency), "")
                .ExpectedContext(nameof(IDependencyContextVisitor.MethodAttributeContext),
                    nameof(ClassWithMethodAttributeDependency.MethodWithAttributeDependency), "")
                .EnsureNoOtherReferences()
                .Ensure();
        }

        [Test]
        public void TypeLoader_Recognizes_AttributeDependencies_At_Constructor()
        {
            var type = FetchType<ClassWithAttributeDependencyAtConstructor>();
            DependencyAssertion
                .ForType(type)

                .Expected<DependencyAttribute>()
                .Expected<DependencyAttribute>()
                .ExpectedContext(nameof(IDependencyContextVisitor.MethodAttributeContext), ".ctor", "")
                .ExpectedContext(nameof(IDependencyContextVisitor.MethodAttributeContext), ".ctor", "")
                .EnsureNoOtherReferences()
                .Ensure();
        }
        
        [Test]
        public void TypeLoader_Recognizes_AttributeDependencies_At_Property_Getter_And_Setter()
        {
            var type = FetchType<ClassWithPropertyAttributeDependency>();
            DependencyAssertion
                .ForType(type)

                .Expected<DependencyAttribute>()
                .Expected<DependencyAttribute>()
                .ExpectedContext(nameof(IDependencyContextVisitor.MethodAttributeContext), "get_MethodWithAttributeDependency", "")
                .ExpectedContext(nameof(IDependencyContextVisitor.MethodAttributeContext), "set_MethodWithAttributeDependency", "")
                .EnsureNoOtherReferences()
                .Ensure();
        }

        [Test]
        public void TypeLoader_Recognizes_Simple_AttributeDependencies_At_Property()
        {
            var type = FetchType<ClassWithSimplePropertyAttributeDependency>();
            DependencyAssertion
                .ForType(type)

                .Expected<DependencyAttribute>()
                .EnsureNoOtherReferences()
                .Ensure();
        }

        [Test]
        public void TypeLoader_Recognizes_Simple_Properties()
        {
            var type = FetchType<ClassWithSimplePropertyDependency>();
            DependencyAssertion
                .ForType(type)
                .Expected<bool>()
                .EnsureNoOtherReferences()
                .Ensure();
        }

        [Test]
        public void TypeLoader_Recognizes_Attributes_At_Fields_As_Dependency()
        {
            var type = FetchType<ClassWithFieldAttributeDependency>();
            DependencyAssertion
                .ForType(type)
                .Expected<DependencyAttribute>()
                .EnsureNoOtherReferences()
                .Ensure();
        }

        [Test]
        public void TypeLoader_Recognizes_Attributes_At_Interfaces_As_Dependency()
        {
            var type = FetchType<IInterfaceWithAttributeDependency>();
            DependencyAssertion
                .ForType(type)
                .Expected<DependencyAttribute>()
                .EnsureNoOtherReferences()
                .Ensure();
        }

        [Test]
        public void TypeLoader_Recognizes_Attributes_At_MethodReturn_As_Dependency()
        {
            var type = FetchType<ClassWithMethodAttributeDependency2>();
            DependencyAssertion
                .ForType(type)
                .Expected<DependencyAttribute>()
                .EnsureNoOtherReferences()
                .Ensure();
        }

        [Test]
        public void TypeLoader_Recognizes_Attributes_At_Events_As_Dependency()
        {
            var type = FetchType<ClassWithAttributeDependencyAtEvent>();
            DependencyAssertion
                .ForType(type)
                .Expected<DependencyAttribute>(Times.Once)
                .EnsureNoOtherReferences()
                .Ensure();
        }

        [Test]
        public void TypeLoader_Recognizes_DelegateTypes_As_Dependency()
        {
            var type = FetchType<ClassWithDelegateDependencyAtEvent>();
            DependencyAssertion
                .ForType(type)
                .Expected<MyEventHandler>(Times.Once)
                .EnsureNoOtherReferences()
                .Ensure();
        }


        [Test]
        public void TypeLoader_Recognizes_Attributes_At_MethodParameters_As_Dependency()
        {
            var type = FetchType<ClassWithMethodAttributeDependency3>();
            DependencyAssertion
                .ForType(type)
                .Expected<DependencyAttribute>(Times.Twice)
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
        public void TypeLoader_Can_Analyze_Assembly()
        {
            var sut = new TypeLoader(new[] { @"./YADA.Example.dll" });
            var types = sut.GetTypes();

            //foreach(var type in types) 
            //{
            //    PrintType(type);
            //    TestContext.WriteLine("--------------------------------------------------------------");
            //}
        }

        [Test]
        public void TypeLoader_GetTypes_Does_Not_Contain_Module()
        {
            var sut = new TypeLoader(new[] { @"./YADA.Example.dll" });
            var types = sut.GetTypes();
            var t = types.Select(r => r.FullName).ToList();
            Assert.That(types.Select(r => r.FullName), Does.Not.Contain("<Module>"));

        }

        [Test]
        public void TypeLoader_GetTypes_Delivers_NestedTypes_As_Well()
        {
            var sut = new TypeLoader(new[] { @"./YADA.Example.dll" });
            var types = sut.GetTypes();
            var t = types.Select(r => r.FullName).ToList();
            Assert.That(types.Select(r => r.FullName), Does.Contain(typeof(ClassWithNestedClass.NestedClass).FullName.Replace('+', '/')));

        }
        [Test]
        public void TypeLoader_GetTypes_Does_Deliver_Also_The_Assembly_Of_The_Types()
        {
            var sut = new TypeLoader(new[] { @"./YADA.Example.dll" });
            var types = sut.GetTypes();

            var assemblyNames = new List<string>(types.Select(t => t.AssemblyName));

            // also the dependencies must have a type
            foreach (var type in types)
            {
                foreach (var dependency in type.Dependencies)
                {
                    assemblyNames.Add(dependency.Type.AssemblyName);
                }
            }

            var assemblyName = assemblyNames.Distinct().ToList();

            Assert.That(assemblyName.Count, Is.EqualTo(1));
            var expectedName = typeof(Example1).Assembly.FullName;

            Assert.That(assemblyName.First, Is.EqualTo(expectedName));
        }

        [IgnoreType("Example.Example1")]
        [Test]
        public void TypeLoader_Types_Can_Be_Ignored()
        {
            var sut = new TypeLoader(new[] { @"./YADA.Example.dll" });
            var types = sut.GetTypes();

            Assert.That(types.Any(t => t.FullName == "Example.Example1"), Is.False);
        }

        [IgnoreType("**Example*")]
        [Test]
        public void TypeLoader_Types_Can_Be_Ignored2()
        {
            var sut = new TypeLoader(new[] { @"./YADA.Example.dll" });
            var types = sut.GetTypes();
            var l = types.Select(t => t.FullName).ToList();
            Assert.That(types.Any(t => t.FullName.Contains("Example")), Is.False);
        }



        [IgnoreType("Example.Dependency1", IgnoreTypeOptions.IgnoreAlsoAsDependency)]
        [Test]
        public void TypeLoader_Ignored_Types_Does_Not_Appear_As_Dependencies_Of_Other_Types()
        {
            var sut = new TypeLoader(new[] { @"./YADA.Example.dll" });
            var types = sut.GetTypes();
            var result = types.First(t => t.FullName == "Example.Example1");

            Assert.That(result.Dependencies.Any(d => d.Type.FullName == "Example.Dependency1"), Is.False);
        }

        private void PrintType(ITypeDescription type)
        {
            TestContext.WriteLine(type.FullName);
            foreach (var dependency in type.Dependencies)
            {
                TestContext.WriteLine($"    ->{dependency.Type.FullName} ({dependency.Occurrence}) :");
                foreach (var context in dependency.Contexts)
                {
                    TestContext.WriteLine($"        {context}");
                }
            }
        }


        private static ITypeDescription FetchType<T>()
        {
            return TypeLoader.GetType(typeof(T).FullName, typeof(T).Assembly.Location);
        }
    }
}