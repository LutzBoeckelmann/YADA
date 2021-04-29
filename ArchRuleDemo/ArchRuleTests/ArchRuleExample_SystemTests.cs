using NUnit.Framework;
using ArchRuleDemo.ArchitecturalModel;
using ArchRuleDemo.ArchitecturalRules;
using YADA.Core.DependencyRuleEngine.Rules;
using YADA.Core.DependencyRuleEngine.Feedback;
using ArchRuleDemo.DependencyRuleEngine;
using YADA.Core.Analyser;

namespace ArchRuleDemo.ArchRuleTests
{
    [TestFixture]
    public class ArchRuleExample_SystemTests
    {
        [Test]
        public void Test()
        {
            var sut = new TypeLoader(new[] { @"./ArchRuleExample.dll" });
            var types = sut.GetTypes();

            var engine = CreateSut();
            var feedback = new FeedbackCollector();
            var result = engine.Analyse(types, feedback);


            TestContext.WriteLine("--------------------------------------------");
            TestContext.WriteLine(feedback.Print().ToString());

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