// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using YADA.Core.DependencyRuleEngine.Feedback;

namespace YADA.Core.DependencyRuleEngine.Rules
{
    public interface IDependencyRule<T, K>
    {
        DependencyRuleResult Apply(T type, K dependency, IFeedbackCollector feedback);
    }
}

