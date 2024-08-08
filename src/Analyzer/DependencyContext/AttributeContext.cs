// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

namespace YADA.Analyzer
{
    internal class AttributeContext : DependencyContextBase
    {
        public AttributeContext() : base("") { }
        
        public override void Visit(IDependencyContextVisitor visitor)
        {
            visitor.ClassAttributeContext();
        }

        public override T Visit<T>(IDependencyContextVisitor<T> visitor)
        {
            return visitor.ClassAttributeContext();
        }

    }
}