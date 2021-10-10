// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

namespace YADA.Core.Analyser
{
    /// <summary>
    /// An abstract base class for any Method based dependency context
    /// </summary>
    public abstract class DependencyContextBase : IDependencyContext
    {
        private readonly string m_Name;

        protected string Name => m_Name;
        
        protected DependencyContextBase(string name)
        {
            m_Name = name;
        }

        public abstract void Visit(IDependencyContextVisitor visitor);
        public abstract T Visit<T>(IDependencyContextVisitor<T> visitor);
    }
}