// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using YADA.Core;

namespace YADA.Test
{
    public class CorrectNamespaceTypeRule : ITypeRule<ArchRuleExampleType>
    {
        public DependencyRuleResult Apply(ArchRuleExampleType type, IFeedbackSet feedback)
        {
            if(!type.Valid) 
            {
                using (var sub = feedback.AddViolatedRuleFeedback(nameof(CorrectNamespaceTypeRule), type.FullName, "has not the correct namespace")) 
                {
                    if(!type.DomainLayer.Valid) 
                    {
                        sub.Add("DomainLayer unexpected");
                    }
                    if(!type.TechnicalLayer.Valid) 
                    {
                        sub.Add("TechnicalLayer unexpected");
                    }
                }
                
                return DependencyRuleResult.Reject;
            }
            
            return DependencyRuleResult.Approve;
        }
    }
}