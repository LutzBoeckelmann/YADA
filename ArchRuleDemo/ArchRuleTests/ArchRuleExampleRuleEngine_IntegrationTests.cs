// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using NUnit.Framework;
using YADA.Core.Analyser;
using ArchRuleDemo.ArchRuleExampleDependencyRuleEngine;

using ArchRuleDemo.ArchitecturalModel;
using ArchRuleDemo.ArchitecturalRules;
using YADA.Core.DependencyRuleEngine.Rules;
using YADA.Core.DependencyRuleEngine.Feedback;
using YADA.Core.DependencyRuleEngine;

namespace ArchRuleDemo.ArchRuleTests
{
    [TestFixture]
    public class ArchRuleExampleRuleEngine_IntegrationTests
    {
        //.DomainLayer.Module.TechnicalLayer.Non.Architectural.Stuff


        //TechnicalLayer : Data | BusinessLogic | UI
        //DomainLayer: Infrastructure | Core | Extentions
        //ArchRuleExample.Infrastructure.Module1.Data.SubComponentHelper

      
        [Test]
        public void Analyse_ValidTypes_True()
        {
            var sut = CreateSut();

            var typ1 = new TypeDescriptionFake("ArchRuleExample.Infrastructure.Module.Data.Class1");
            var typ2 = new TypeDescriptionFake("ArchRuleExample.Infrastructure.Module.Data.Class2");

            typ2.Add(typ1);

            var result = sut.Analyse(new ITypeDescription[] { typ1, typ2 }, new FeedbackCollector());

            Assert.That(result, Is.True);
        }

        [Test]
        public void Analyse_TypeWithLayerViolation_False()
        {
            var sut = CreateSut();

            var typ1 = new TypeDescriptionFake("ArchRuleExample.Core.Module.Data.Class1");

            var typ2 = new TypeDescriptionFake("ArchRuleExample.Infrastructure.Module.Data.Class2");

            typ2.Add(typ1);

            var result = sut.Analyse(new ITypeDescription[] { typ1, typ2 }, new FeedbackCollector());

            Assert.That(result, Is.False);
        }

        [Test]
        public void Analyse_SomeTypeWithLayerViolationCorrectAsLast_False()
        {
            var sut = CreateSut();

            var typ1 = new TypeDescriptionFake("ArchRuleExample.Core.Module.Data.Class1");
            var typ2 = new TypeDescriptionFake("ArchRuleExample.Infrastructure.Module.Data.Class2");
            typ2.Add(typ1);
            var typ3 = new TypeDescriptionFake("ArchRuleExample.Core.Module.Data.Class3");

            var result = sut.Analyse(new ITypeDescription[] { typ1, typ2, typ3 }, new FeedbackCollector());

            Assert.That(result, Is.False);
        } 
        
        private DependencyRuleEngine CreateSut()
        {
            var typeRepository = new ArchRuleExampleTypeRepository();
            var mapper = new ArchRuleExampleRuleEngineMapper(typeRepository);

            var typeRules = new ITypeRule<ITypeDescription>[]
            {
                new BaseTypeRule<ArchRuleExampleType, ArchRuleExampleDependency>( new CorrectNamespaceTypeRule(), mapper)
            };

            var dependencyRules = new IDependencyRule<ITypeDescription, IDependency>[]
            {
                  new BaseDependencyRule<ArchRuleExampleType, ArchRuleExampleDependency>(new CrossComponentAccessOnlyOnSameTechnicalLayerDependencyRule(), mapper),
                  new BaseDependencyRule<ArchRuleExampleType, ArchRuleExampleDependency>(new OnlyAccessTypesOnOwnOrLowerTechnicalLayerDependencyRule(), mapper),
                  new BaseDependencyRule<ArchRuleExampleType, ArchRuleExampleDependency>(new OnlyAccessTypesOnOwnOrLowerDomainLayerDependencyRule(), mapper)
             };

            return new DependencyRuleEngine(typeRules, dependencyRules);
        }

    }
}