// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

namespace YADA.Core.Analyser
{
    /// <summary>
    /// An abstract base class for any Method based dependency context
    /// </summary>
    public abstract class MethodContextBase : IDependencyContext
    {
        private readonly string m_MethodName;

        protected string MethodName => m_MethodName;
        
        protected MethodContextBase(string methodName)
        {
            m_MethodName = methodName;
        }

        public abstract void Visit(IDependencyContextVisitor visitor);
        public abstract T Visit<T>(IDependencyContextVisitor<T> visitor);
    }
}