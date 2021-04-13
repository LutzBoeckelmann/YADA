// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using YADA.Core;

namespace YADA.Test
{
    [TestFixture]
    public class RuleEngineTests
    {
        public class Filter : IDependencyRule
        {
            private readonly Func<ITypeDescription, IDependency, bool> m_Function;

            public Filter(Func<ITypeDescription, IDependency, bool> function)
            {
                m_Function = function;
            }

            public bool Apply(ITypeDescription type, IDependency dependency)
            {
                return m_Function(type, dependency);
            }
        }

        public class Filter2 : ITypeRule
        {
            private readonly Func<ITypeDescription, bool> m_Function;

            public Filter2(Func<ITypeDescription, bool> function)
            {
                m_Function = function;
            }

            public bool Apply(ITypeDescription type)
            {
                return m_Function(type);
            }
        }

        public class DependencyMock : IDependency
        {
            public DependencyMock(ITypeDescription type, int occurrence) 
            {
                Type = type;
                Occurrence = occurrence;
            }
            public ITypeDescription Type { get; }

            public int Occurrence { get; }
        }

        public class TypeMock : ITypeDescription
        {
            private readonly List<IDependency> m_Dependencies;

            public TypeMock(string fullName) 
            {
                FullName = fullName;
                m_Dependencies = new List<IDependency>();
            }

            public string FullName { get; }

            public IEnumerable<IDependency> Dependencies => m_Dependencies;

            public void Add(TypeMock dependency, int occurrence = 1) 
            {
                m_Dependencies.Add(new DependencyMock(dependency, occurrence));
            }
        }
        
        [Test]
        public void Constructor_CorrectInput_Success() 
        {
            var sut = new DependencyRuleEngine(new []{new Filter2(_=>true)}, new []{new Filter((s,d)=>true)});

            Assert.NotNull(sut);
        }
        
        [Test]
        public void Analyse_AnyFilterDeliversTrue_Success() 
        {
            var input = new List<ITypeDescription>() { new TypeMock("Type1"), new TypeMock("Type2") };

            var sut = new DependencyRuleEngine(new []{new Filter2(_=>true)}, new []{new Filter((s,d)=>true)});

            var result = sut.Analyse(input);
            
            Assert.That(result, Is.True);
        }

        [Test]
        public void Analyse_SetOfTypes_TypeFilterCalledForAnyType() 
        
        {
            var input = new List<ITypeDescription>() { new TypeMock("Type1"), new TypeMock("Type2") };

            var list = new List<ITypeDescription>();

            var sut = new DependencyRuleEngine(new []{new Filter2(t => { list.Add(t); return true; })}, new []{new Filter((s,d)=>true)});

            var result = sut.Analyse(input);

            Assert.That(list, Is.EquivalentTo(input));
        }

        [Test]
        public void Analyse_SetOfTypesWithoutDependency_DependencyFiltersNotCalled() 
        
        {
            var input = new List<ITypeDescription>() { new TypeMock("Type1"), new TypeMock("Type2") };

            bool dependencyFilterCall = false;

            var sut = new DependencyRuleEngine(new []{new Filter2(_ =>true)}, new []{new Filter((s,d)=>dependencyFilterCall = true)});

            var result = sut.Analyse(input);

            Assert.That(dependencyFilterCall, Is.False);
        }

        [Test]
        public void Analyse_SetOfTypesOneWithExactlyDependency_DependencyFiltersCalled() 
        
        {
            var input = new List<TypeMock>() { new TypeMock("Type1"), new TypeMock("Type2") };
            input[0].Add(input[1]);
            


            var calls = new List<Tuple<ITypeDescription, IDependency>>();
            var sut = new DependencyRuleEngine(new []{new Filter2(_ =>true)}, new []{new Filter((s, d) => { calls.Add(new Tuple<ITypeDescription, IDependency>(s, d)); return true; })});

            var result = sut.Analyse(input);

            Assert.That(calls, Is.EquivalentTo(new []{new Tuple<ITypeDescription, IDependency>(input[0], input[0].Dependencies.First())}));
        }

        

        public class MockRepository
        {
            private readonly Dictionary<string, TypeMock> m_Mocks = new Dictionary<string, TypeMock>();

            public ITypeDescription GetOrCreate(string fullName) 
            {
                if(!m_Mocks.ContainsKey(fullName)) 
                {
                    m_Mocks.Add(fullName, new TypeMock(fullName));
                }

                return m_Mocks[fullName];
            }
        }
        
    }
}