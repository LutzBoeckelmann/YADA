// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Collections.Generic;
using YADA.ArchGuard.BuildingBlock;
using YADA.ArchGuard.Feedback;

namespace YADA.ArchGuard.Behavior.Impl
{
    // is this a behavior or more a trait?
    class ExtensionChildBehavior : IInternalChildBehaviorType
    {
        public bool Check(IReadOnlyList<IBuildingBlock> chain, int currentIndex, ICheckFeedback feedback)
        {
            feedback.AddFeedback(FeedbackType.Warning, "ExtensionChildBehavior not implemented");


            return true;
        }
    }
}

