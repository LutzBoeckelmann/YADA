// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

namespace YADA.Core.DependencyRuleEngine.Feedback
{
    public interface IRuleFeedback
    {
        IRuleFeedback AddInfo(string name);
        IDependencyFeedback ForbiddenDependency(string dependency);
    }
}

