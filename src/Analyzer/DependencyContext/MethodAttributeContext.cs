// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

namespace YADA.Analyzer
{
    internal class MethodAttributeContext : DependencyContextBase
    {
        public MethodAttributeContext(string methodName) : base(methodName)
        {

        }

        public override void Visit(IDependencyContextVisitor visitor)
        {
            visitor.MethodAttributeContext(Name);
        }

        public override T Visit<T>(IDependencyContextVisitor<T> visitor)
        {
            return visitor.MethodAttributeContext(Name);
        }
    }
}