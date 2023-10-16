// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

namespace YADA.Analyzer
{
    internal class MethodDefinitionLocalVariableContext : DependencyContextBase
    {
        
        public MethodDefinitionLocalVariableContext(string name)   : base(name) {   }
        
        public override void Visit(IDependencyContextVisitor visitor)
        {
            visitor.MethodDefinitionLocalVariable(Name);
        }

        public override T Visit<T>(IDependencyContextVisitor<T> visitor) 
        {
            return visitor.MethodDefinitionLocalVariable(Name);
        }
    }
}
