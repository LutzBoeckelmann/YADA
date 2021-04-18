// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using YADA.Core;

namespace YADA.Test
{
    public class OnlyAccessTypesOnOwnOrLowerTechnicalLayerDependencyRule : IDependencyRule<ArchRuleExampleType, ArchRuleExampleDependency>
    {
        public DependencyRuleResult Apply(ArchRuleExampleType type, ArchRuleExampleDependency dependency)
        {
            if(!dependency.Valid) 
            {
                return DependencyRuleResult.Ignore;
            }
            
            if(dependency.TechnicalLayer.MayBeAccessedFrom(type.TechnicalLayer))
            {
                return DependencyRuleResult.Approve;
            }

            return DependencyRuleResult.Reject;
        }
    }
}