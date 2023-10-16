// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using YADA.DependencyRuleEngine;
using ArchRuleDemo.ArchitecturalModel;
using ArchRuleDemo.ArchRuleExampleDependencyRuleEngine;
using YADA.DependencyRuleEngine.Rules;
using YADA.DependencyRuleEngine.Feedback;

namespace ArchRuleDemo.ArchitecturalRules
{
    public class CrossComponentAccessOnlyOnSameTechnicalLayerDependencyRule : IDependencyRule<ArchRuleExampleType, ArchRuleExampleDependency>
    {
        public DependencyRuleResult Apply(ArchRuleExampleType type, ArchRuleExampleDependency dependency, IFeedbackCollector feedback)
        {
            if (!dependency.Valid)
            {
                return DependencyRuleResult.Ignored;
            }

            if (type.Module.Name == dependency.Module.Name)
            {
                return DependencyRuleResult.Approve;
            }

            if (type.TechnicalLayer.Layer == dependency.TechnicalLayer.Layer)
            {
                return DependencyRuleResult.Approve;
            }
            var dependencyFeedback = feedback.AddFeedbackForType(type.FullName).ViolatesRule(nameof(CrossComponentAccessOnlyOnSameTechnicalLayerDependencyRule)).ForbiddenDependency(dependency.DependencyType.FullName);

            foreach (var item in dependency.Context)
            {
                dependencyFeedback.At(item);
            }




            return DependencyRuleResult.Reject;
        }
    }
}