// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

namespace YADA.Core.Analyser
{
    internal class MethodDefinitionLocalVariableContext : MethodContextBase
    {
        
        public MethodDefinitionLocalVariableContext(string name)   : base(name) {   }
        
        public override void Visit(IDependencyContextVisitor visitor)
        {
            visitor.MethodDefinitionLocalVariable(MethodName);
        }

        public override T Visit<T>(IDependencyContextVisitor<T> visitor) 
        {
            return visitor.MethodDefinitionLocalVariable(MethodName);
        }
    }
}
