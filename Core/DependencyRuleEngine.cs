// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System;
using System.Collections.Generic;
using System.Linq;

namespace YADA.Core
{
    public enum DependencyRuleResult
    {
        Approve,
        Ignore,
        Reject
    }

    public interface ITypeRule <T>
    {
        DependencyRuleResult Apply(T type, IFeedbackSet feedback);
    }

    public interface IDependencyRule<T,K>
    {
        DependencyRuleResult Apply(T type, K dependency, IFeedbackSet feedback);
    }


    public class DependencyRuleResultResultSet
    {
        private List<DependencyRuleResult> m_Results = new List<DependencyRuleResult>();

        public void Add(DependencyRuleResult result) 
        {
            m_Results.Add(result);
        }

        public bool Ignore() 
        {
            return m_Results.Any(i => i == DependencyRuleResult.Ignore);
        }

        public bool Approved() 
        {
            bool notRejected = true;
            bool approved = false;

            foreach(var part in m_Results) 
            {
                notRejected &= part != DependencyRuleResult.Reject;
                approved |= part == DependencyRuleResult.Approve;
            }

            return approved && notRejected;
        }
    }


    public class DependencyRuleEngine : BaseDependencyRuleEngine<ITypeDescription, IDependency>
    {
        public DependencyRuleEngine(IEnumerable<ITypeRule<ITypeDescription>> typeRules, IEnumerable<IDependencyRule<ITypeDescription, IDependency>> dependencyRules) : base(typeRules, dependencyRules, new DefaultInputMapper())
        {
        }
    }

    public class DefaultInputMapper : IDependencyRuleInputMapper<ITypeDescription, IDependency>
    {
        public IDependency MapDependency(IDependency dependency)
        {
            return dependency;
        }

        public ITypeDescription MapTypeDescription(ITypeDescription type)
        {
            return type;
        }
    }

    public interface IDependencyRuleInputMapper<T,K>
    {
        K MapDependency(IDependency dependency);

        T MapTypeDescription(ITypeDescription type);
    } 

    public interface IViolatedRuleFeedback :IDisposable
    {
        void Add(string message);

        
    }

    public interface IFeedbackSet 
    {
        void AddFeedback(string violatedRule, string type, string message);
        IViolatedRuleFeedback AddViolatedRuleFeedback(string violatedRule, string type, string message);
    }

    public class SimpleStringCollectionFeedbackSet : IFeedbackSet
    {
        private class ViolatedRuleFeedback : IViolatedRuleFeedback
        {
            private readonly SimpleStringCollectionFeedbackSet m_Owner;
            private int m_Position;

            public ViolatedRuleFeedback(SimpleStringCollectionFeedbackSet owner, int position)
            {
                m_Owner = owner;
                m_Position = position;
            }


            public void Add(string message)
            {
                if (m_Position == -1) { throw new ObjectDisposedException(nameof(IViolatedRuleFeedback)); }
                m_Owner.m_Results.Insert(m_Position++,$"  {message}");
            }

            public void Dispose()
            {
                m_Position = -1;
            }
        }
        private List<string> m_Results = new List<string>();

        public IEnumerable<string> Messages => m_Results;

        public void AddFeedback(string violatedRule, string type, string message)
        {
            m_Results.Add($"{violatedRule} at type {type} {message}");
        }

        public IViolatedRuleFeedback AddViolatedRuleFeedback(string violatedRule, string type, string message)
        {
            AddFeedback(violatedRule, type, message);
            return new ViolatedRuleFeedback(this, m_Results.Count);
        }
    }

    public abstract class BaseDependencyRuleEngine<T, K> 
    {
        /*
        
        - pre filter 
        - sub rules
        
        - At least on rule needs to apply
        - No rule may reject
        
        */
        private readonly IDependencyRuleInputMapper<T, K> m_Mapper;

        private readonly List<IDependencyRule<T,K>> m_DependencyRules;

        private readonly List<ITypeRule<T>> m_TypeRules;

        protected BaseDependencyRuleEngine( IEnumerable<ITypeRule<T>> typeRules, IEnumerable<IDependencyRule<T,K>> dependencyRules, IDependencyRuleInputMapper<T, K> mapper)
        {
            m_DependencyRules = dependencyRules.ToList();
            m_TypeRules = typeRules.ToList();
            m_Mapper = mapper;
        }

        public bool Analyse(IEnumerable<ITypeDescription> types, IFeedbackSet feedback) 
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

        private bool CheckType(ITypeDescription type, IFeedbackSet feedback) 
        {
            DependencyRuleResultResultSet result = new DependencyRuleResultResultSet();

            foreach (var typeRule in m_TypeRules) 
            {
                result.Add(typeRule.Apply(m_Mapper.MapTypeDescription(type), feedback));
            }

            return result.Approved();
        }

        private bool CheckDependencies(ITypeDescription type, IFeedbackSet feedback) 
        {
            DependencyRuleResultResultSet result = new DependencyRuleResultResultSet();
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

