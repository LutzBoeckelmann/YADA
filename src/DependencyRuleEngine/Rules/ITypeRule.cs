// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using YADA.DependencyRuleEngine.Feedback;

namespace YADA.DependencyRuleEngine.Rules
{
    /// <summary>
    /// A type rule checks a type in general. So types which should not be further analysed may be skipped 
    /// or types with a invalid fullqualified name can be rejected.
    /// At least one rule needs to approve the type for further processing.
    /// </summary>
    /// <typeparam name="T">Generic parameter decribing the Type, the engine maps ITypeDescription to T</typeparam>
    public interface ITypeRule<T>
    {
        /// <summary>
        /// Apply rule on given type.
        /// </summary>
        /// <param name="type">The type to be checked</param>
        /// <param name="feedback">FeedbackCollector to provide feedback from this rule</param>
        /// <returns></returns>
        DependencyRuleResult Apply(T type, IFeedbackCollector feedback);
    }
}

