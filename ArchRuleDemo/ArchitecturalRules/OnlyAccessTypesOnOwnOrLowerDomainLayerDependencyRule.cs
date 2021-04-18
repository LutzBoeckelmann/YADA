// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using YADA.Core;

namespace YADA.Test
{
    public class OnlyAccessTypesOnOwnOrLowerDomainLayerDependencyRule : IDependencyRule<ArchRuleExampleType, ArchRuleExampleDependency>
    {
        public DependencyRuleResult Apply(ArchRuleExampleType type, ArchRuleExampleDependency dependency)
        {
            if(!dependency.Valid) 
            {
                return DependencyRuleResult.Ignore;
            }
            
            if(dependency.DomainLayer.MayBeAccessedFrom(type.DomainLayer))
            {
                return DependencyRuleResult.Approve;
            }

            return DependencyRuleResult.Reject;
        }
    }
}