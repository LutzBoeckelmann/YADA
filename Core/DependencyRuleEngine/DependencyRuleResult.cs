// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

namespace YADA.Core.DependencyRuleEngine
{
    public enum DependencyRuleResult
    {
        /// <summary>
        /// The rule approves the type or dependency. To be successful at least one rule must be 
        /// approve and no rule my reject.
        /// </summary>
        Approve,
        /// <summary>
        /// The rule does not apply to the checked value. Ignored will not affect the judgement
        /// </summary>
        Ignored,
        /// <summary>
        /// The rule rejects the result. This means the analyse fails. In case of a type the type rule the dependencies of 
        /// the type will not be analysed.
        /// </summary>
        Reject,
        /// <summary>
        /// The current type or dependency should not be further processed.
        /// </summary>
        Skip
    }
}

