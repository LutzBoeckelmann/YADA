// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Collections.Generic;
using YADA.Analyzer;
using YADA.ArchGuard.Feedback;

namespace YADA.ArchGuard.BuildingBlock
{
    public interface IConcreteBuildingBlock
    {
        bool IsEqual(IBuildingBlock buildingBlock);
        void AddType(ITypeDescription type);
        void AddDependingBlocks();
        bool CheckDependencies(List<IConcreteBuildingBlock> concreteBuildingBlocks, ICheckFeedback feedback);
    }
}

