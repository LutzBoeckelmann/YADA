// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

namespace YADA.Core.DependencyRuleEngine
{
    public enum DependencyRuleResult
    {
        Approve,
        
        /// <summary>
        /// The rule does not apply to the checked value. Ignored will not affect the judgement
        /// </summary>
        Ignored,
        Reject,
        /// <summary>
        /// The 
        /// </summary>
        Skip
    }
}

