// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Collections.Generic;

namespace YADA.Analyzer
{
    /// <summary>
    /// A dependency between types. Aggregated at the ITypeDescription which declares the dependency
    /// </summary>
    public interface IDependency 
    {
        /// <summary>
        /// The type of the dependency. 
        /// </summary>
        /// <value></value>
        ITypeDescription Type { get; }
        /// <summary>
        /// How often the dependency to the specific type was found in the type.
        /// More detailed are available at the Contexts enumeration.
        /// </summary>
        int Occurrence { get; }

        /// <summary>
        /// A set of IDependencyContext instances containing specific information
        /// about any occurrence where the dependency was found.
        /// </summary>
        IEnumerable<IDependencyContext> Contexts { get; }
    }
}
