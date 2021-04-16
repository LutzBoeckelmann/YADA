// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using Moq;
using YADA.Core;

namespace YADA.Test
{
    public static class MoqProvider 
    {
        public static ITypeDescription GetTypeDescriptionMoq(string fullName) 
        {
            var type = new Mock<ITypeDescription>();
            type.Setup(t => t.FullName).Returns(fullName);

            return type.Object;
        }

        public static IDependency GetDependencyMoq(ITypeDescription type) 
        {
            var dependency = new Mock<IDependency>();
            dependency.Setup(d => d.Type).Returns(type);
            return dependency.Object;
        }

    }
}