// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

namespace YADA.Core.DependencyRuleEngine.Feedback
{
    /// <summary>
    /// An interface to extract the information collected during the 
    /// analyse run. The different feedback instances calls the correct
    /// method where the information can be further processed.
    /// 
    /// The methods are called during the exploration of the feedback
    /// in order of the appearance.
    /// 
    /// The order of the methods in this interface follows the logical 
    /// hierarchy of the feedback collection.
    /// 
    /// At the moment the feedback works string based.
    /// </summary>
    public interface IFeedbackVisitor
    {
        /// <summary>
        /// The type containing any violations
        /// </summary>
        /// <param name="type">Type name</param>
        void Type(string type);
        /// <summary>
        /// The violated rule which has added this feedback.
        /// </summary>
        /// <param name="rule">Name of the rule</param>
        void ViolatedRule(string rule);
        /// <summary>
        /// An additional information added to a rule feedback
        /// </summary>
        /// <param name="msg">The information</param>
        void Info(string msg);
        /// <summary>
        /// The dependency which violates a specific rule
        /// </summary>
        /// <param name="dependency">The dependency</param>
        void ForbiddenDependency(string dependency);
        /// <summary>
        /// The context in which a violating dependency was found
        /// </summary>
        /// <param name="context">Context information</param>
        void Context(string context);

    }
}