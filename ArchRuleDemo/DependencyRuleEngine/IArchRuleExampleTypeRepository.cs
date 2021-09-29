// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

namespace ArchRuleDemo.ArchitecturalModel
{
    public interface IArchRuleExampleTypeRepository
    {
        ArchRuleExampleType GetTypeRepresentation(string fullqualifiedType);
    }
}