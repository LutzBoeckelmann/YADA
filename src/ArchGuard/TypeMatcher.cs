// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Collections.Generic;

using YADA.Analyzer;
using YADA.ArchGuard.BuildingBlock;

namespace YADA.ArchGuard
{

    public class TypeMatcher
    {
        private readonly IBuildingBlock m_Project;
        private IDictionary<string, IReadOnlyCollection<IBuildingBlock>> m_Dict = new Dictionary<string, IReadOnlyCollection<IBuildingBlock>>();
        
        
        public TypeMatcher(IBuildingBlock project)
        {
            m_Project = project;
        }

        public IReadOnlyList<IBuildingBlock> GetConnectingChain(ITypeDescription type, ITypeDescription dependency)
        {
            var typeChain = new List<IBuildingBlock>(GetChain(type));
            var dependencyChain = new List<IBuildingBlock>(GetChain(dependency));

            return GetConnectingChain(typeChain, dependencyChain);
        }

        private IReadOnlyList<IBuildingBlock> GetConnectingChain(IReadOnlyList<IBuildingBlock> typeChain, IReadOnlyList<IBuildingBlock> dependencyChain)
        {
            List<IBuildingBlock> result = new List<IBuildingBlock>();
            
            // type is parent of dependency
            // dependency is parent of type
            // both are excluded by default concrete building blocks may not have children. 

            // type and dependency share a common ancestor

            if (typeChain[0] == dependencyChain[0])
            {
                result.Add(typeChain[0]);
                return result;
            }
            for (int i = 1; i < typeChain.Count; i++)
            {
                // search parent
                for (int j = 1; j < dependencyChain.Count; j++)
                {
                    if (typeChain[i] == dependencyChain[j])
                    {
                        for (int x = 0; x <= i; x++)
                        {
                            result.Add(typeChain[x]);
                        }

                        for (int y = j - 1; y >= 0; y--)
                        {
                            result.Add(dependencyChain[y]);
                        }

                        return result;
                    }
                }

            }


            return null;
        }

      

        public IReadOnlyCollection<IBuildingBlock> GetChain(ITypeDescription type)
        {
            var key = type.AssemblyName + type.FullName;

            if (!m_Dict.TryGetValue(key, out IReadOnlyCollection<IBuildingBlock> result))
            {
                result = m_Project.GetChain(type);
                m_Dict.Add(key, result);
            }

            return result;
        }
    }
}

