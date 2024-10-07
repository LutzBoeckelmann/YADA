// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Collections.Generic;
using YADA.ArchGuard.BuildingBlock;
using YADA.ArchGuard.Feedback;

namespace YADA.ArchGuard.Behavior.Impl
{

    public class InternalBehavior : IInternalBehavior
    {
        List<IInternalChildBehaviorType> m_List = new List<IInternalChildBehaviorType>();

        public void AddBehavior(string behavior)
        {
            IInternalChildBehaviorType internalBehavior = null;
            
            // switch case temporary. Add expendable reader 
            switch (behavior)
            {
                case "Extension":
                    internalBehavior = new ExtensionChildBehavior();
                    break;
                case "Protected":
                    internalBehavior = new ProtectedChildBehavior();
                    break;
                case "Private":
                    internalBehavior = new PrivateChildBehavior();
                    break;
                case "Public":
                    internalBehavior = new PublicChildBehavior();
                    break;
            }

            m_List.Add(internalBehavior);
        }

        public bool Check(IReadOnlyList<IBuildingBlock> chain, int currentIndex, ICheckFeedback feedback)
        {
            var result = true; // is it needed that at least one rule says true?

            foreach(var behavior in m_List)
            {
                // check binding
                result &= behavior.Check(chain, currentIndex, feedback);
            }

            return result;
        }

    }
}

