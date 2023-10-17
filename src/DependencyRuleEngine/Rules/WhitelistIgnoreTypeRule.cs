// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Collections.Generic;
using System.Linq;
using YADA.Analyzer;
using YADA.DependencyRuleEngine.Feedback;

namespace  YADA.DependencyRuleEngine.Rules
{
    /// <summary>
    /// A rule to skip any type which is not contained in the given white list
    /// </summary>
    public class WhitelistIgnoreTypeRule : ITypeRule<ITypeDescription>
    {
        private readonly IList<string> m_Whitelist;

        public WhitelistIgnoreTypeRule(IEnumerable<string> typesOnWhitelist)
        {
            m_Whitelist = typesOnWhitelist.ToList();
        }

        public DependencyRuleResult Apply(ITypeDescription type, IFeedbackCollector feedback)
        {
            if(!m_Whitelist.Contains(type.FullName)) 
            {
                return DependencyRuleResult.Skip;
            }
            return DependencyRuleResult.Ignored;
        }
    }
}