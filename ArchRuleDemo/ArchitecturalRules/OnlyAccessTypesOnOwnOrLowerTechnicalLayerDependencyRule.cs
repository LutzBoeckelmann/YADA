// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using YADA.Core;

namespace YADA.Test
{
    public class OnlyAccessTypesOnOwnOrLowerTechnicalLayerDependencyRule : IDependencyRule<ArchRuleExampleType, ArchRuleExampleDependency>
    {
        public DependencyRuleResult Apply(ArchRuleExampleType type, ArchRuleExampleDependency dependency, IFeedbackSet feedback)
        {
            if(!dependency.Valid) 
            {
                return DependencyRuleResult.Ignore;
            }
            
            if(dependency.TechnicalLayer.MayBeAccessedFrom(type.TechnicalLayer))
            {
                return DependencyRuleResult.Approve;
            }
            
            feedback.AddFeedback(nameof(OnlyAccessTypesOnOwnOrLowerTechnicalLayerDependencyRule), type.FullName, $"to {dependency.DependencyType.FullName}");
            return DependencyRuleResult.Reject;
        }
    }
}