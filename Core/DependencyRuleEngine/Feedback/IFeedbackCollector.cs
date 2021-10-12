// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

namespace YADA.Core.DependencyRuleEngine.Feedback
{
    /// <summary>
    /// The root interface to add feedback.
    /// </summary>
    public interface IFeedbackCollector : IFeedback
    {
        /// <summary>
        /// Add feedback for a type.
        /// </summary>
        /// <param name="type">The name of the type</param>
        /// <returns>A specialized feedback interface to add feedback to the type</returns>
        ITypeFeedback AddFeedbackForType(string type);
    }
}

