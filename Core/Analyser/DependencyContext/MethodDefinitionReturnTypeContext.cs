// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

namespace YADA.Core.Analyser
{
    internal class MethodDefinitionReturnTypeContext : IDependencyContext
    {
        private readonly string m_Name;
        public MethodDefinitionReturnTypeContext(string name)   
        {
            m_Name = name;
        }

        public void Visit(IDependencyContextVisitor visitor)
        {
            visitor.MethodDefinitionReturnType(m_Name);
        }

        public T Visit<T>(IDependencyContextVisitor<T> visitor) 
        {
            return visitor.MethodDefinitionReturnType(m_Name);
        }
    }
}
