// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Collections.Generic;

namespace YADA.Core.Analyser
{
    public interface ITypeDescription
    {
        string AssemblyName { get; }
        string FullName { get; }

        IEnumerable<IDependency> Dependencies { get; }

    }
}
