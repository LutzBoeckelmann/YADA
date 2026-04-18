// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Collections.Generic;
using YADA.ArchGuard.BuildingBlock;
using YADA.ArchGuard.Feedback;

namespace YADA.ArchGuard.Behavior.Impl
{
    class ContainerBehaviorOpen : IInternalContainerBehavior
    {
        public string AsString => "Open";

        public bool Check(IReadOnlyList<IBuildingBlock> chain, int currentIndex, IBuildingBlock buildingBlock, ICheckFeedback feedback)
        {
            return true;
        }
    }
}

