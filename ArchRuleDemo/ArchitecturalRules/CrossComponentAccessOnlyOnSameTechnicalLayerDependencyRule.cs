// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using YADA.Core.DependencyRuleEngine;
using YADA.Core.DependencyRuleEngine.Impl;

namespace YADA.Test
{
    public class CrossComponentAccessOnlyOnSameTechnicalLayerDependencyRule : IDependencyRule<ArchRuleExampleType, ArchRuleExampleDependency>
    {
        public DependencyRuleResult Apply(ArchRuleExampleType type, ArchRuleExampleDependency dependency, IFeedbackCollector feedback)
        {
            if(!dependency.Valid) 
            {
                return DependencyRuleResult.Ignore;
            }

            if(type.Module.Name == dependency.Module.Name) 
            {
                return DependencyRuleResult.Approve;
            }

            if(type.TechnicalLayer.Layer == dependency.TechnicalLayer.Layer) 
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