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

    public class BuildingBlockBehaviorData
    {
        public string ContainerBehavior { get; set; }
        public string InternalBehavior { get; set; }
    }

    public class BuildingBlockBehavior : IBuildingBlockBehavior
    {
        public static BuildingBlockBehavior Create(BuildingBlockBehaviorData data)
        {
            var containerBehavior  = data.ContainerBehavior;
            if(string.IsNullOrEmpty(containerBehavior))
            {
                containerBehavior = "Open";
            }
           
            var internalBehavior = data.InternalBehavior;
            if (string.IsNullOrEmpty(internalBehavior))
            {
                internalBehavior = "Public";
            }
            return new BuildingBlockBehavior(data.ContainerBehavior.Split(','), data.InternalBehavior.Split(','));
        }

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

