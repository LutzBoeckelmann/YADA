// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Collections.Generic;
using YADA.ArchGuard.BuildingBlock;
using YADA.ArchGuard.Feedback;

namespace YADA.ArchGuard.Behavior.Impl
{
    interface IInternalContainerBehavior
    {
        bool Check(IReadOnlyList<IBuildingBlock> chain, int currentIndex, IBuildingBlock buildingBlock, ICheckFeedback feedback);
    }
}

