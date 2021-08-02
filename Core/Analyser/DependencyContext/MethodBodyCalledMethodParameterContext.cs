// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

namespace YADA.Core.Analyser
{
    internal class MethodBodyCalledMethodParameterContext : IDependencyContext
    {
        private readonly string m_MethodName;
        private readonly string m_CalledMethodFullName;

        public MethodBodyCalledMethodParameterContext(string name, string calledMethodFullName)
        {
            m_MethodName = name;
            m_CalledMethodFullName = calledMethodFullName;
        }

        public void Visit(IDependencyContextVisitor visitor)
        {
            visitor.MethodBodyCalledMethodParameter(m_MethodName, m_CalledMethodFullName);
        }

        public T Visit<T>(IDependencyContextVisitor<T> visitor) 
        {
            return visitor.MethodBodyCalledMethodParameter(m_MethodName, m_CalledMethodFullName);
        }
    }
}
