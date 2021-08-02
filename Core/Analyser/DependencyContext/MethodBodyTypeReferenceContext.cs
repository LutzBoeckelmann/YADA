// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

namespace YADA.Core.Analyser
{
    internal class MethodBodyTypeReferenceContext : IDependencyContext
    {
        private readonly string m_MethodName;
        public MethodBodyTypeReferenceContext(string name) {
            m_MethodName = name;
        }

        public  void Visit(IDependencyContextVisitor visitor)
        {
            visitor.MethodBodyReferencedType(m_MethodName);
        }

        public T Visit<T>(IDependencyContextVisitor<T> visitor) 
        {
            return visitor.MethodBodyReferencedType(m_MethodName);
        }
    }
}
