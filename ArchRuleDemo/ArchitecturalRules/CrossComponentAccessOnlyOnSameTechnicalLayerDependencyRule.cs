// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using YADA.Core.DependencyRuleEngine;
using ArchRuleDemo.ArchitecturalModel;
using ArchRuleDemo.ArchRuleExampleDependencyRuleEngine;
using YADA.Core.DependencyRuleEngine.Rules;
using YADA.Core.DependencyRuleEngine.Feedback;
using YADA.Core.Analyser;

namespace ArchRuleDemo.ArchitecturalRules
{
    public class BaseDependencyRule<T, K> : IDependencyRule<ITypeDescription, IDependency>
    {
        private readonly IDependencyRule<T, K> m_InternalRule;

        private readonly IDependencyRuleInputMapper<T, K> m_Mapper;
        
        public BaseDependencyRule(IDependencyRule<T, K> internalRule, IDependencyRuleInputMapper<T, K> mapper)
        {
            m_InternalRule = internalRule;
            m_Mapper = mapper;
        }

        public DependencyRuleResult Apply(ITypeDescription type, IDependency dependency, IFeedbackCollector feedback)
        {
            return m_InternalRule.Apply(m_Mapper.MapTypeDescription(type), m_Mapper.MapDependency(dependency), feedback);
        }
    }


    public class CrossComponentAccessOnlyOnSameTechnicalLayerDependencyRule : IDependencyRule<ArchRuleExampleType, ArchRuleExampleDependency>
    {
        public DependencyRuleResult Apply(ArchRuleExampleType type, ArchRuleExampleDependency dependency, IFeedbackCollector feedback)
        {
            if (!dependency.Valid)
            {
                return DependencyRuleResult.Ignore;
            }

            if (type.Module.Name == dependency.Module.Name)
            {
                return DependencyRuleResult.Approve;
            }

            if (type.TechnicalLayer.Layer == dependency.TechnicalLayer.Layer)
            {
                return DependencyRuleResult.Approve;
            }
            var dependencyFeedback = feedback.AddFeedbackForType(type.FullName).ViolatesRule(nameof(CrossComponentAccessOnlyOnSameTechnicalLayerDependencyRule)).ForbiddenDependency(dependency.DependencyType.FullName);

            foreach (var item in dependency.Context)
            {
                dependencyFeedback.At(item);
            }




            return DependencyRuleResult.Reject;
        }
    }
}