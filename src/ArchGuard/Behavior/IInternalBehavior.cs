// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Collections.Generic;
using YADA.ArchGuard.Feedback;
using YADA.ArchGuard.BuildingBlock;

namespace YADA.ArchGuard.Behavior
{
    /// <summary>
    /// Outside accessible
    /// </summary>


    /// <summary>
    /// Only accessible within the same container
    /// </summary>


    /// <summary>
    /// Like Private but also accessible from extending building blocks
    /// </summary>



    /// <summary>
    /// My not be accessed at all only an extension, special rights within the extended building block see 
    /// Protected.
    /// 
    /// For the computation of cycles an extension belongs to the original building block.
    /// </summary>

    public interface IInternalBehavior
    {
        void AddBehavior(string childBehavior);
        bool Check(IReadOnlyList<IBuildingBlock> chain, int currentIndex, ICheckFeedback feedback);
    }
}

