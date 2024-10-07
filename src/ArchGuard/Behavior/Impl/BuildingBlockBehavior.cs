// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

namespace YADA.ArchGuard.Behavior.Impl
{
    interface IBehaviorCreator
    {
        IInternalContainerBehavior CreateBehavior(string tag);
    }

    class ContainerBehaviorOpenCreator : IBehaviorCreator
    {
        public IInternalContainerBehavior CreateBehavior(string tag)
        {
            if (tag == "Open")
            {
                return new ContainerBehaviorOpen();
            }
            return null;
        }
    }

    public class BuildingBlockBehavior : IBuildingBlockBehavior
    {
        public BuildingBlockBehavior() :this(new string[] { "Open" }, new string[] { "Public" })
        {

        }

        public BuildingBlockBehavior(string[] containerBehaviors) :this(containerBehaviors, new string[] { "Public" })
        {

        }

        public BuildingBlockBehavior(string[] containerBehaviors, string[] childBehavoirType)
        {
            ContainerBehavior = new ContainerBehavior();
            foreach (var containerBehavior in containerBehaviors)
            {
                ContainerBehavior.AddBehavior(containerBehavior);
            }

            InternalBehavior = new InternalBehavior();
            foreach (var childBehavior in childBehavoirType)
            {
                InternalBehavior.AddBehavior(childBehavior);
            }
        }

        public IInternalBehavior InternalBehavior { get; }

        public IContainerBehavior ContainerBehavior { get; }
    }
}

