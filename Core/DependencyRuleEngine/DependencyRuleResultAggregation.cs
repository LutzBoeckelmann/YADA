// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

namespace YADA.Core.DependencyRuleEngine
{
    internal class DependencyRuleResultAggregation 
    {
        private bool m_OneApproved;
        private bool m_Skipped;
        private bool m_Reject;

        public void Add(DependencyRuleResult result) 
        {
            if(result == DependencyRuleResult.Approve) 
            {
                m_OneApproved = true;
            }
            if(result == DependencyRuleResult.Skip)
            {
                m_Skipped = true;
            }
            if(result == DependencyRuleResult.Reject) 
            {
                m_Reject = true;
            }
        }

        public DependencyRuleResult AggregatedResult() 
        {
            if(m_Skipped) 
            {
                return DependencyRuleResult.Skip;
            }

            if(m_Reject) 
            {
                return DependencyRuleResult.Reject;
            }
          
            if(m_OneApproved) 
            {
                return DependencyRuleResult.Approve;
            }

            return DependencyRuleResult.Ignored;
        }
    }
}

