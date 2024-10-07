// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System;
using System.Collections.Generic;

using YADA.ArchGuard.BuildingBlock;
using YADA.ArchGuard.Feedback;

namespace YADA.ArchGuard.Behavior.Impl
{
    class ProtectedChildBehavior : IInternalChildBehaviorType
    {
        public bool Check(IReadOnlyList<IBuildingBlock> chain, int currentIndex, ICheckFeedback feedback)
        {
            feedback.AddFeedback(FeedbackType.Warning, "ProtectedBehavior not implemented");
            
            if (currentIndex > chain.Count - 2)
            {
                return true; // in this case it could not be outgoing 
            }

            var parent = chain[currentIndex + 1];

            if (parent != chain[currentIndex].Parent)
            {
                throw new NotSupportedException("parent in chain should be same as real parent or not? check in test");
            }

            var next = chain[currentIndex + 2];

            // add checks for extension in the chain
            /*
                List<string> children = new List<string>();
                // here we have to check if its an extension
                for (int i = currentIndex; i < chain.Count; i++)
                {
                    var current = chain[i] as BuildingBlock;
                    // collect all names from direct parents (in the chain to root) any of this building blocks may be extended by the source
                    // from the right start with the source and check if some of the by passing building blocks is an extension but only to the root

                }
             
             */

            return next.Parent == parent;


        }
    }
}

