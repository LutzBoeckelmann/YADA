// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Collections.Generic;
using YADA.ArchGuard.BuildingBlock;
using YADA.ArchGuard.Feedback;

namespace YADA.ArchGuard.Behavior
{
    /*
## public 
accessible from outside

## private
not accessible from outside, implementation assembly only within the own parent accessible

## protected
not accessible from outside, only within by sibling building blocks in the same parent, like private
but it may be as well accessed by extensions


## extension*/

    interface IInternalChildBehaviorType
    {
        bool Check(IReadOnlyList<IBuildingBlock> chain, int currentIndex, ICheckFeedback feedback);
    }
}

