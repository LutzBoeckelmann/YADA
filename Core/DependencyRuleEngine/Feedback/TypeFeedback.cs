// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Collections.Generic;
using System.Text;

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
        public void Print(StringBuilder result)
        {
            foreach (var pair in m_RuleViolations)
            {
                result.AppendLine($"  ViolatesRule: {pair.Key}");
                pair.Value.Print(result);
            }
        }


    }
}

