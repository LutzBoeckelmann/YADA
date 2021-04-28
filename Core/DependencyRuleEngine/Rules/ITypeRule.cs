// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt
using YADA.Core.DependencyRuleEngine.Impl;

namespace YADA.Core.DependencyRuleEngine
{
    public interface ITypeRule <T>
    {
        DependencyRuleResult Apply(T type, IFeedbackCollector feedback);
    }
}

