// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

namespace YADA.ArchGuard.BuildingBlock
{
    public interface IBuildingBlockDescription
    {
        string Name { get; }
        IBuildingBlockTypeFilter TypeFilter { get; }
        bool Abstract { get; }
        // some identifier for the type
        // or a list of rule to apply
    }
}

