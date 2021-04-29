// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt
using YADA.Core.Analyser;

namespace YADA.Core.DependencyRuleEngine
{
    public interface IDependencyRuleInputMapper<T, K>
    {
        K MapDependency(IDependency dependency);

        T MapTypeDescription(ITypeDescription type);
    }
}

