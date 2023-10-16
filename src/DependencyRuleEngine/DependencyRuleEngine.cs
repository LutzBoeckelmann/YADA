// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Collections.Generic;
using YADA.DependencyRuleEngine.Rules;
using YADA.Analyzer;
using System.Linq;
using YADA.DependencyRuleEngine.Feedback;

namespace YADA.DependencyRuleEngine
{
    /// <summary>
    /// A rule engine to analyze a set of types against a rule set. The engine will be configured 
    /// with a set of rules for types and dependencies.
    /// </summary>
    public class RuleEngine
    {
        private readonly List<IDependencyRule<ITypeDescription, IDependency>> m_DependencyRules;

        private readonly List<ITypeRule<ITypeDescription>> m_TypeRules;

        /// <summary>
        /// Construct a rule engine. 
        /// </summary>
        /// <param name="typeRules">Rules to check any type</param>
        /// <param name="dependencyRules">Rules to check any dependency</param>
        public RuleEngine(IEnumerable<ITypeRule<ITypeDescription>> typeRules, IEnumerable<IDependencyRule<ITypeDescription, IDependency>> dependencyRules)
        {
            m_DependencyRules = dependencyRules.ToList();
            m_TypeRules = typeRules.ToList();
        }

        /// <summary>
        /// Any given type will be checked against any configured rule.
        /// At first any type rule will be applied at any type. 
        /// No rule may reject the type, in this case the result is false.
        /// After a rule returns Skip the type will not further processed
        /// Skipping a type does not affect the overall result.
        /// 
        /// If at least one rule has approved the type, all dependency rules
        /// will be applied on the types dependencies. If the type has 
        /// dependencies at least one rule must approve any dependency.
        /// </summary>
        /// <param name="types">The set of types to be checked</param>
        /// <param name="feedback"></param>
        /// <returns>True if any type and dependency obey to any rule</returns>
        public bool Analyse(IEnumerable<ITypeDescription> types, IFeedbackCollector feedback) 
        {
            bool allTypesApproved = false;
            bool allDependenciesOk = true;
            foreach(var type in types) 
            {
                var typeAccepted = CheckType(type, feedback);

                var notRejected = typeAccepted != DependencyRuleResult.Reject;
                
                allTypesApproved |= notRejected;

                if (typeAccepted == DependencyRuleResult.Approve)
                {
                    allDependenciesOk &= CheckDependencies(type, feedback);
                }
            }
            
            return allTypesApproved && allDependenciesOk;
        }

        private DependencyRuleResult CheckType(ITypeDescription type, IFeedbackCollector feedback) 
        {
            DependencyRuleResultAggregation result = new DependencyRuleResultAggregation();
            
            foreach (var typeRule in m_TypeRules) 
            {
                var ruleResult = typeRule.Apply(type, feedback);
                
                result.Add(ruleResult);
                if(ruleResult == DependencyRuleResult.Skip || ruleResult == DependencyRuleResult.Reject) 
                {
                    return result.AggregatedResult();
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

