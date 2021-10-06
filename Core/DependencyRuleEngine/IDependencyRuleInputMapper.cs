// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt
using YADA.Core.Analyser;

namespace YADA.Core.DependencyRuleEngine
{
    // [KA] XML comments missing
    public interface IDependencyRuleInputMapper<T, K>
    {
        K MapDependency(IDependency dependency);

        T MapTypeDescription(ITypeDescription type);
    }
}

