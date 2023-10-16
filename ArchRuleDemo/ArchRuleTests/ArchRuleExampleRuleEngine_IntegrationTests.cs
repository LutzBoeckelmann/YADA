// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using NUnit.Framework;
using YADA.Analyzer;
using ArchRuleDemo.ArchRuleExampleDependencyRuleEngine;

using ArchRuleDemo.ArchitecturalModel;
using ArchRuleDemo.ArchitecturalRules;
using YADA.DependencyRuleEngine.Rules;
using YADA.DependencyRuleEngine.Feedback;
using YADA.DependencyRuleEngine;

namespace ArchRuleDemo.ArchRuleTests
{
    [TestFixture]
    public class ArchRuleExampleRuleEngine_IntegrationTests
    {
        [Test]
        public void Analyse_ValidTypes_True()
        {
            var sut = CreateSut();

            var typ1 = new TypeDescriptionFake("ArchRuleExample.Infrastructure.Module.Data.Class1", "ArchRuleExample");
            var typ2 = new TypeDescriptionFake("ArchRuleExample.Infrastructure.Module.Data.Class2", "ArchRuleExample");

            typ2.Add(typ1);

            var result = sut.Analyse(new ITypeDescription[] { typ1, typ2 }, new FeedbackCollector());

            Assert.That(result, Is.True);
        }

        [Test]
        public void Analyse_TypeWithLayerViolation_False()
        {
            var sut = CreateSut();

            var typ1 = new TypeDescriptionFake("ArchRuleExample.Core.Module.Data.Class1", "ArchRuleExample");

            var typ2 = new TypeDescriptionFake("ArchRuleExample.Infrastructure.Module.Data.Class2", "ArchRuleExample");

            typ2.Add(typ1);

            var result = sut.Analyse(new ITypeDescription[] { typ1, typ2 }, new FeedbackCollector());

            Assert.That(result, Is.False);
        }

        [Test]
        public void Analyse_SomeTypeWithLayerViolationCorrectAsLast_False()
        {
            var sut = CreateSut();

            var typ1 = new TypeDescriptionFake("ArchRuleExample.Core.Module.Data.Class1", "ArchRuleExample");
            var typ2 = new TypeDescriptionFake("ArchRuleExample.Infrastructure.Module.Data.Class2", "ArchRuleExample");
            typ2.Add(typ1);
            var typ3 = new TypeDescriptionFake("ArchRuleExample.Core.Module.Data.Class3", "ArchRuleExample");

            var result = sut.Analyse(new ITypeDescription[] { typ1, typ2, typ3 }, new FeedbackCollector());

            Assert.That(result, Is.False);
        } 
        
        private RuleEngine CreateSut()
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

            return new RuleEngine(typeRules, dependencyRules);
        }

    }
}