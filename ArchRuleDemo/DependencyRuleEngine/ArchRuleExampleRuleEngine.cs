// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Collections.Generic;
using ArchRuleDemo.ArchitecturalModel;
using YADA.Core.DependencyRuleEngine.Rules;
using YADA.Core.DependencyRuleEngine;

namespace ArchRuleDemo.DependencyRuleEngine
{
    public class ArchRuleExampleRuleEngine : BaseDependencyRuleEngine<ArchRuleExampleType, ArchRuleExampleDependency>
    {
        public ArchRuleExampleRuleEngine(IEnumerable<ITypeRule<ArchRuleExampleType>> typeRules, IEnumerable<IDependencyRule<ArchRuleExampleType, ArchRuleExampleDependency>> dependencyRules, ArchRuleExampleRuleEngineMapper mapper) : base(typeRules, dependencyRules, mapper)
        {
        }
    }
}