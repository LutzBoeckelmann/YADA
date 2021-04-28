// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Collections.Generic;

namespace YADA.Core.Analyser.Impl
{
    public class Dependency : IDependency
    {
        private readonly List<IDependencyContext> m_Contexts = new List<IDependencyContext>();
        public Dependency(TypeDescription type) 
        {
            Type = type;
        }

        public ITypeDescription Type { get; }
        public int Occurrence => m_Contexts.Count;

        public void AddUsageContext(IDependencyContext context) {m_Contexts.Add(context); }

        public IEnumerable<IDependencyContext> Contexts => m_Contexts;
    }
}
