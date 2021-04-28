// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Collections.Generic;
using YADA.Core.Analyser;

namespace Core.DependencyRuleEngine
{
    public class DependencyMock : IDependency
    {
        public DependencyMock(ITypeDescription type, int occurrence)
        {
            Type = type;
            Occurrence = occurrence;
        }
        public ITypeDescription Type { get; }

        public int Occurrence { get; }

        public IEnumerable<IDependencyContext> Contexts {get { yield break; } }
    }
}