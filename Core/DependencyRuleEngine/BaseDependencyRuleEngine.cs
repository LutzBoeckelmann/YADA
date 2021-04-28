// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Collections.Generic;
using System.Linq;
using YADA.Core.Analyser;

namespace YADA.Core.DependencyRuleEngine.Impl
{
    public abstract class BaseDependencyRuleEngine<T, K> 
    {
        private readonly IDependencyRuleInputMapper<T, K> m_Mapper;

        private readonly List<IDependencyRule<T,K>> m_DependencyRules;

        private readonly List<ITypeRule<T>> m_TypeRules;

        protected BaseDependencyRuleEngine( IEnumerable<ITypeRule<T>> typeRules, IEnumerable<IDependencyRule<T,K>> dependencyRules, IDependencyRuleInputMapper<T, K> mapper)
        {
            m_DependencyRules = dependencyRules.ToList();
            m_TypeRules = typeRules.ToList();
            m_Mapper = mapper;
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
                result.Add(typeRule.Apply(m_Mapper.MapTypeDescription(type), feedback));
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
                    result.Add(rule.Apply(m_Mapper.MapTypeDescription(type), m_Mapper.MapDependency(dependency), feedback));
                }
            }

            return hasNoDependencies || result.Approved();
        }
    }
}

