// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

namespace YADA.DependencyRuleEngine.Feedback
{
    /// <summary>
    /// Base interface for any feedback
    /// </summary>
    public interface IFeedback 
    {
        /// <summary>
        /// Explores this instance and its children recursive
        /// </summary>
        /// <param name="visitor">A visitor to add the result</param>
        void Explore(IFeedbackVisitor visitor);
    }
}