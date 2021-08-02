// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

namespace YADA.Core.Analyser
{
    internal class MethodDefinitionParameterContext : IDependencyContext
    {
        private readonly string m_Name;

        public MethodDefinitionParameterContext(string name) 
        {
            m_Name = name;
        }

        public void Visit(IDependencyContextVisitor visitor)
        {
            visitor.MethodDefinitionParameter(m_Name);
        }

        public T Visit<T>(IDependencyContextVisitor<T> visitor) 
        {
            return visitor.MethodDefinitionParameter(m_Name);
        }
    }
}
