// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt
using ArchRuleDemo.ArchitecturalModel;
using YADA.Core.DependencyRuleEngine.Feedback;
using YADA.Core.DependencyRuleEngine.Rules;
using YADA.Core.DependencyRuleEngine;
using YADA.Core.Analyser;
using ArchRuleDemo.ArchRuleExampleDependencyRuleEngine;

namespace ArchRuleDemo.ArchitecturalRules
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
 
    public class CorrectNamespaceTypeRule : ITypeRule<ArchRuleExampleType>
    {
        public DependencyRuleResult Apply(ArchRuleExampleType type, IFeedbackCollector feedback)
        {
            if (!type.Valid)
            {
                if (!type.DomainLayer.Valid)
                {
                    feedback.AddFeedbackForType(type.FullName).ViolatesRule(nameof(CorrectNamespaceTypeRule)).AddInfo("DomainLayer unexpected");
                }

                if (!type.TechnicalLayer.Valid)
                {
                    feedback.AddFeedbackForType(type.FullName).ViolatesRule(nameof(CorrectNamespaceTypeRule)).AddInfo("TechnicalLayer unexpected");
                }

                return DependencyRuleResult.Reject;
            }

            return DependencyRuleResult.Approve;
        }
    }

    public abstract class BaseTypeRule2<T,K> : ITypeRule<ITypeDescription>
    {
        private readonly IDependencyRuleInputMapper<T, K> m_Mapper;
        public BaseTypeRule2(IDependencyRuleInputMapper<T, K> mapper)
        {
            m_Mapper = mapper;
        }

        public DependencyRuleResult Apply(ITypeDescription type, IFeedbackCollector feedback)
        {
            return InternalApply(m_Mapper.MapTypeDescription(type), feedback);
        }

        protected abstract DependencyRuleResult InternalApply(T type, IFeedbackCollector feedback);
    }

    public class CorrectNamespaceTypeRule2 : BaseTypeRule2<ArchRuleExampleType, ArchRuleExampleDependency>
    {
        public CorrectNamespaceTypeRule2(ArchRuleExampleRuleEngineMapper mapper) : base(mapper) {}

        protected override DependencyRuleResult InternalApply(ArchRuleExampleType type, IFeedbackCollector feedback)
        {
            if (!type.Valid)
            {
                if (!type.DomainLayer.Valid)
                {
                    feedback.AddFeedbackForType(type.FullName).ViolatesRule(nameof(CorrectNamespaceTypeRule)).AddInfo("DomainLayer unexpected");
                }

                if (!type.TechnicalLayer.Valid)
                {
                    feedback.AddFeedbackForType(type.FullName).ViolatesRule(nameof(CorrectNamespaceTypeRule)).AddInfo("TechnicalLayer unexpected");
                }

                return DependencyRuleResult.Reject;
            }

            return DependencyRuleResult.Approve;
        }
    }

}