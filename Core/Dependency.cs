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
        public Dependency(TypeDescription type) 
        {
            Type = type;
            Occurrence = 0;
        }

        public ITypeDescription Type { get; }
        public int Occurrence { get; private set; }

        public void IncrementUsage() { Occurrence++; }

        public IEnumerable<IDependencyContext> Contexts { get; }
    }
}
