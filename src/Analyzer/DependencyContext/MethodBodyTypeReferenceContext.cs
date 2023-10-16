// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

namespace YADA.Analyzer
{
    internal class MethodBodyTypeReferenceContext : DependencyContextBase
    {
        public MethodBodyTypeReferenceContext(string name) : base (name) {  }

        public override void Visit(IDependencyContextVisitor visitor)
        {
            visitor.MethodBodyReferencedType(Name);
        }

        public override T Visit<T>(IDependencyContextVisitor<T> visitor) 
        {
            return visitor.MethodBodyReferencedType(Name);
        }
    }
}
