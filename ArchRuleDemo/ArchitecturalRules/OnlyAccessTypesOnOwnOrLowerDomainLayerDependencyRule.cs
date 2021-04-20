// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using YADA.Core;

namespace YADA.Test
{
    public class OnlyAccessTypesOnOwnOrLowerDomainLayerDependencyRule : IDependencyRule<ArchRuleExampleType, ArchRuleExampleDependency>
    {
        public DependencyRuleResult Apply(ArchRuleExampleType type, ArchRuleExampleDependency dependency, IFeedbackSet feedback)
        {
            if(!dependency.Valid) 
            {
                return DependencyRuleResult.Ignore;
            }
            
            if(dependency.DomainLayer.MayBeAccessedFrom(type.DomainLayer))
            {
                return DependencyRuleResult.Approve;
            }

            feedback.AddFeedback(nameof(OnlyAccessTypesOnOwnOrLowerDomainLayerDependencyRule), type.FullName, $"to {dependency.DependencyType.FullName}");
            return DependencyRuleResult.Reject;
        }
    }
}