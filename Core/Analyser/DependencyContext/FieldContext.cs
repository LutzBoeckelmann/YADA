// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

namespace YADA.Core.Analyser
{
    internal class FieldContext : IDependencyContext
    {
        private readonly string m_Name;
        public FieldContext(string fieldName) 
        {   
            m_Name = fieldName;
        }
        public void Visit(IDependencyContextVisitor visitor)
        {
            visitor.FieldDefinition(m_Name);
        }

        public T Visit<T>(IDependencyContextVisitor<T> visitor) 
        {
            return visitor.FieldDefinition(m_Name);
        }
    }
}
