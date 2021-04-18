// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

namespace YADA.Test
{

    public class ArchRuleExampleDependency
    {
        public ArchRuleExampleType DependencyType { get; }
        public bool Valid => DependencyType.Valid;
        public ArchRuleDomainLayer DomainLayer => DependencyType.DomainLayer;
        public ArchRuleTechnicalLayer TechnicalLayer => DependencyType.TechnicalLayer;
        public ArchRuleModule Module => DependencyType.Module;

        public ArchRuleExampleDependency(ArchRuleExampleType dependencyType)
        {
            DependencyType = dependencyType;
        }
    }
}