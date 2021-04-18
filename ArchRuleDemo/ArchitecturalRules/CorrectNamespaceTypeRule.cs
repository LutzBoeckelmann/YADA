// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using YADA.Core;

namespace YADA.Test
{
    public class CorrectNamespaceTypeRule : ITypeRule<ArchRuleExampleType>
    {
        public DependencyRuleResult Apply(ArchRuleExampleType type)
        {
            if(!type.Valid) 
            {
                return DependencyRuleResult.Reject;
            }

            return DependencyRuleResult.Approve;
        }
    }
}