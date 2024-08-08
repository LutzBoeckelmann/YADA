// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using YADA.Analyzer;

namespace YADA.AnalyzerTests;

public struct Times
{
    public Times(int time)
    {
        Time = time;
        IsNotCounting = false;
    }

    private Times(int time, bool isNotCounting)
    {
        Time = time;
        IsNotCounting = isNotCounting;
    }

    public static Times operator +(Times a, Times b)
    {
        return new Times(a.Time + b.Time);
    }

    public static Times NoCounting => new Times(-1, true);
    public static Times Zero => new Times(0);
    public static Times Once => new Times(1);
    public static Times Twice => new Times(2);
    public static Times ThreeTimes => new Times(3);
    public int Time { get; set; }
    public bool IsNotCounting { get; }
}


class DependencyAssertion
{
    private class TestVisitor : IDependencyContextVisitor<TestVisitor>
    {
        private readonly List<Tuple<string, string, string>> m_Expected =
            new List<Tuple<string, string, string>>();

        public void Add(string name, string first, string second)
        {
            m_Expected.Add(new Tuple<string, string, string>(name, first, second));
        }

        public TestVisitor FieldDefinition(string fieldName)
        {
            AssertTuple(nameof(FieldDefinition), fieldName);

            return this;
        }

        public TestVisitor BaseClassDefinition()
        {
            AssertTuple(nameof(BaseClassDefinition));

            return this;
        }

        public TestVisitor MethodDefinitionParameter(string methodName)
        {
            AssertTuple(nameof(MethodDefinitionParameter), methodName);

            return this;
        }

        public TestVisitor MethodDefinitionLocalVariable(string methodName)
        {
            AssertTuple(nameof(MethodDefinitionLocalVariable), methodName);

            return this;
        }

        public TestVisitor MethodDefinitionReturnType(string methodName)
        {
            AssertTuple(nameof(MethodDefinitionReturnType), methodName);

            return this;
        }

        public TestVisitor MethodBodyCallMethodAtType(string methodName, string calledMethodFullName)
        {
            AssertTuple(nameof(MethodBodyCallMethodAtType), methodName, calledMethodFullName);

            return this;
        }

        public TestVisitor MethodBodyCalledMethodReturnType(string methodName, string calledMethodFullName)
        {
            AssertTuple(nameof(MethodBodyCalledMethodReturnType), methodName, calledMethodFullName);

            return this;
        }

        public TestVisitor MethodBodyCalledMethodParameter(string methodName, string calledMethodFullName)
        {
            AssertTuple(nameof(MethodBodyCalledMethodParameter), methodName, calledMethodFullName);

            return this;
        }

        public TestVisitor MethodBodyReferencedType(string methodName)
        {
            AssertTuple(nameof(MethodBodyReferencedType), methodName);

            return this;
        }

        public TestVisitor MethodBodyAccessedFieldType(string methodName, string fieldName)
        {
            AssertTuple(nameof(MethodBodyAccessedFieldType), methodName, fieldName);

            return this;
        }

        public TestVisitor ClassAttributeContext()
        {
            AssertTuple(nameof(ClassAttributeContext));

            return this;
        }

        public TestVisitor MethodAttributeContext(string methodName)
        {
            AssertTuple(nameof(MethodAttributeContext), methodName);

            return this;
        }

        public TestVisitor FieldAttribute(string fieldAttributeName)
        {
            AssertTuple(nameof(FieldAttribute), fieldAttributeName);

            return this;
        }

        private void AssertTuple(string first, string second = "", string third = "")
        {
            var found = m_Expected.FirstOrDefault(t =>
                t.Item1 == first && t.Item2 == second && t.Item3 == third);

            if (found != null)
            {
                m_Expected.Remove(found);
            }
            else
            {
                Assert.Fail($"Expected ({first}, {second}, {third}) not found");
            }
        }

        public void AssertEmpty()
        {
            Assert.IsEmpty(m_Expected, "There are expected contexts left");
        }
    }

    private readonly ITypeDescription m_Type;
    private bool m_IgnoreSystemDotObject;
    private bool m_CountOccurrences = true;
    private readonly List<Action> m_Assertions = new List<Action>();
    private readonly TestVisitor m_TestVisitor = new TestVisitor();
    private bool m_AssertContext = false;
    private readonly Dictionary<Type, Times> m_ExpectedTypes = new Dictionary<Type, Times>();

  
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
        return Expected(type, Times.Once);
    }

    public DependencyAssertion Expected<T>(Times times)
    {
        return Expected(typeof(T), times);
    }

    public DependencyAssertion Expected(Type type, Times times)
    {

        if (!m_ExpectedTypes.ContainsKey(type))
        {
            m_ExpectedTypes.Add(type, times);
            m_Assertions.Add(() => InternalAssertDependsOn(type));
        }
        else
        {
            m_ExpectedTypes[type] += times;
        }
        
        return this;
    }

    public DependencyAssertion EnsureNoOtherReferences(bool countOccurences = false, bool ignoreSystemDotObject = true)
    {
        m_CountOccurrences = countOccurences;
        m_IgnoreSystemDotObject = ignoreSystemDotObject;
        m_Assertions.Add(InternalEnsureNoOtherReferences);

        return this;
    }

    public DependencyAssertion ExpectedContext(string context, string first, string second)
    {
        m_AssertContext = true;
        m_TestVisitor.Add(context, first, second);

        return this;
    }



    public void Ensure()
    {
        foreach (var action in m_Assertions)
        {
            action();
        }

        if (m_AssertContext)
        {
            foreach (var typeDependency in m_Type.Dependencies)
            {
                if (!IgnoreDotnetType(typeDependency.Type.FullName))
                {
                    foreach (var context in typeDependency.Contexts)
                    {
                        context.Visit(m_TestVisitor);
                    }
                }
            }
        }

        m_TestVisitor.AssertEmpty();
    }

    private bool IgnoreDotnetType(string fullName)
    {
        if (m_ExpectedTypes.Keys.Any(k => k.FullName == fullName))
        {
            return false;
        }
        return fullName.StartsWith("System") && m_IgnoreSystemDotObject;
    }


    private void InternalEnsureNoOtherReferences()
    {
        var count = 0;
        foreach (var typeDependency in m_Type.Dependencies)
        {
            if (!IgnoreDotnetType(typeDependency.Type.FullName))
            {
                count += m_CountOccurrences ? typeDependency.Occurrence : 1;
            }
        }

        var expected = m_CountOccurrences ? m_ExpectedTypes.Select(i => i.Value.Time).Sum() : m_ExpectedTypes.Keys.Count;
        Assert.That(count, Is.EqualTo(expected),
            $"At type {m_Type.FullName} are {expected} expected but {count} are available");
    }

    private void InternalAssertDependsOn(Type dependency)
    {
        Assert.That(DependsOn(dependency), Is.True,
            $"The type {m_Type.FullName} should depend on {dependency.FullName}");
    }

    private bool DependsOn(Type dependency)
    {
        var dependencyHolder = m_Type.Dependencies.FirstOrDefault(t => t.Type.FullName == dependency.FullName);

        var result = dependencyHolder != null;
        m_ExpectedTypes.TryGetValue(dependency, out var times);
        
        if (result && !times.IsNotCounting)
        {
            result = dependencyHolder.Occurrence == times.Time;
        }

        return result;
    }

}