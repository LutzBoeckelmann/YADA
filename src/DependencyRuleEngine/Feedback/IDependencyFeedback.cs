// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using YADA.Analyzer;

namespace YADA.DependencyRuleEngine.Feedback
{
    /// <summary>
    /// A specialized feedback interface to add information about a dependency,
    /// which violated a rule.
    /// </summary>
    public interface IDependencyFeedback : IFeedback
    {
        /// <summary>
        /// Adds information about the context in which the dependency was found.
        /// </summary>
        /// <param name="context">Specific information of the location</param>
        /// <returns>This dependency feedback self, to support multiple information for a dependency</returns>
        IDependencyFeedback At(IDependencyContext context);
    }
}

