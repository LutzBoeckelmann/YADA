// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Collections.Generic;

namespace YADA.Core.Analyser
{
    public interface IDependency 
    {
        
        ITypeDescription Type { get; }
        int Occurrence { get; }

        IEnumerable<IDependencyContext> Contexts { get; }
    }
}
