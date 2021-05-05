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
                result |= typeAccepted;

                if (typeAccepted)
                {
                    allDependenciesOk &= CheckDependencies(type, feedback);
                }
            }
            return result && allDependenciesOk;
        }

        private bool CheckType(ITypeDescription type, IFeedbackCollector feedback) 
        {
            DependencyRuleResultSet result = new DependencyRuleResultSet();

            foreach (var typeRule in m_TypeRules) 
            {
                result.Add(typeRule.Apply(type, feedback));
            }

            return result.Approved();
        }

        private bool CheckDependencies(ITypeDescription type, IFeedbackCollector feedback) 
        {
            DependencyRuleResultSet result = new DependencyRuleResultSet();
            bool hasNoDependencies = true;
            foreach (var dependency in type.Dependencies)
            {
                hasNoDependencies = false;
                foreach (var rule in m_DependencyRules)
                {
                    result.Add(rule.Apply(type, dependency, feedback));
                }
            }

            return hasNoDependencies || result.Approved();
        }
    }
}

