// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

namespace YADA.Core.Analyser
{
    internal class MethodDefinitionReturnTypeContext : MethodContextBase
    {
        public MethodDefinitionReturnTypeContext(string name)   : base (name) { }

        public override void Visit(IDependencyContextVisitor visitor)
        {
            visitor.MethodDefinitionReturnType(MethodName);
        }

        public override T Visit<T>(IDependencyContextVisitor<T> visitor) 
        {
            return visitor.MethodDefinitionReturnType(MethodName);
        }
    }
}
