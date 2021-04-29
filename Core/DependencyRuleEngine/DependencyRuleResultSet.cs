// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Collections.Generic;
using System.Linq;

namespace YADA.Core.DependencyRuleEngine
{
    internal class DependencyRuleResultSet
    {
        private List<DependencyRuleResult> m_Results = new List<DependencyRuleResult>();

        public void Add(DependencyRuleResult result) 
        {
            m_Results.Add(result);
        }

        public bool Ignore() 
        {
            return m_Results.Any(i => i == DependencyRuleResult.Ignore);
        }

        public bool Approved() 
        {
            bool notRejected = true;
            bool approved = false;

            foreach(var part in m_Results) 
            {
                notRejected &= part != DependencyRuleResult.Reject;
                approved |= part == DependencyRuleResult.Approve;
            }

            return approved && notRejected;
        }
    }
}

