// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Collections.Generic;

namespace YADA.DependencyRuleEngine.Feedback
{
    internal class TypeFeedback : ITypeFeedback
    {
        private readonly Dictionary<string, RuleFeedback> m_RuleViolations = new Dictionary<string, RuleFeedback>();

        public IRuleFeedback ViolatesRule(string nameOfRule)
        {
            if (!m_RuleViolations.ContainsKey(nameOfRule))
            {
                m_RuleViolations.Add(nameOfRule, new RuleFeedback());
            }

            return m_RuleViolations[nameOfRule];
        }

        public void Explore(IFeedbackVisitor visitorExt)
        {
             foreach (var pair in m_RuleViolations)
            {
                using (visitorExt.ViolatedRule(pair.Key))
                {
                    pair.Value.Explore(visitorExt);
                }

            }
        }
    }
}

