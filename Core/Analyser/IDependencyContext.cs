// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

namespace YADA.Core.Analyser
{
    public interface IDependencyContext 
    {
        void Visit(IDependencyContextVisitor visitor);
        T Visit<T>(IDependencyContextVisitor<T> visitor);
    }
}
