// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

namespace YADA.Analyzer
{
    internal class InheritesContext : DependencyContextBase
    {
        public InheritesContext(string dependencyName) : base(dependencyName) { }
        public override void Visit(IDependencyContextVisitor visitor)
        {
            visitor.BaseClassDefinition(Name);
        }

        public override T Visit<T>(IDependencyContextVisitor<T> visitor) 
        {
            return visitor.BaseClassDefinition(Name);
        }
    }
}
