// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Collections.Generic;
using System.Linq;
using YADA.Analyzer;
using YADA.DependencyRuleEngine.Feedback;

namespace YADA.DependencyRuleEngine.Rules
{
    public class BlacklistIgnoreTypeRule : ITypeRule<ITypeDescription>
    {
        private readonly IList<string> m_IgnoredTypes = new List<string>();

        public BlacklistIgnoreTypeRule(IEnumerable<string> typesToIngore)
        {
            m_IgnoredTypes = typesToIngore.ToList();
        }
        public DependencyRuleResult Apply(ITypeDescription type, IFeedbackCollector feedback)
        {
            if(m_IgnoredTypes.Contains(type.FullName)) 
            {
                return DependencyRuleResult.Skip;
            }
            return DependencyRuleResult.Ignored;
        }
    }
}