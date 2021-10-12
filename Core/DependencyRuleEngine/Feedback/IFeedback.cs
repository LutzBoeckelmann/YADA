// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

namespace YADA.Core.DependencyRuleEngine.Feedback
{
    /// <summary>
    /// Base interface for any feedback
    /// </summary>
    public interface IFeedback 
    {
        /// <summary>
        /// Explores this instance and its childs recursive
        /// </summary>
        /// <param name="visitor">A visitor to add the result</param>
        void Explore(IFeedbackVisitor visitor);
    }
}