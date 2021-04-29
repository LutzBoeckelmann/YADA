// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt
using ArchRuleDemo.ArchitecturalModel;
using YADA.Core.DependencyRuleEngine.Feedback;
using YADA.Core.DependencyRuleEngine.Rules;
using YADA.Core.DependencyRuleEngine;

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
}