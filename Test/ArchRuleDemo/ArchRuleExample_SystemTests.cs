using NUnit.Framework;
using YADA.Core;

namespace YADA.Test
{
    [TestFixture]
    public class ArchRuleExample_SystemTests
    {
        [Test]
        public void Test()
        {
            var sut = new Core.TypeLoader(new[] { @"./ArchRuleExample.dll" });
            var types = sut.GetTypes();

            var engine = CreateSut();
            var feedback = new SimpleStringCollectionFeedbackSet();
            var result = engine.Analyse(types, feedback);

            Assert.That(result, Is.False);

        }


        private ArchRuleExampleRuleEngine CreateSut()
        {
            var typeRepository = new ArchRuleExampleTypeRepository();
            var mapper = new ArchRuleExampleRuleEngineMapper(typeRepository);

            var typeRules = new ITypeRule<ArchRuleExampleType>[]
            {
                new CorrectNamespaceTypeRule()
            };

            var dependencyRules = new IDependencyRule<ArchRuleExampleType, ArchRuleExampleDependency>[]
            {
                new CrossComponentAccessOnlyOnSameTechnicalLayerDependencyRule(),
                new OnlyAccessTypesOnOwnOrLowerTechnicalLayerDependencyRule(),
                new OnlyAccessTypesOnOwnOrLowerDomainLayerDependencyRule()
            };

            return new ArchRuleExampleRuleEngine(typeRules, dependencyRules, mapper);
        }


    }
}