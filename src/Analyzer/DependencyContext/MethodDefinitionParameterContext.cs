// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

namespace YADA.Analyzer
{
    internal class MethodDefinitionParameterContext : DependencyContextBase
    {
        public MethodDefinitionParameterContext(string name) : base(name) { }

        public override void Visit(IDependencyContextVisitor visitor)
        {
            visitor.MethodDefinitionParameter(Name);
        }

        public override T Visit<T>(IDependencyContextVisitor<T> visitor) 
        {
            return visitor.MethodDefinitionParameter(Name);
        }
    }
}
