// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

namespace YADA.Core.Analyser
{
    internal class MethodBodyCalledMethodReturnTypeContext : IDependencyContext
    {
        private readonly string m_MethodName;
        private readonly string m_CalledMethodFullName;

        public MethodBodyCalledMethodReturnTypeContext(string name, string calledMethodFullName) 
        {
            m_MethodName = name;
            m_CalledMethodFullName = calledMethodFullName;
        }

        public void Visit(IDependencyContextVisitor visitor)
        {
            visitor.MethodBodyCalledMethodReturnType(m_MethodName, m_CalledMethodFullName);
        }

        public T Visit<T>(IDependencyContextVisitor<T> visitor) 
        {
            return visitor.MethodBodyCalledMethodReturnType(m_MethodName, m_CalledMethodFullName);
        }
    }
}
