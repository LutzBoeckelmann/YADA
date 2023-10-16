// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using YADA.DependencyRuleEngine.Feedback;
using YADA.Analyzer;

namespace YADA.DependencyRuleEngine.Rules
{
    public abstract class AbstractBaseTypeRule<T,K> : ITypeRule<ITypeDescription>
    {
        private readonly IDependencyRuleInputMapper<T, K> m_Mapper;
        public AbstractBaseTypeRule(IDependencyRuleInputMapper<T, K> mapper)
        {
            m_Mapper = mapper;
        }

        public DependencyRuleResult Apply(ITypeDescription type, IFeedbackCollector feedback)
        {
            return InternalApply(m_Mapper.MapTypeDescription(type), feedback);
        }

        protected abstract DependencyRuleResult InternalApply(T type, IFeedbackCollector feedback);
    }

}