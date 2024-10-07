// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Collections.Generic;
using System.Linq;

using YADA.Analyzer;
using YADA.ArchGuard.Feedback;

namespace YADA.ArchGuard.BuildingBlock.Impl
{

    /// <summary>
    /// A real building block including all of its types in the concrete project.
    /// </summary>
    public class ConcreteBuildingBlock : IConcreteBuildingBlock
    {
        private class Dependency
        {
            public ITypeDescription Type { get; }

            public Dependency(ITypeDescription type, IDependency dependency, IReadOnlyCollection<IBuildingBlock> chain)
            {
                Type = type;
                DependencyType = dependency;
                DependencyChain = chain;
            }

            public IDependency DependencyType { get; }
            public IReadOnlyCollection<IBuildingBlock> DependencyChain { get;}
        }

        
        private IReadOnlyCollection<IBuildingBlock> m_Chain;
        private TypeMatcher m_TypeMatcher;
        private readonly Dictionary<string, ITypeDescription> m_Types = new Dictionary<string, ITypeDescription>();
        private IList<Dependency> m_Dependencies = new List<Dependency>();

        public ConcreteBuildingBlock(IReadOnlyCollection<IBuildingBlock> chain, TypeMatcher typeMatcher)
        {
            m_Chain = chain;
            m_TypeMatcher = typeMatcher;
        }
        
        public bool IsEqual(IBuildingBlock block)
        {
            return block.Description.Name == m_Chain.First().Description.Name;
        }

        public IReadOnlyCollection<ITypeDescription> Types => m_Types.Values;

        public void AddType(ITypeDescription type)
        {
            m_Types.Add(type.ToString(), type);
        }

        public void AddDependingBlocks()
        {
            foreach (var type in m_Types.Values) 
            {
                foreach (var dependency in type.Dependencies)
                {
                    var dependingChain = m_TypeMatcher.GetChain(dependency.Type);

                    m_Dependencies.Add(new Dependency(type, dependency, dependingChain));
                }
            }
        }

        public bool CheckDependencies(List<IConcreteBuildingBlock> buildingBlocks, ICheckFeedback feedback)
        {
            var result = true;
            foreach (var dependency in m_Dependencies)
            {
                var connectingChain = m_TypeMatcher.GetConnectingChain(dependency.Type, dependency.DependencyType.Type);
                for (int i = 0; i < connectingChain.Count; i++)
                {
                    result &= connectingChain[i].Check(connectingChain, feedback);
                }
            }
            return result;
        }
    
    }
}

