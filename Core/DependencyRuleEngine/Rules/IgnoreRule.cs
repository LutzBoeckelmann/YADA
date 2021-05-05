using System.Collections.Generic;
using YADA.Core.Analyser;
using YADA.Core.DependencyRuleEngine;
using YADA.Core.DependencyRuleEngine.Feedback;
using YADA.Core.DependencyRuleEngine.Rules;

namespace Core.DependencyRuleEngine.Rules
{
    public class IgnoreRuleTypeRule : ITypeRule<ITypeDescription>
    {
        private readonly IList<string> m_IgnoredTypes = new List<string>();
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