using NUnit.Framework;
using ArchRuleDemo.ArchitecturalModel;
using ArchRuleDemo.ArchitecturalRules;
using YADA.Core.DependencyRuleEngine.Rules;
using YADA.Core.DependencyRuleEngine.Feedback;
using ArchRuleDemo.ArchRuleExampleDependencyRuleEngine;
using YADA.Core.Analyser;
using YADA.Core.DependencyRuleEngine;

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