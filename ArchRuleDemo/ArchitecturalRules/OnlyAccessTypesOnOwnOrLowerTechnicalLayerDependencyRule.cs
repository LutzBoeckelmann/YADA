// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using YADA.Core;
using YADA.Core.DependencyRuleEngine;
using YADA.Core.DependencyRuleEngine.Impl;

namespace YADA.Test
{
    public class OnlyAccessTypesOnOwnOrLowerTechnicalLayerDependencyRule : IDependencyRule<ArchRuleExampleType, ArchRuleExampleDependency>
    {
        public DependencyRuleResult Apply(ArchRuleExampleType type, ArchRuleExampleDependency dependency, IFeedbackCollector feedback)
        {
            if(!dependency.Valid) 
            {
                return DependencyRuleResult.Ignore;
            }
            
            if(dependency.TechnicalLayer.MayBeAccessedFrom(type.TechnicalLayer))
            {
                return DependencyRuleResult.Approve;
            }
                        
            var dependencyFeedback = feedback.AddFeedbackForType(type.FullName)
                .ViolatesRule(nameof(OnlyAccessTypesOnOwnOrLowerTechnicalLayerDependencyRule))
                .ForbiddenDependency(dependency.DependencyType.FullName);

            foreach(var context in dependency.Context) 
            {
                dependencyFeedback.At(context);
            }
                        
            return DependencyRuleResult.Reject;
        }
    }
}