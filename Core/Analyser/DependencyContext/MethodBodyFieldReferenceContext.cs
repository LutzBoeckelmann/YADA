// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

namespace YADA.Core.Analyser
{
    internal class MethodBodyFieldReferenceContext : MethodContextBase 
    {
        private readonly string m_FieldName;
        
        public MethodBodyFieldReferenceContext(string methodName, string fieldName) : base(methodName)
        {
            m_FieldName = fieldName;
        }

        public override void Visit(IDependencyContextVisitor visitor)
        {
            visitor.MethodBodyAccessedFieldType(MethodName, m_FieldName);
        }

        public override T Visit<T>(IDependencyContextVisitor<T> visitor) 
        {
            return visitor.MethodBodyAccessedFieldType(MethodName, m_FieldName);
        }
    }
}
