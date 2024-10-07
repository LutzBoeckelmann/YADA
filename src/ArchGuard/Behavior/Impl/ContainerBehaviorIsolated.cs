// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Collections.Generic;
using YADA.ArchGuard.BuildingBlock;
using YADA.ArchGuard.Feedback;

namespace YADA.ArchGuard.Behavior.Impl
{
    class ContainerBehaviorIsolated : IInternalContainerBehavior
    {

        public bool Check(IReadOnlyList<IBuildingBlock> chain, int currentIndex, IBuildingBlock buildingBlock, ICheckFeedback feedback)
        {
            if (currentIndex > 0 && currentIndex + 1 < chain.Count && chain[currentIndex - 1].Parent == buildingBlock && chain[currentIndex + 1].Parent == buildingBlock)
            {
                feedback.AddFeedback($"Children in {chain[currentIndex].Description.Name} are isolated so a dependency between {chain[currentIndex-1].Description.Name} and {chain[currentIndex + 1].Description.Name}");
                return false;
            }

            return true;
        }
    }
}

