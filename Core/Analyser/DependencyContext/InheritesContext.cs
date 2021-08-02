// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

namespace YADA.Core.Analyser
{
    internal class InheritesContext : IDependencyContext
    {
        private readonly string m_Name;

        public InheritesContext(string dependencyName) 
        {   
            m_Name = dependencyName;
        }
        public  void Visit(IDependencyContextVisitor visitor)
        {
            visitor.BaseClassDefinition(m_Name);
        }

        public T Visit<T>(IDependencyContextVisitor<T> visitor) 
        {
            return visitor.BaseClassDefinition(m_Name);
        }
    }
}
