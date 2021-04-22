// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        DependencyRuleResult Apply(T type, IFeedbackCollector feedback);
    }

    public interface IDependencyRule<T,K>
    {
        DependencyRuleResult Apply(T type, K dependency, IFeedbackCollector feedback);
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

    public interface IFeedbackCollector 
    {
        ITypeFeedback AddFeedbackForType(string type);
    }

    public interface ITypeFeedback 
    {
        IRuleFeedback ViolatesRule(string nameOfRule);
    }

    public interface IRuleFeedback 
    {
        IRuleFeedback AddInfo(string name);
        IDependencyFeedback ForbiddenDependency(string dependency);
    }

    public interface IDependencyFeedback 
    {
        IDependencyFeedback At(string context);
    }

    public class FeedbackCollector : IFeedbackCollector
    {
        private readonly IDictionary<string, TypeFeedback> m_Feedbacks = new Dictionary<string, TypeFeedback>();
        public ITypeFeedback AddFeedbackForType(string type)
        {
            if(!m_Feedbacks.ContainsKey(type)) 
            {
                m_Feedbacks.Add(type, new TypeFeedback());
            }

            return m_Feedbacks[type];
        }

        public StringBuilder Print() 
        {
            StringBuilder result = new StringBuilder();

            foreach(var pair in m_Feedbacks)
            {
                result.AppendLine(pair.Key);
                pair.Value.Print(result);
            }

            return result;
        }
    }

    public class TypeFeedback : ITypeFeedback
    {
   
        private Dictionary<string, RuleFeedback> m_RuleViolations = new Dictionary<string, RuleFeedback>();
   
       
        public IRuleFeedback ViolatesRule(string nameOfRule)
        {
           if(!m_RuleViolations.ContainsKey(nameOfRule)) 
            {
                m_RuleViolations.Add(nameOfRule, new RuleFeedback());
            }
            
            return m_RuleViolations[nameOfRule];
        }
        public void Print(StringBuilder result)
        {
            foreach(var pair in m_RuleViolations)
            {
                result.AppendLine($"  ViolatesRule: {pair.Key}");
                pair.Value.Print(result);
            }
        }

     
    }

    public class RuleFeedback : IRuleFeedback
    {
        private readonly List<string> m_Infos = new List<string>();
        private readonly Dictionary<string, DependencyFeedback> m_ViolatedDependency = new Dictionary<string, DependencyFeedback>();

        public IRuleFeedback AddInfo(string name)
        {
            m_Infos.Add(name);
            return this;
        }

        public IDependencyFeedback ForbiddenDependency(string dependency)
        {
            if (!m_ViolatedDependency.ContainsKey(dependency))
            {
                m_ViolatedDependency.Add(dependency, new DependencyFeedback());
            }

            return m_ViolatedDependency[dependency];
        }

        internal void Print(StringBuilder result)
        {
            foreach(var msg in m_Infos) 
            {
                result.AppendLine($"    Info: {msg}");
            }

            foreach(var dependency in m_ViolatedDependency) 
            {
                result.AppendLine($"    ForbiddenDependency {dependency.Key}");
                dependency.Value.Print(result);
            }
        }
    }

    public class DependencyFeedback : IDependencyFeedback
    {
        private readonly List< string> m_Contexted = new List<string>();
        public IDependencyFeedback At(string context)
        {
            m_Contexted.Add(context);
            return this;
        }

        internal void Print(StringBuilder result)
        {
            foreach(var context in m_Contexted) 
            {
                result.AppendLine($"    At: {context}");
            }
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
            DependencyRuleResultResultSet result = new DependencyRuleResultResultSet();

            foreach (var typeRule in m_TypeRules) 
            {
                result.Add(typeRule.Apply(m_Mapper.MapTypeDescription(type), feedback));
            }

            return result.Approved();
        }

        private bool CheckDependencies(ITypeDescription type, IFeedbackCollector feedback) 
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

