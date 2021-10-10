// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

namespace YADA.Core.Analyser
{
    internal class MethodBodyCalledMethodParameterContext : MethodContextBase
    {
        private readonly string m_CalledMethodFullName;

        public MethodBodyCalledMethodParameterContext(string name, string calledMethodFullName) : base(name)
        {
            m_CalledMethodFullName = calledMethodFullName;
        }

        public override void Visit(IDependencyContextVisitor visitor)
        {
            visitor.MethodBodyCalledMethodParameter(MethodName, m_CalledMethodFullName);
        }

        public override T Visit<T>(IDependencyContextVisitor<T> visitor) 
        {
            return visitor.MethodBodyCalledMethodParameter(MethodName, m_CalledMethodFullName);
        }
    }
}
