// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using YADA.Core;

namespace YADA.Test
{
    public class CrossComponentAccessOnlyOnSameTechnicalLayerDependencyRule : IDependencyRule<ArchRuleExampleType, ArchRuleExampleDependency>
    {
        public DependencyRuleResult Apply(ArchRuleExampleType type, ArchRuleExampleDependency dependency)
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

            return DependencyRuleResult.Reject;
        }
    }
}