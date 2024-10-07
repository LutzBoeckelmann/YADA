// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Collections.Generic;
using YADA.ArchGuard.BuildingBlock;
using YADA.ArchGuard.Feedback;

namespace YADA.ArchGuard.Behavior.Impl
{
    class ContainerBehaviorLayer : IInternalContainerBehavior
    {

        public bool Check(IReadOnlyList<IBuildingBlock> chain, int currentIndex, IBuildingBlock buildingBlock, ICheckFeedback feedback)
        {
            if (currentIndex > 0 && currentIndex + 1 < chain.Count && chain[currentIndex - 1].Parent == buildingBlock && chain[currentIndex + 1].Parent == buildingBlock)
            {
                var index1 = buildingBlock.GetIndex(chain[currentIndex - 1]);
                var index2 = buildingBlock.GetIndex(chain[currentIndex + 1]);

                var result = index1 < index2;
                if (!result)
                { 
                    feedback.AddFeedback($"{chain[currentIndex].Description.Name} is layered, a dependency must no run upward from so a dependency between {chain[currentIndex - 1].Description.Name} to {chain[currentIndex + 1].Description.Name} ");
                }

                return result;
            }

            return true;
        }
    }
}

