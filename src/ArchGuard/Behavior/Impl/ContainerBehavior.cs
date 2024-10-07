// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System;
using System.Collections.Generic;
using YADA.ArchGuard.BuildingBlock;
using YADA.ArchGuard.Feedback;

namespace YADA.ArchGuard.Behavior.Impl
{
    public class ContainerBehavior : IContainerBehavior
    {
        List<IInternalContainerBehavior> m_List = new List<IInternalContainerBehavior>();

        // questionable if we really need more than one
        public void AddBehavior(string behavior)
        {
            IInternalContainerBehavior internalBehavior = null;
            switch (behavior)
            {
                /// <summary>
                /// Any child may access any other type in an open container
                /// </summary>
                case "Open":
                    internalBehavior = new ContainerBehaviorOpen();
                    break;

                /// <summary>
                /// Children may only access children which where added later
                /// </summary>
                case "Layer":
                    internalBehavior = new ContainerBehaviorLayer();
                    break;
                /// <summary>
                /// Child may not access any other child
                /// </summary>
                case "Restricted":
                    internalBehavior = new ContainerBehaviorIsolated();
                    break;
            }

            if(internalBehavior == null)
            {
                throw new ArgumentException("unknown behavior", nameof(behavior));
            }

            m_List.Add(internalBehavior);
        }

        public bool Check(IReadOnlyList<IBuildingBlock> chain, int currentIndex, IBuildingBlock buildingBlock, ICheckFeedback feedback)
        {
            var result = true; // is it needed that at least one rule says true?

            foreach (var behavior in m_List)
            {
                // check binding
                result &= behavior.Check(chain, currentIndex, buildingBlock, feedback);
            }

            return result;
        }
    }
}

