// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

namespace YADA.Core.DependencyRuleEngine
{
    public interface IDependencyRule<T,K>
    {
        DependencyRuleResult Apply(T type, K dependency, IFeedbackCollector feedback);
    }
}

