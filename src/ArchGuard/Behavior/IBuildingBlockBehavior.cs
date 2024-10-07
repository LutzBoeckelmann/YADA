// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

namespace YADA.ArchGuard.Behavior
{
    public interface IBuildingBlockBehavior
    {
        IInternalBehavior InternalBehavior { get; }

        IContainerBehavior ContainerBehavior { get; }
    }
}

