// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

namespace YADA.Core.DependencyRuleEngine.Feedback
{
    public interface IDependencyFeedback
    {
        IDependencyFeedback At(string context);
    }
}

