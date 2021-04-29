// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using YADA.Core.DependencyRuleEngine.Feedback;

namespace YADA.Core.DependencyRuleEngine.Rules
{
    public interface ITypeRule<T>
    {
        DependencyRuleResult Apply(T type, IFeedbackCollector feedback);
    }
}

