// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using YADA.Analyzer;

namespace YADA.DependencyRuleEngine
{
    internal class DefaultInputMapper : IDependencyRuleInputMapper<ITypeDescription, IDependency>
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
}

