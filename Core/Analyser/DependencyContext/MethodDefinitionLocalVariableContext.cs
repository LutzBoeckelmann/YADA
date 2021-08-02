// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

namespace YADA.Core.Analyser
{
    internal class MethodDefinitionLocalVariableContext : IDependencyContext
    {
        private readonly string m_Name;
        public MethodDefinitionLocalVariableContext(string name)   
        {
            m_Name = name;
        }
        
        public void Visit(IDependencyContextVisitor visitor)
        {
            visitor.MethodDefinitionLocalVariable(m_Name);
        }

        public T Visit<T>(IDependencyContextVisitor<T> visitor) 
        {
            return visitor.MethodDefinitionLocalVariable(m_Name);
        }
    }
}
