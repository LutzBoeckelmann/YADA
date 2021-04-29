// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

namespace YADA.Core.DependencyRuleEngine.Feedback
{
    public interface IFeedbackCollector
    {
        ITypeFeedback AddFeedbackForType(string type);
    }
}

