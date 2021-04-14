// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Collections.Generic;

namespace YADA.Core
{
    public class TypeDescription : ITypeDescription
    {
        private readonly Dictionary<string, Dependency> m_Dependencies = new Dictionary<string, Dependency>();
        public TypeDescription(string fullName) 
        {
            FullName = fullName;
        }


        public string FullName { get; }

        public IEnumerable<IDependency> Dependencies => m_Dependencies.Values;

        public void AddDependency(TypeDescription description, IDependencyContext context) 
        {
            if(!m_Dependencies.ContainsKey(description.FullName)) 
            {
                m_Dependencies.Add(description.FullName, new Dependency(description));
            }

            m_Dependencies[description.FullName].AddUsageContext(context);
        }
    }
}
