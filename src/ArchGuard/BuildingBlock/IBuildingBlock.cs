// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Collections.Generic;
using YADA.Analyzer;
using YADA.ArchGuard.Feedback;

namespace YADA.ArchGuard.BuildingBlock
{
    public interface IBuildingBlock
    {
        IBuildingBlock Parent { get; }

        IReadOnlyList<IBuildingBlock> Children { get; }

        IBuildingBlockDescription Description { get; }

        bool Check(IReadOnlyList<IBuildingBlock> chain, ICheckFeedback feedback);

        int GetIndex(IBuildingBlock buildingBlock);

        IReadOnlyCollection<IBuildingBlock> GetChain(ITypeDescription type);
    }
}

