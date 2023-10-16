// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

namespace YADA.Analyzer
{
    internal class MethodBodyCallMethodAtType : DependencyContextBase
    {
        private readonly string m_CalledMethodFullName;

        public MethodBodyCallMethodAtType(string name, string calledMethodFullName) : base (name)
        {
            m_CalledMethodFullName = calledMethodFullName;
        }

        public override void Visit(IDependencyContextVisitor visitor)
        {
            visitor.MethodBodyCallMethodAtType(Name, m_CalledMethodFullName);
        }

        public override T Visit<T>(IDependencyContextVisitor<T> visitor) 
        {
            return visitor.MethodBodyCallMethodAtType(Name, m_CalledMethodFullName);
        }
    }
}
