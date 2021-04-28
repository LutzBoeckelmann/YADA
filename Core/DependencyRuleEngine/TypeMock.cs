// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Collections.Generic;
using YADA.Core.Analyser;

namespace Core.DependencyRuleEngine
{
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
}