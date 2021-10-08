// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt
using YADA.Core.Analyser;

namespace YADA.Core.DependencyRuleEngine
{
    /// <summary>
    /// The rule engine uses an instance of this interface to convert dependencies and type description
    /// into specific classes. This enables the use of specialized interfaces as input of for
    /// the rules. This makes it possible to express the architecture model in code and makes the rules
    /// testable without YADA interfaces and against the specific representations.
    /// </summary>
    /// <typeparam name="T">A adapter for ITypeDescription</typeparam>
    /// <typeparam name="K">A adapter for IDependency</typeparam>
    public interface IDependencyRuleInputMapper<T, K>
    {
        /// <summary>
        /// Creates an adapter for the given dependency
        /// </summary>
        /// <param name="dependency">The dependency to encapsulate</param>
        /// <returns>The adapter for the given dependency</returns>
        K MapDependency(IDependency dependency);

        /// <summary>
        /// Creates an adapter for the given typeDescription 
        /// </summary>
        /// <param name="type">The type to encapsulate</param>
        /// <returns>The adapter for the given type</returns>
        T MapTypeDescription(ITypeDescription type);
    }
}

