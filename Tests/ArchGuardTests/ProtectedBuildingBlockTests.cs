// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Collections.Generic;
using NUnit.Framework;
using YADA.Analyzer;
using YADA.ArchGuard;
using YADA.ArchGuard.Behavior.Impl;
using YADA.ArchGuard.BuildingBlock;
using YADA.ArchGuard.BuildingBlock.Impl;
using YADA.ArchGuard.Feedback.Impl;

namespace ArchGuardTests
{
    public class TestDataProvider
    {
        public class FilterBuilder
        {
            List<string> m_Content = new List<string>();

            public FilterBuilder Or()
            {
                m_Content.Add("OR");
                return this;
            }

            public FilterBuilder And()
            {
                m_Content.Add("AND");
                return this;
            }

            public FilterBuilder Not()
            {
                m_Content.Add("NOT");
                return this;
            }

            public FilterBuilder Regex(string pattern)
            {
                m_Content.Add(pattern);
                return this;
            }

            public IReadOnlyCollection<string> Create()
            {
                return m_Content;
            }
        }

        public IBuildingBlock GetTestData()
        {/*
            Project.Domain.DomainSubsystem.SomeType
            Project.Domain.SubsystemAExtension.ExtensionType

            Project.Core.SubsystemA.Impl.Class
            Project.Core.SubsystemA.Impl.InteralClass
            Project.Core.SubsystemA.IClass
            
            Project:
                Layers: (Domain, Core, Infra)
                        Domain 
                            
                            Domain1
                                Domain.DomainSubsystem.Type
                            SubsystemAExtension (may access core2 but only via impl, make it sense only to access core2, would this means )
                                Domain.SubsystemA
                        Core 
                            SubsystemA
                                2 Layers
                                    Impl
                                        private Impl
                                            
                                            Core.SubsystemA.Impl.Class
                                            
                                        protected Impl
                                            Core.SubsystemA.Impl.InteralClass

                                    Public
                                        Core.SubsystemA
                        */

            BuildingBlock project = new BuildingBlock(null, new BuildingBlockDescription("root", new BuildingBlockTypeFilter(".*(Domain|Core).*"), true), new BuildingBlockBehavior(new string[] { "Layer" }));
            
            var domain = project.AddChild(new BuildingBlockDescription("DomainLayer", new BuildingBlockTypeFilter(".*Domain.*"), true), new BuildingBlockBehavior(new string[] { "Restricted" }));

            domain.AddChild(new BuildingBlockDescription("Domain1", new BuildingBlockTypeFilter(".*Domain.DomainSubsystem.*"), false), new BuildingBlockBehavior(new string[] { "Open" }));
            domain.AddChild(new BuildingBlockDescription("ExtCore1", new BuildingBlockTypeFilter(".*Domain.SubsystemA.*"), false), new BuildingBlockBehavior(new string[] { "Open" }, new string[] { "Extension" }));

            var coreLayerBox = project.AddChild(new BuildingBlockDescription("CoreLayer", new BuildingBlockTypeFilter(".*Core.*"), true), new BuildingBlockBehavior(new string[] { "Layer" })); // unclear the structure is smelly
            var sub  systemA = coreLayerBox.AddChild(new BuildingBlockDescription("Core.SubsystemA", new BuildingBlockTypeFilter(".*Core.SubsystemA*"), false), new BuildingBlockBehavior(new string[] { "Layer" }));
            
            var SubsystemAImpl = subsystemA.AddChild(new BuildingBlockDescription("Core.SubsystemA.Impl", new BuildingBlockTypeFilter(".*Core.SubsystemA.Impl.*"), false), new BuildingBlockBehavior(new string[] { "Private" }));
            
            subsystemA.AddChild(new BuildingBlockDescription("Core.SubsystemA.Public", new BuildingBlockTypeFilter(new[] { ".*Core.SubsystemA.Impl.*", "AND", "NOT", ".* Core.SubsystemA" }), false), new BuildingBlockBehavior(new string[] { "Open" }));


            return project;
        }
    }

    [TestFixture]
    public class ProtectedBuildingBlockTests
    {
        [Test]
        public void Test()
        {
            var domain1 = new TypeDescriptionFake("Domain.Domain1", "Blub");


            var testData = new TestDataProvider();
            var matcher = new TypeMatcher(testData);
            var factory = new ConcreteArchitectureFactory(matcher);

            var architecture = factory.GetArchitecture(new[] { domain1, core1, core2, infraSub2, infraSub1, infraSub11 });
        }
    }

    [Test]
    public void Test3()
    {
        var sut = new TestDataProvider();

        var testProject = sut.GetTestData() as BuildingBlock;
        var domain1 = new TypeDescriptionFake("Domain.Domain1", "Blub");

        var core1 = new TypeDescriptionFake("CoreLayer.Core1", "Blub");
        var core2 = new TypeDescriptionFake("CoreLayer.Core2", "Blub");

        var infraSub2 = new TypeDescriptionFake("Infrastructure.Sub2", "Blub");
        var infraSub1 = new TypeDescriptionFake("Infrastructure.Sub1", "Blub");
        var infraSub11 = new TypeDescriptionFake("Infrastructure.Sub1.2", "Blub");

        domain1.Add(infraSub2);

        infraSub2.Add(infraSub1);


        infraSub1.Add(infraSub2);
        core1.Add(core2);

        domain1.Add(core2);

        var matcher = new TypeMatcher(testProject);
        var factory = new ConcreteArchitectureFactory(matcher);
        var architecture = factory.GetArchitecture(new[] { domain1, core1, core2, infraSub2, infraSub1, infraSub11 });

        Assert.NotNull(architecture);
        Assert.That(architecture, Has.Count.EqualTo(5));

        Assert.IsFalse(architecture.Valid);
    }

}
