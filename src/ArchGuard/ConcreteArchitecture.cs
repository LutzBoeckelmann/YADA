// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Collections.Generic;
using YADA.ArchGuard.BuildingBlock;
using YADA.ArchGuard.Feedback;

namespace YADA.ArchGuard
{
    /*
     
    workflow

    given an arch description as tree of building blocks
    for the analyze it will be cloned and the leaves will be replaced by concrete blocks
    
     */

    public class ConcreteArchitecture
    {
        private List<IConcreteBuildingBlock> m_ConcreteBuildingBlocks;
        private readonly bool m_Valid;
        private readonly IFeedbackCollector m_Feedback;

        public ConcreteArchitecture(List<IConcreteBuildingBlock> concreteBuildingBlocks, bool valid, IFeedbackCollector feedback)
        {
            m_ConcreteBuildingBlocks = concreteBuildingBlocks;
            m_Valid = valid;
            m_Feedback = feedback;
        }

        public int Count => m_ConcreteBuildingBlocks.Count;
        public bool Valid => m_Valid;

        public IFeedbackCollector Feedback => m_Feedback;
    }
}

