// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt
using ArchRuleDemo.ArchitecturalModel;
using YADA.DependencyRuleEngine.Feedback;
using YADA.DependencyRuleEngine.Rules;
using YADA.DependencyRuleEngine;
using ArchRuleDemo.ArchRuleExampleDependencyRuleEngine;

namespace ArchRuleDemo.ArchitecturalRules
{

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

    public class CorrectNamespaceTypeRule2 : AbstractBaseTypeRule<ArchRuleExampleType, ArchRuleExampleDependency>
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