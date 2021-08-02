// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

namespace YADA.Core.Analyser
{
    internal class MethodBodyFieldReferenceContext : IDependencyContext 
    {
        private readonly string m_MethodName;
        private readonly string m_FieldName;
        
        public MethodBodyFieldReferenceContext(string methodName, string fieldName)
        {
            m_MethodName = methodName;
            m_FieldName = fieldName;
        }

        public void Visit(IDependencyContextVisitor visitor)
        {
            visitor.MethodBodyAccessedFieldType(m_MethodName, m_FieldName);
        }

        public T Visit<T>(IDependencyContextVisitor<T> visitor) 
        {
            return visitor.MethodBodyAccessedFieldType(m_MethodName, m_FieldName);
        }
    }
}
