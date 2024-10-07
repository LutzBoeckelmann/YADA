// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System;
using System.Collections.Generic;
using YADA.ArchGuard.BuildingBlock;
using YADA.ArchGuard.Feedback;

namespace YADA.ArchGuard.Behavior.Impl
{
    class PrivateChildBehavior : IInternalChildBehaviorType
    {
        public bool Check(IReadOnlyList<IBuildingBlock> chain, int currentIndex, ICheckFeedback feedback)
        {
            if (currentIndex <= 1)
            {
                return true; // in this case it could not be outgoing 
            }

            var parent = chain[currentIndex - 1];

            if (parent != chain[currentIndex].Parent)
            {
                throw new NotSupportedException("parent in chain should be same as real parent or not? check in test");
            }

            var next = chain[currentIndex - 2];
            var result = next.Parent == parent;
            if(!result)
            {
                feedback.AddFeedback($"Only direct siblings in {parent.Description.Name} may depend on private building block {chain[currentIndex].Description.Name}. But {next.Description.Name} is not a sibling");
            }
            return result;
        }
    }
}

