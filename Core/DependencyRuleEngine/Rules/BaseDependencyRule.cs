// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using YADA.Core.DependencyRuleEngine.Feedback;
using YADA.Core.Analyser;

namespace YADA.Core.DependencyRuleEngine.Rules
{
    public class BaseDependencyRule<T, K> : IDependencyRule<ITypeDescription, IDependency>
    {
        private readonly IDependencyRule<T, K> m_InternalRule;

        private readonly IDependencyRuleInputMapper<T, K> m_Mapper;
        
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