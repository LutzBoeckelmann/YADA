// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

namespace YADA.Test
{
    public interface IArchRuleExampleTypeRepository 
    {
        ArchRuleExampleType GetTypeRepresentation(string fullqualifiedType);
    }
}