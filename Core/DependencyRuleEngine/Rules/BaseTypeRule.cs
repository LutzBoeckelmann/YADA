// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt
using YADA.Core.DependencyRuleEngine.Feedback;
using YADA.Core.Analyser;

namespace YADA.Core.DependencyRuleEngine.Rules
{
    /// <summary>
    /// An adapter to map any typed ITypeRule<T> to ITypeRule<ITypeDescription> to make it usable
    /// in the rule engine.
    /// </summary>
    /// <typeparam name="T">A specific type definition</typeparam>
    /// <typeparam name="K">A specific dependency definition, needed for a general mapper</typeparam>
    public class BaseTypeRule<T,K> : ITypeRule<ITypeDescription>
    {
        private readonly ITypeRule<T> m_InternalRule;
        private readonly IDependencyRuleInputMapper<T, K> m_Mapper;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="internalRule">The rule which should be adapted</param>
        /// <param name="mapper">The mapper to map T and K</param>
        public BaseTypeRule(ITypeRule<T> internalRule, IDependencyRuleInputMapper<T, K> mapper)
        {
            m_InternalRule = internalRule;
            m_Mapper = mapper;
        }

        public DependencyRuleResult Apply(ITypeDescription type, IFeedbackCollector feedback)
        {
            return m_InternalRule.Apply(m_Mapper.MapTypeDescription(type), feedback);
        }
    }
}