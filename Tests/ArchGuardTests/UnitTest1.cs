// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Collections.Generic;
using System.IO;
using System.Linq;
using ArchGuardReader;
using NUnit.Framework;
using YADA.Analyzer;
using YADA.ArchGuard;
using YADA.ArchGuard.Behavior.Impl;
using YADA.ArchGuard.BuildingBlock;
using YADA.ArchGuard.BuildingBlock.Impl;
using YADA.ArchGuard.Feedback.Impl;

namespace ArchMatcherTests
{
    public class TestDataProvider
    {
        public IBuildingBlock GetTestData2()
        {/*
            Layers: (Domain, Core, Infra)
                        Domain 
                            restricted 
                           Domain1 ExtCore2 (may access core2 but only via impl, make it sense only to access core2, would this means )

                        Core 
                         2 Layers
                            Core3

                            restricted
                           Core1 Core2 

                        Infrastructure
                            - two restricted
                           Sub1 Sub2

                        */

            BuildingBlock project = new BuildingBlock(null, new BuildingBlockDescription("root", new BuildingBlockTypeFilter(".*(Domain|Core|Infrastructure).*")), new BuildingBlockBehavior( new string[] { "Layer"}));
            var domain = project.AddChild(new BuildingBlockDescription("DomainLayer", new BuildingBlockTypeFilter(".*Domain.*")), new BuildingBlockBehavior(new string[] { "Restricted" }));
            domain.AddChild(new BuildingBlockDescription("Domain1", new BuildingBlockTypeFilter(".*Domain1.*")), new BuildingBlockBehavior(new string[] { "Open" }));
            domain.AddChild(new BuildingBlockDescription("ExtCore2", new BuildingBlockTypeFilter(".*ExtCore2.*")), new BuildingBlockBehavior(new string[] { "Open" }, new string[] { "Extension" }));

            var coreLayerBox = project.AddChild(new BuildingBlockDescription("CoreLayer", new BuildingBlockTypeFilter(".*(Core3|Core2|Core1).*")), new BuildingBlockBehavior(new string[] { "Layer" })); // unclear the structure is smelly
            coreLayerBox.AddChild(new BuildingBlockDescription("Core3", new BuildingBlockTypeFilter(".*Core3.*")), new BuildingBlockBehavior(new string[] { "Restricted" }));
            var lowerCoreLayer = coreLayerBox.AddChild(new BuildingBlockDescription("Core21", new BuildingBlockTypeFilter(".*(Core2|Core1).*")), new BuildingBlockBehavior(new string[] { "Open" }));
            lowerCoreLayer.AddChild(new BuildingBlockDescription("Core1", new BuildingBlockTypeFilter(".*Core1.*")), new BuildingBlockBehavior(new string[] { "Open" }));
            lowerCoreLayer.AddChild(new BuildingBlockDescription("Core2", new BuildingBlockTypeFilter(".*Core2.*")), new BuildingBlockBehavior(new string[] { "Open" }, new string[] { "Private" }));
            var infra = project.AddChild(new BuildingBlockDescription("InfrastructureLayer", new BuildingBlockTypeFilter(".*Infrastructure.*")), new BuildingBlockBehavior(new string[] { "Restricted" }));

            var infraSub1 = infra.AddChild(new BuildingBlockDescription("Sub1", new BuildingBlockTypeFilter(".*Infrastructure.Sub1.*")), new BuildingBlockBehavior(new string[] { "Restricted" }));
            var infraSub2 = infra.AddChild(new BuildingBlockDescription("Sub2", new BuildingBlockTypeFilter(".*Infrastructure.Sub2.*")), new BuildingBlockBehavior());


            return project;
        }
    }

    [TestFixture]
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }
        /*
      Layers: (Domain, Core, Infra)
                  Domain 
                      restricted 
                     Domain1 ExtCore2 (may access core2 but only via impl, make it sense only to access core2, would this means )

                  Core 
                   2 Layers
                      Core3

                      restricted
                     Core1 Core2 

                  Infrastructure
                      - two restricted
                     Sub1 Sub2
        
            root .*(Domain|CoreLayer|Infrastructure).*
                DomainLayer .*DomainFlow.*
                    Domain1 .*Domain1.*
                    ExtCore2 .*ExtCore2.*

            CoreLayer .*(Core3|Core2|Core1).*
            Core3 .*Core3.*
            Core21 .*(Core2|Core1).*
            Core1 .*Core1.*
            Core2 .*Core2.*
            InfrastructureLayer .*Infrastructure.*
            
            InfrastructureLayer .*Infrastructure.Sub1*
            InfrastructureLayer .*Infrastructure.Sub2*
                  */

        [Test]
        public void Test2()
        {
            var testProject = GetTestData("TestProject.yaml");
            ITypeDescription core1 = new TypeDescriptionFake("CoreLayer.Core1", "Blub");
            ITypeDescription type3 = new TypeDescriptionFake("CoreLayer.Core2", "Blub");
            var typeMatcher = new TypeMatcher(testProject);

            var result = typeMatcher.GetConnectingChain(core1, type3).Select(i => i.Description.Name);

            Assert.That(result, Is.EquivalentTo(new[] { "Core1", "Core21", "Core2" }));
        }



        [Test]
        public void Test1()
        {
            var testProject = GetTestData("TestProject.yaml");
            ITypeDescription domain1 = new TypeDescriptionFake("Domain.Domain1", "Blub");

            ITypeDescription core1 = new TypeDescriptionFake("CoreLayer.Core1", "Blub");
            ITypeDescription core2 = new TypeDescriptionFake("CoreLayer.Core2", "Blub");

            ITypeDescription infraSub2 = new TypeDescriptionFake("Infrastructure.Sub2", "Blub");
            ITypeDescription infraSub1 = new TypeDescriptionFake("Infrastructure.Sub1", "Blub");
            ITypeDescription infraSub11 = new TypeDescriptionFake("Infrastructure.Sub1.2", "Blub");

            var typeChain = testProject.GetChain(domain1);

            Assert.That(typeChain.Select(i => i.Description.Name), Is.EquivalentTo(new[] { "Domain1", "DomainLayer", "root" }));

            var type2Chain = testProject.GetChain(infraSub2);
            Assert.That(type2Chain.Select(i => i.Description.Name), Is.EquivalentTo(new[] { "Sub2", "InfrastructureLayer", "root" }));

            var type3Chain = testProject.GetChain(core2);
            Assert.That(type3Chain.Select(i => i.Description.Name), Is.EquivalentTo(new[] { "Core2", "Core21", "CoreLayer", "root" }));
            Assert.IsNotNull(testProject);

            var typeMatcher = new TypeMatcher(testProject);
            var connect = typeMatcher.GetConnectingChain(domain1, infraSub2);
            var dependencyChecker = new DependencyChecker(typeMatcher);
            List<string> messages = new List<string>();
            FeedbackCollector feedback = new FeedbackCollector();
            
            var result1 = dependencyChecker.Check(domain1, infraSub2, feedback);
            Assert.IsTrue(result1);
            
            var result2 = dependencyChecker.Check(infraSub2, infraSub1, feedback);
            Assert.IsFalse(result2);

            // infra1 <- infra2 isolated parent
            Assert.IsFalse(dependencyChecker.Check(infraSub1, infraSub2, feedback));

            //core1 <- type1
            var r4 = dependencyChecker.Check(core1, core2, feedback);

            Assert.IsTrue(r4);

            var r6 = dependencyChecker.Check(infraSub1, domain1, feedback);
            Assert.IsFalse(r6);

            feedback.Clean();
            var r5 = dependencyChecker.Check(domain1, core2, feedback);

            Assert.IsFalse(r5, "private");
            Assert.That(feedback.CollectedFeedback, Is.Not.Empty);

            var matcher = new TypeMatcher(testProject);
            var factory = new ConcreteArchitectureFactory(matcher);
            var architecture = factory.GetArchitecture(new[] { domain1, core1, core2, infraSub2, infraSub1, infraSub11 });

            Assert.NotNull(architecture);
            Assert.That(architecture, Has.Count.EqualTo(5));

            //Assert.IsFalse(architecture.Valid);
        }

        [Test]
        public void Test3()
        {
            var testProject = GetTestData("TestProject.yaml");
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


        private BuildingBlock GetTestData(string projectFile)
        {
            var reader = new YamlProject();
            var yamlProject = reader.ReadProject(Path.Combine(TestContext.CurrentContext.TestDirectory, "_Projects", projectFile));
            var project = BuildingBlockFactory.Get(yamlProject.Create());
         
            return (BuildingBlock)project;
        }

        private BuildingBlock GetSmallProject2()
        {
            BuildingBlock project = new BuildingBlock(null, new BuildingBlockDescription("root", new BuildingBlockTypeFilter(".*(Domain|Core).*")), new BuildingBlockBehavior(new string[] { "Layer" }));
            var domain = project.AddChild(new BuildingBlockDescription("DomainLayer", new BuildingBlockTypeFilter(".*Domain.*")), new BuildingBlockBehavior(new string[] { "Open" }));
            domain.AddChild(new BuildingBlockDescription("Domain1", new BuildingBlockTypeFilter(".*Domain1.*")), new BuildingBlockBehavior(new string[] { "Open" }, new string[] { "Private" }));
            var coreLayer = project.AddChild(new BuildingBlockDescription("CoreLayer", new BuildingBlockTypeFilter(".*Core.*")), new BuildingBlockBehavior(new string[] { "Open" })); // unclear the structure is smelly
            coreLayer.AddChild(new BuildingBlockDescription("Core1", new BuildingBlockTypeFilter(".*Core1.*")), new BuildingBlockBehavior(new string[] { "Open" }));

            return project;
        }

     
        [Test]
        public void ReaderTest()
        {
            var sut = new YamlProject(); 
            var dto = sut.ReadProject(Path.Combine(TestContext.CurrentContext.TestDirectory, "_Projects", "TestProject.yaml"));
            
            Assert.NotNull(dto);            
        }

        [Test]
        public void WriteTest()
        {
            var sut = new YamlProject();

            var dto = new YamlBuildingBlockDTO()
            {
                name = "project",
                filter = new List<string>() { ".*(Domain|Core).*" },
                behavior = new YamlBehaviorDTO()
                {
                    Container = new List<string>() { "Public" },
                    Internal = new List<string>() { "Layer" }
                },
                children = new List<YamlBuildingBlockDTO>()
                {
                    new YamlBuildingBlockDTO() {
                        name = "DomainLayer",
                        filter = new List<string>() { ".*Domain.*" },
                        behavior = new YamlBehaviorDTO()
                        {
                            Container = new List<string>() { "Public" },
                            Internal = new List<string>() { "Restricted" }
                        },
                        children = new List<YamlBuildingBlockDTO>
                        {
                            new YamlBuildingBlockDTO(){
                                name = "Domain1",
                                filter = new List<string> () {".*Domain.DomainSubsystem.*" },
                                behavior = new YamlBehaviorDTO
                                {
                                    Container = new List<string>() { "Open" },
                                    Internal = new List<string>() { "Public" }
                                }
                            },

                            new YamlBuildingBlockDTO {
                                name = "ExtCore1",
                                filter = new List<string> { ".*Domain.SubsystemA.*" },
                                behavior = new YamlBehaviorDTO
                                {
                                    Container = new List<string>() { "Open", "Extension" },
                                    Internal = new List<string>() { "Public" }
                                }
                            }
                        }
                    }

                }
            };

            var input = dto.Create();
            var yamlProject = sut.SerializeProject(input);
                        
            Assert.NotNull(dto);
        }


        [Test]
        public void RoundTrip()
        {
            var sut = new YamlProject();

            var yaml = File.ReadAllText(Path.Combine(TestContext.CurrentContext.TestDirectory, "_Projects", "TestProject.yaml"));
            var input = sut.DeserializeProject(yaml).Create();
            
            var root = BuildingBlockFactory.Get(input);
            var output = sut.Get((BuildingBlock)root);
            
            var serialized = sut.SerializeProject(output);
            
            Assert.AreEqual(yaml, serialized);

        }

        [Test]
        public void HandCraftedProject_ExportedYamlProject_AreEqual()
        {
            var project = GetTestData("SmallProject.yaml");
            var p2 = GetSmallProject2();

            var result = BuildingBlockComparer.Compare(project, p2);
            Assert.True(result);
        }


        [Test]
        public void Test4()
        {
            var project = GetTestData("SmallProject.yaml");
            

            var domain = new TypeDescriptionFake("Domain.Domain1", "Blub");
            var core = new TypeDescriptionFake("CoreLayer.Core1", "Blub");

            core.Add(domain);

            var coreChain = project.GetChain(core);
            var domainChain = project.GetChain(domain);


            var matcher = new TypeMatcher(project);
            var connectingChain = matcher.GetConnectingChain(core, domain);

            var factory = new ConcreteArchitectureFactory(matcher);
            var architecture = factory.GetArchitecture(new[] { core });

            Assert.NotNull(architecture);
            Assert.That(architecture.Feedback.CollectedFeedback.Count, Is.EqualTo(2));
        }
    }
}