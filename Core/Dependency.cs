// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

namespace YADA.Core
{
    public class Dependency : IDependency
    {
        public Dependency(TypeDescription type) 
        {
            Type = type;
            Occurrence = 0;
        }

        public ITypeDescription Type { get; }
        public int Occurrence { get; private set; }

        public void IncrementUsage() { Occurrence++; }
    }
}
