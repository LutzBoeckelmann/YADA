// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

namespace YADA.Core.Analyser
{
    internal class MethodDefinitionParameterContext : MethodContextBase
    {
        public MethodDefinitionParameterContext(string name) : base(name) { }

        public override void Visit(IDependencyContextVisitor visitor)
        {
            visitor.MethodDefinitionParameter(MethodName);
        }

        public override T Visit<T>(IDependencyContextVisitor<T> visitor) 
        {
            return visitor.MethodDefinitionParameter(MethodName);
        }
    }
}
