// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Collections.Generic;

namespace YADA.Analyzer
{
    /// <summary>
    /// Represents a type found by the TypeLoader.
    /// </summary>
    public interface ITypeDescription
    {
        /// <summary>
        /// Fullqualified name of the Assembly the type belongs too.
        /// </summary>
        string AssemblyName { get; }

        /// <summary>
        /// Fullqualified name of the type.
        /// </summary>
        /// <value></value>
        string FullName { get; }

        /// <summary>
        /// Any dependencies of the type.
        /// </summary>
        IEnumerable<IDependency> Dependencies { get; }
    }
}
