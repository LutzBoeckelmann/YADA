// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Collections.Generic;
using System.Linq;

using YADA.Analyzer;
using YADA.ArchGuard.Feedback.Impl;
using YADA.ArchGuard.BuildingBlock;
using YADA.ArchGuard.BuildingBlock.Impl;

namespace YADA.ArchGuard
{
    public class ConcreteArchitectureFactory
    {
        private readonly TypeMatcher m_TypeMatcher;

        public ConcreteArchitectureFactory(TypeMatcher typeMatcher)
        {
            m_TypeMatcher = typeMatcher;
        }

        public ConcreteArchitecture GetArchitecture(IEnumerable<ITypeDescription> types)
        {
            FeedbackCollector feedback = new FeedbackCollector();
            List<IConcreteBuildingBlock> concreteBuildingBlocks = new List<IConcreteBuildingBlock>();


            foreach (var type in types)
            {
                var chain = m_TypeMatcher.GetChain(type);

                IConcreteBuildingBlock concreteBuildingBlock = concreteBuildingBlocks.FirstOrDefault(i => i.IsEqual(chain.First()));
                if (concreteBuildingBlock == null)
                {
                    concreteBuildingBlock = new ConcreteBuildingBlock(chain, m_TypeMatcher);
                    concreteBuildingBlocks.Add(concreteBuildingBlock);
                }

                concreteBuildingBlock.AddType(type);
            }

            foreach (var concreteBuildingBlock in concreteBuildingBlocks)
            {
                concreteBuildingBlock.AddDependingBlocks();
            }

            var valid = true;
            var messages = new List<string>();
            foreach (var concreteBuildingBlock in concreteBuildingBlocks)
            {
                valid &= concreteBuildingBlock.CheckDependencies(concreteBuildingBlocks, feedback);
            }

            return new ConcreteArchitecture(concreteBuildingBlocks, valid, feedback);
        }
    }
}

