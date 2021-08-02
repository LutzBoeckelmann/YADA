// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

namespace YADA.Core.DependencyRuleEngine.Feedback
{
    /// <summary>
    /// An interface to provided additional feedback for a violated rule.
    /// </summary>
    public interface IRuleFeedback
    {
        /// <summary>
        /// Additional information. Add more specific information what was violated if needed.
        /// </summary>
        /// <param name="info">The additional infomation</param>
        /// <returns>Returns this rulefeedback instance self</returns>
        IRuleFeedback AddInfo(string info);
        /// <summary>
        /// Adds the dependency which introduces the violation to the feedback.
        /// </summary>
        /// <param name="dependency">The dependency</param>
        /// <returns>A feedback interface to add specific information of the dependency</returns>
        IDependencyFeedback ForbiddenDependency(string dependency);
    }
}

