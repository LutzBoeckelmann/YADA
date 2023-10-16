// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Collections.Generic;

namespace YADA.Analyzer
{
    /// <summary>
    /// A simple fake implementation of IDependency for testability purposes. Can be used test your rules without reading real examples.
    /// </summary>
    public class DependencyFake : IDependency
    {
        public DependencyFake(ITypeDescription type, int occurrence)
        {
            Type = type;
            Occurrence = occurrence;
        }
        public ITypeDescription Type { get; }

        public int Occurrence { get; }

        public IEnumerable<IDependencyContext> Contexts { get { yield break; } }
    }
}