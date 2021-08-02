// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

namespace YADA.Core.DependencyRuleEngine.Feedback
{   
    /// <summary>
    /// Feedback interface a type level.
    /// </summary>
    public interface ITypeFeedback
    {
        /// <summary>
        /// Add feedback for the current type. 
        /// </summary>
        /// <param name="nameOfRule">Name of the rule which is violated by the current type</param>
        /// <returns>An IRuleFeedback interface for further feedback</returns>
        IRuleFeedback ViolatesRule(string nameOfRule);
    }
}

