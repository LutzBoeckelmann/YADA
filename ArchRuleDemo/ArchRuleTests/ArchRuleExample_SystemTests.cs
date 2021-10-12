using NUnit.Framework;
using ArchRuleDemo.ArchitecturalModel;
using ArchRuleDemo.ArchitecturalRules;
using YADA.Core.DependencyRuleEngine.Rules;
using YADA.Core.DependencyRuleEngine.Feedback;
using ArchRuleDemo.ArchRuleExampleDependencyRuleEngine;
using YADA.Core.Analyser;
using YADA.Core.DependencyRuleEngine;
using System.Collections.Generic;
using System.Linq;

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

//            TestContext.WriteLine(feedback.GetFeedback().Count());
            
            Assert.That(result, Is.False);
        }

        [Test]
        public void Output_All_Violations_In_Example_Assembly()
        {
            var sut = new TypeLoader(new[] { @"./ArchRuleExample.dll" });
            var types = sut.GetTypes();

            var engine = CreateSut();
            var feedback = new FeedbackCollector();
            var result = engine.Analyse(types, feedback);

            ResultCollectorSimplePrinter printer = new ResultCollectorSimplePrinter();

            feedback.Explore(printer);

            TestContext.WriteLine(printer.GetFeedback());

            // foreach (var pair in feedback.GetFeedback())
            // {
            //     TestContext.WriteLine("--------------------------------------------");
            //     TestContext.WriteLine($"Type: {pair.Item1}");
            //     TestContext.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            //     TestContext.WriteLine(pair.Item2);
            // }
            
            Assert.That(result, Is.False);
        }

        [Test]
        public void Analyse_SingleTypeOnWhitelist_SingleTypeFails() 
        {
            var sut = new TypeLoader(new[] { @"./ArchRuleExample.dll" });
            var types = sut.GetTypes();

            var engine = CreateWithWhiteListSut(new []{"ArchRuleExample.Infrastructure.InfraModule1.UI.InfraModuleUIClass1"});
            var feedback = new FeedbackCollector();
            var result = engine.Analyse(types, feedback);

            ResultCollectorSimplePrinter printer = new ResultCollectorSimplePrinter();

            feedback.Explore(printer);

            TestContext.WriteLine(printer.GetFeedback());

            // // var typeFeedback = feedback.GetFeedback().Select(t=>t.Item1).ToArray();
            //     foreach (var pair in feedback.GetFeedback())
            // {
            //     TestContext.WriteLine("--------------------------------------------");
            //     TestContext.WriteLine($"Type: {pair.Item1}");
            //     TestContext.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            //     TestContext.WriteLine(pair.Item2);
            // }
            // // Assert.That(typeFeedback, Is.EquivalentTo(new[] { "ArchRuleExample.Core.CoreModule1.Data.Module1DataClass1" }));

            Assert.That(result, Is.False);
        }

    // only correct types should not fail
        [Test]
        public void Analyse_OnlyCorrectTypes_Success() 
        {
            var sut = new TypeLoader(new[] { @"./ArchRuleExample.dll" });
            var types = sut.GetTypes();

            var engine = CreateWithWhiteListSut(new []{"ArchRuleExample.Core.CoreModule1.Data.Module1DataClass1"});
            var feedback = new FeedbackCollector();
            var result = engine.Analyse(types, feedback);
            ResultCollectorSimplePrinter printer = new ResultCollectorSimplePrinter();

            feedback.Explore(printer);

            Assert.That(printer.GetFeedback(), Is.Empty);

            Assert.That(result, Is.True);
        }

    	
        private DependencyRuleEngine CreateWithWhiteListSut(IEnumerable<string> whiteList)
        {
            var typeRepository = new ArchRuleExampleTypeRepository();
            var mapper = new ArchRuleExampleRuleEngineMapper(typeRepository);

            var typeRules = new ITypeRule<ITypeDescription>[]
            {
                new WhitelistIgnoreTypeRule(whiteList),
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