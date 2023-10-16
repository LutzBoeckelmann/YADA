// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using YADA.DependencyRuleEngine;
using ArchRuleDemo.ArchitecturalModel;
using ArchRuleDemo.ArchRuleExampleDependencyRuleEngine;
using YADA.DependencyRuleEngine.Rules;
using YADA.DependencyRuleEngine.Feedback;

namespace ArchRuleDemo.ArchitecturalRules
{
    public class OnlyAccessTypesOnOwnOrLowerDomainLayerDependencyRule : IDependencyRule<ArchRuleExampleType, ArchRuleExampleDependency>
    {
        public DependencyRuleResult Apply(ArchRuleExampleType type, ArchRuleExampleDependency dependency, IFeedbackCollector feedback)
        {
            if (!dependency.Valid)
            {
                return DependencyRuleResult.Ignored;
            }

            if (dependency.DomainLayer.MayBeAccessedFrom(type.DomainLayer))
            {
                return DependencyRuleResult.Approve;
            }

            var dependencyFeedback = feedback.AddFeedbackForType(type.FullName)
                .ViolatesRule(nameof(OnlyAccessTypesOnOwnOrLowerDomainLayerDependencyRule))
                .ForbiddenDependency(dependency.DependencyType.FullName);

            foreach (var context in dependency.Context)
            {
                dependencyFeedback.At(context);
            }


            return DependencyRuleResult.Reject;
        }
    }
}