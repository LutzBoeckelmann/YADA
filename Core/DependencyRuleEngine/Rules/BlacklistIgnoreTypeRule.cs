// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Collections.Generic;
using System.Linq;
using YADA.Core.Analyser;
using YADA.Core.DependencyRuleEngine.Feedback;

namespace YADA.Core.DependencyRuleEngine.Rules
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