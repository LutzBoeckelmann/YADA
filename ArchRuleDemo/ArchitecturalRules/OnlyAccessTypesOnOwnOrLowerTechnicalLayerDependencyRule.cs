// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt


using YADA.Core.DependencyRuleEngine;

using ArchRuleDemo.ArchitecturalModel;
using ArchRuleDemo.ArchRuleExampleDependencyRuleEngine;
using YADA.Core.DependencyRuleEngine.Rules;
using YADA.Core.DependencyRuleEngine.Feedback;

namespace ArchRuleDemo.ArchitecturalRules
{
    public class OnlyAccessTypesOnOwnOrLowerTechnicalLayerDependencyRule : IDependencyRule<ArchRuleExampleType, ArchRuleExampleDependency>
    {
        public DependencyRuleResult Apply(ArchRuleExampleType type, ArchRuleExampleDependency dependency, IFeedbackCollector feedback)
        {
            if (!dependency.Valid)
            {
                return DependencyRuleResult.Ignore;
            }

            if (dependency.TechnicalLayer.MayBeAccessedFrom(type.TechnicalLayer))
            {
                return DependencyRuleResult.Approve;
            }

            var dependencyFeedback = feedback.AddFeedbackForType(type.FullName)
                .ViolatesRule(nameof(OnlyAccessTypesOnOwnOrLowerTechnicalLayerDependencyRule))
                .ForbiddenDependency(dependency.DependencyType.FullName);

            foreach (var context in dependency.Context)
            {
                dependencyFeedback.At(context);
            }

            return DependencyRuleResult.Reject;
        }
    }
}