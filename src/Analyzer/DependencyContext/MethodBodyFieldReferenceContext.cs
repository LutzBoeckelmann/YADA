// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

namespace YADA.Analyzer
{
    internal class MethodBodyFieldReferenceContext : DependencyContextBase 
    {
        private readonly string m_FieldName;
        
        public MethodBodyFieldReferenceContext(string methodName, string fieldName) : base(methodName)
        {
            m_FieldName = fieldName;
        }

        public override void Visit(IDependencyContextVisitor visitor)
        {
            visitor.MethodBodyAccessedFieldType(Name, m_FieldName);
        }

        public override T Visit<T>(IDependencyContextVisitor<T> visitor) 
        {
            return visitor.MethodBodyAccessedFieldType(Name, m_FieldName);
        }
    }
}
