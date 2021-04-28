// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Collections.Generic;
using YADA.Core.DependencyRuleEngine;
using YADA.Core.DependencyRuleEngine.Impl;

namespace YADA.Test
{
    public class ArchRuleExampleRuleEngine : BaseDependencyRuleEngine<ArchRuleExampleType, ArchRuleExampleDependency>
    {
        public ArchRuleExampleRuleEngine(IEnumerable<ITypeRule<ArchRuleExampleType>> typeRules, IEnumerable<IDependencyRule<ArchRuleExampleType, ArchRuleExampleDependency>> dependencyRules, ArchRuleExampleRuleEngineMapper mapper) : base(typeRules, dependencyRules, mapper)
        {
        }
    }
}