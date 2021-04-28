// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

namespace YADA.Core.DependencyRuleEngine
{
    public interface ITypeFeedback 
    {
        IRuleFeedback ViolatesRule(string nameOfRule);
    }
}

