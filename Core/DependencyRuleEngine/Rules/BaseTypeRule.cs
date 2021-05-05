// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt
using YADA.Core.DependencyRuleEngine.Feedback;
using YADA.Core.Analyser;

namespace YADA.Core.DependencyRuleEngine.Rules
{
    public class BaseTypeRule<T,K> : ITypeRule<ITypeDescription>
    {
        private readonly ITypeRule<T> m_InternalRule;
        private readonly IDependencyRuleInputMapper<T, K> m_Mapper;
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