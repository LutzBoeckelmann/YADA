// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Collections.Generic;
using YADA.ArchGuard.BuildingBlock;
using YADA.ArchGuard.Feedback;

namespace YADA.ArchGuard.Behavior.Impl
{
    // is this a behavior or more a trait?
    // This is a trait and additional it needs the information (tag) of the
    // extended subsystem. In this case the extension will be treated like
    // every other building block in the extended building block
    class ExtensionChildBehavior : IInternalChildBehaviorType
    {
        public string AsString => "Extension";

        public bool Check(IReadOnlyList<IBuildingBlock> chain, int currentIndex, ICheckFeedback feedback)
        {
            feedback.AddFeedback(FeedbackType.Warning, "ExtensionChildBehavior not implemented");


            return true;
        }
    }
}

