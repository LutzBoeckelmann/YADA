// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Collections.Generic;
using YADA.Core.Analyser;

namespace YADA.Core.DependencyRuleEngine.Impl
{
    public class DependencyRuleEngine : BaseDependencyRuleEngine<ITypeDescription, IDependency>
    {
        public DependencyRuleEngine(IEnumerable<ITypeRule<ITypeDescription>> typeRules, IEnumerable<IDependencyRule<ITypeDescription, IDependency>> dependencyRules) : base(typeRules, dependencyRules, new DefaultInputMapper())
        {
        }
    }
}

