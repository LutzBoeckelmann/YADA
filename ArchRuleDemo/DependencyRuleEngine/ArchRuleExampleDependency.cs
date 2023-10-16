// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Collections.Generic;
using ArchRuleDemo.ArchitecturalModel;
using YADA.Analyzer;

namespace ArchRuleDemo.ArchRuleExampleDependencyRuleEngine
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
            Context = new List<IDependencyContext>();
        }

        public ArchRuleExampleDependency(ArchRuleExampleType dependencyType, List<IDependencyContext> context)
        {
            DependencyType = dependencyType;
            Context = context;
        }
        public List<IDependencyContext> Context;
    }
}