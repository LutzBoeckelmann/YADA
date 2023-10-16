// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Collections.Generic;

namespace YADA.Analyzer
{
    /// <summary>
    /// A simple fake implementation of ITypeDescription for test purposes.
    /// </summary>
    public class TypeDescriptionFake : ITypeDescription
    {
        private readonly List<IDependency> m_Dependencies;

        public TypeDescriptionFake(string fullName, string assemblyName)
        {
            FullName = fullName;
            m_Dependencies = new List<IDependency>();
        }
        public TypeDescriptionFake(string fullName)
        {
            FullName = fullName;
            m_Dependencies = new List<IDependency>();
        }
        

        public string FullName { get; }

        public IEnumerable<IDependency> Dependencies => m_Dependencies;

        public string AssemblyName { get; }

        public void Add(TypeDescriptionFake dependency, int occurrence = 1)
        {
            m_Dependencies.Add(new DependencyFake(dependency, occurrence));
        }
    }
}