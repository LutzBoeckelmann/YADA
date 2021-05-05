// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Linq;
using ArchRuleDemo.ArchitecturalModel;
using YADA.Core.Analyser;
using YADA.Core.DependencyRuleEngine;

namespace ArchRuleDemo.ArchRuleExampleDependencyRuleEngine
{
    public class ArchRuleExampleRuleEngineMapper : IDependencyRuleInputMapper<ArchRuleExampleType, ArchRuleExampleDependency>
    {
        private readonly IArchRuleExampleTypeRepository m_TypeMapper;

        public ArchRuleExampleRuleEngineMapper(IArchRuleExampleTypeRepository typeMapper)
        {
            m_TypeMapper = typeMapper;
        }

        public ArchRuleExampleDependency MapDependency(IDependency dependency)
        {
            var dependencyType = m_TypeMapper.GetTypeRepresentation(dependency.Type.FullName);
            return new ArchRuleExampleDependency(dependencyType, dependency.Contexts.Select(t => t.ToString()).ToList());
        }

        public ArchRuleExampleType MapTypeDescription(ITypeDescription type)
        {
            return m_TypeMapper.GetTypeRepresentation(type.FullName);
        }
    }
}