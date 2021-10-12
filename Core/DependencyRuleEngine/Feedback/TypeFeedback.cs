// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Collections.Generic;

namespace YADA.Core.DependencyRuleEngine.Feedback
{
    internal class TypeFeedback : ITypeFeedback
    {
        private Dictionary<string, RuleFeedback> m_RuleViolations = new Dictionary<string, RuleFeedback>();

        public IRuleFeedback ViolatesRule(string nameOfRule)
        {
            if (!m_RuleViolations.ContainsKey(nameOfRule))
            {
                m_RuleViolations.Add(nameOfRule, new RuleFeedback());
            }

            return m_RuleViolations[nameOfRule];
        }

        public void Explore(IFeedbackVisitor visitor) 
        {
            foreach (var pair in m_RuleViolations)
            {
                visitor.ViolatedRule(pair.Key);
                pair.Value.Explore(visitor);

            }
        }

   
    }
}

