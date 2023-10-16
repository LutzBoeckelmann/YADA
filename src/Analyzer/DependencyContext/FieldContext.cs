// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

namespace YADA.Analyzer
{
    internal class FieldContext : DependencyContextBase
    {
        public FieldContext(string fieldName) : base(fieldName) { }
        public override void Visit(IDependencyContextVisitor visitor)
        {
            visitor.FieldDefinition(Name);
        }

        public override T Visit<T>(IDependencyContextVisitor<T> visitor) 
        {
            return visitor.FieldDefinition(Name);
        }
    }
}
