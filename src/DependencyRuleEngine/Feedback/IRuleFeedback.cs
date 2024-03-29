// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

namespace YADA.DependencyRuleEngine.Feedback
{
    /// <summary>
    /// An interface to provided additional feedback for a violated rule.
    /// </summary>
    public interface IRuleFeedback : IFeedback
    {
        /// <summary>
        /// Additional information. Add more specific information what was violated if needed.
        /// </summary>
        /// <param name="info">The additional information</param>
        /// <returns>Returns this IRuleFeedback instance self</returns>
        IRuleFeedback AddInfo(string info);
        /// <summary>
        /// Adds the dependency which introduces the violation to the feedback.
        /// </summary>
        /// <param name="dependency">The dependency</param>
        /// <returns>A feedback interface to add specific information of the dependency</returns>
        IDependencyFeedback ForbiddenDependency(string dependency);
    }
}

