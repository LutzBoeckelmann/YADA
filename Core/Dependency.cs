// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Collections.Generic;

namespace YADA.Core
{

    public interface IDependency 
    {
        
        ITypeDescription Type { get; }
        int Occurrence { get; }

        IEnumerable<IDependencyContext> Contexts { get; }
    }

    public interface IDependencyContext 
    {

    }


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
