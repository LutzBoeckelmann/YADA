// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

namespace YADA.Analyzer
{
    internal class MethodDefinitionReturnTypeContext : DependencyContextBase
    {
        public MethodDefinitionReturnTypeContext(string name)   : base (name) { }

        public override void Visit(IDependencyContextVisitor visitor)
        {
            visitor.MethodDefinitionReturnType(Name);
        }

        public override T Visit<T>(IDependencyContextVisitor<T> visitor) 
        {
            return visitor.MethodDefinitionReturnType(Name);
        }
    }
}
