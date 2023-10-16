// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using YADA.DependencyRuleEngine.Feedback;

namespace YADA.DependencyRuleEngine.Rules
{
    /// <summary>
    /// A dependency rule checks a specific dependency of a type. So any dependency rule will be called for any dependency of any type to
    /// be analysed. This kind of rules can be used accept or reject the dependency between types. Also dependencies to types may be ignored 
    /// or can be skipped. In opposite of ignore, skip does suppress further processing of the dependency.
    /// </summary>
    /// <typeparam name="T">Generic parameter decribing the Type, the engine maps ITypeDescription to T</typeparam>
    /// <typeparam name="K">Generic parameter decribing the Dependency of a type, the engine maps IDependencyDescription to K</typeparam>
    public interface IDependencyRule<T, K>
    {
        /// <summary>
        /// Apply the rule at the given dependency of the given type.
        /// </summary>
        /// <param name="type">The type which declares the dependency</param>
        /// <param name="dependency">A representation of the dependency of the type </param>
        /// <param name="feedback">FeedbackCollector to provide feedback from this rule</param>
        /// <returns></returns>
        DependencyRuleResult Apply(T type, K dependency, IFeedbackCollector feedback);
    }
}

