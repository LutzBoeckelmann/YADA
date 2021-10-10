// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

namespace YADA.Core.Analyser
{
    internal class MethodBodyCalledMethodReturnTypeContext : MethodContextBase
    {
        private readonly string m_CalledMethodFullName;

        public MethodBodyCalledMethodReturnTypeContext(string name, string calledMethodFullName) : base(name)
        {
            m_CalledMethodFullName = calledMethodFullName;
        }

        public override void Visit(IDependencyContextVisitor visitor)
        {
            visitor.MethodBodyCalledMethodReturnType(MethodName, m_CalledMethodFullName);
        }

        public override T Visit<T>(IDependencyContextVisitor<T> visitor) 
        {
            return visitor.MethodBodyCalledMethodReturnType(MethodName, m_CalledMethodFullName);
        }
    }
}
