// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using YADA.DependencyRuleEngine.Feedback;
using YADA.Analyzer;

namespace YADA.DependencyRuleEngine.Rules
{
    /// <summary>
    /// An adapter to map any typed IDependencyRule<T,K> to IDependencyRule<ITypeDescription, IDependency> to make it usable
    /// in the rule engine.
    /// </summary>
    /// <typeparam name="T">A specific type definition</typeparam>
    /// <typeparam name="K">A specific dependency definition</typeparam>
    public class BaseDependencyRule<T, K> : IDependencyRule<ITypeDescription, IDependency>
    {
        private readonly IDependencyRule<T, K> m_InternalRule;

        private readonly IDependencyRuleInputMapper<T, K> m_Mapper;

        /// <summary>
        /// Constructors a adapter for IDependencyRule<T, K>.
        /// </summary>
        /// <param name="internalRule">The rule which should be adapted</param>
        /// <param name="mapper">A suitable mapper for T and K</param>        
        public BaseDependencyRule(IDependencyRule<T, K> internalRule, IDependencyRuleInputMapper<T, K> mapper)
        {
            m_InternalRule = internalRule;
            m_Mapper = mapper;
        }

        public DependencyRuleResult Apply(ITypeDescription type, IDependency dependency, IFeedbackCollector feedback)
        {
            return m_InternalRule.Apply(m_Mapper.MapTypeDescription(type), m_Mapper.MapDependency(dependency), feedback);
        }
    }
}