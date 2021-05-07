// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Collections.Generic;
using YADA.Core.DependencyRuleEngine.Rules;
using YADA.Core.Analyser;
using System.Linq;
using YADA.Core.DependencyRuleEngine.Feedback;

namespace YADA.Core.DependencyRuleEngine
{
    public class DependencyRuleEngine
    {
        private readonly List<IDependencyRule<ITypeDescription, IDependency>> m_DependencyRules;

        private readonly List<ITypeRule<ITypeDescription>> m_TypeRules;

        public DependencyRuleEngine(IEnumerable<ITypeRule<ITypeDescription>> typeRules, IEnumerable<IDependencyRule<ITypeDescription, IDependency>> dependencyRules)
        {
            m_DependencyRules = dependencyRules.ToList();
            m_TypeRules = typeRules.ToList();
        }

        public bool Analyse(IEnumerable<ITypeDescription> types, IFeedbackCollector feedback) 
        {
            bool result = false;
            bool allDependenciesOk = true;
            foreach(var type in types) 
            {
                var typeAccepted = CheckType(type, feedback);

                result |= typeAccepted != DependencyRuleResult.Reject;

                if (typeAccepted != DependencyRuleResult.Skip)
                {
                    allDependenciesOk &= CheckDependencies(type, feedback);
                }
            }
            
            return result && allDependenciesOk;
        }

        private DependencyRuleResult CheckType(ITypeDescription type, IFeedbackCollector feedback) 
        {
            DependencyRuleResultAggregation result = new DependencyRuleResultAggregation();
            
            foreach (var typeRule in m_TypeRules) 
            {
                var ruleResult = typeRule.Apply(type, feedback);
                
                result.Add(ruleResult);
                if(ruleResult == DependencyRuleResult.Skip) 
                {
                    break;
                }
            }

            return result.AggregatedResult();
        }

        private bool CheckDependencies(ITypeDescription type, IFeedbackCollector feedback) 
        {
            DependencyRuleResultAggregation result = new DependencyRuleResultAggregation();
            bool hasNoDependencies = true;
            foreach (var dependency in type.Dependencies)
            {
                hasNoDependencies = false;
                foreach (var rule in m_DependencyRules)
                {
                    result.Add(rule.Apply(type, dependency, feedback));
                }
            }

            return hasNoDependencies || result.AggregatedResult() != DependencyRuleResult.Reject;
        }
    }
}

