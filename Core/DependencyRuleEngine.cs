// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Collections.Generic;
using System.Linq;

namespace YADA.Core
{
    public interface ITypeRule 
    {
        bool Apply(ITypeDescription type);
    }
    public interface IDependencyRule
    {
        bool Apply(ITypeDescription type, IDependency dependency);
    }


    public class DependencyRuleEngine 
    {
        /*
        
        - pre filter 
        - sub rules
        
        - At least on rule needs to apply
        - No rule may reject
        
        */
        private readonly List<IDependencyRule> m_DependencyRules;

        private readonly List<ITypeRule> m_TypeRules;

        public DependencyRuleEngine( IEnumerable<ITypeRule> typeRules, IEnumerable<IDependencyRule> dependencyRules)
        {
            m_DependencyRules = dependencyRules.ToList();
            m_TypeRules = typeRules.ToList();
        }

        public bool Analyse(IEnumerable<ITypeDescription> types) 
        {
            foreach(var type in types) 
            {
                var typeAccepted = CheckType(type);
                if (typeAccepted)
                {
                    CheckDependencies(type);
                }
            }
            return true;
        }
        private bool CheckType(ITypeDescription type) 
        {
            var result = true;

            foreach (var typeRule in m_TypeRules) 
            {
                result &= typeRule.Apply(type);
            }

            return result;
        }

        private bool CheckDependencies(ITypeDescription type) 
        {
            var result = true;
            foreach (var dependency in type.Dependencies)
            {
                foreach (var rule in m_DependencyRules)
                {
                    result &= rule.Apply(type, dependency);
                }
            }

            return result;
        }
    }
}
