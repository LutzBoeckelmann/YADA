// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Collections.Generic;
using YADA.ArchGuard.BuildingBlock;
using YADA.ArchGuard.Feedback;

namespace YADA.ArchGuard.Behavior
{
    /*
     
Handles the behavior of the client inside its container.
      
## public 
accessible from outside

## private
not accessible from outside, implementation assembly only within the own parent accessible

## protected -> should we add a tag for different protected states?
not accessible from outside, only within by sibling building blocks in the same parent, like private
but it may be as well accessed by extensions


## extension -> not an internal behavior Its a tag. 
    or its an internal behavior which would mean private. 
    never the less the container needs to be identified and than a tag needs to be added
    Potentially the tag could be determined by a function based on the namespace.
    than the user has only to specify it once. -> Additional feature


     */

    interface IInternalChildBehaviorType
    {
        bool Check(IReadOnlyList<IBuildingBlock> chain, int currentIndex, ICheckFeedback feedback);
        string AsString { get; }
    }
}

