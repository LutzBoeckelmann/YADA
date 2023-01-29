// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Collections.Generic;
using System.Linq;
using YADA.Core.Analyser;
using YADA.Core.DependencyRuleEngine.Feedback;

namespace  YADA.Core.DependencyRuleEngine.Rules
{
    /// <summary>
    /// 
    /// </summary>
    public class WhitelistIgnoreTypeRule : ITypeRule<ITypeDescription>
    {
        private readonly IList<string> m_Whitelist = new List<string>();

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