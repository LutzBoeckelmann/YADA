// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using NUnit.Framework;
using ArchRuleDemo.ArchitecturalModel;
using ArchRuleDemo.ArchitecturalRules;
using YADA.DependencyRuleEngine.Rules;
using YADA.DependencyRuleEngine.Feedback;
using ArchRuleDemo.ArchRuleExampleDependencyRuleEngine;
using YADA.Analyzer;
using YADA.DependencyRuleEngine;
using System.Collections.Generic;
using YADA.DependencyRuleEngine.Feedback.Recorder;

namespace ArchRuleDemo.ArchRuleTests
{
    [TestFixture]
    public class ArchRuleExample_SystemTests
    {
        [Test]
        public void FeedbackRecorder_FeedbackReader_Roundtrip()
        {
            var sut = new TypeLoader(new[] { "ArchRuleExample.dll" });
            var types = sut.GetTypes();
            
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

            var engine = new RuleEngine(typeRules, dependencyRules);
            
         
            var feedback = new FeedbackCollector();
            
            engine.Analyse(types, feedback);

            
            var feedbackRecorder = new FeedbackRecorder();
            
            feedback.Explore(feedbackRecorder);
            feedbackRecorder.WriteFeedbackResults( "out.txt");
                       
            FeedbackReader reader = new FeedbackReader();

            reader.ReadRecording("out.txt");

            var visitorResult = feedbackRecorder.GetResult();
            var readerResult = reader.GetResult();
            
            Assert.That(visitorResult, Is.EquivalentTo(readerResult));

            if(System.IO.File.Exists("out.txt"))
            {
                System.IO.File.Delete(@"out.txt");
            }
            
        }
        [IgnoreType("ArchRuleExample.Infrastructure.InfraModule1.UI.InfraModuleUIClass2")]
        [Test]
        public void FeedbackFilter_NoChangesSinceLastBaseline_NoFeedback()
        {
            var sut = new TypeLoader(new[] { "ArchRuleExample.dll" });
            var types = sut.GetTypes();
            
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

            var engine = new RuleEngine(typeRules, dependencyRules);
                     
            var feedback = new FeedbackCollector();
            
            engine.Analyse(types, feedback);

            ResultCollectorSimplePrinter printer = new ResultCollectorSimplePrinter();
            
            var filter = new FeedbackFilter(@"ArchRuleTests/TestData/CompleteBaselineArchRuleDemo.txt", printer);
            
            feedback.Explore(filter);
                
            TestContext.WriteLine(printer.GetFeedback());
            var printerFeedBack = printer.GetFeedback();
            Assert.That(printerFeedBack , Is.Empty);
        }


        [Test]
        public void Output_All_Violations_In_Example_Assembly()
        {
            var sut = new TypeLoader(new[] { "ArchRuleExample.dll" });
            var types = sut.GetTypes();

            var engine = CreateSut();
            var feedback = new FeedbackCollector();
            var result = engine.Analyse(types, feedback);

            ResultCollectorSimplePrinter printer = new ResultCollectorSimplePrinter();

            feedback.Explore(printer);

            TestContext.WriteLine(printer.GetFeedback());

            
            Assert.That(result, Is.False);
        }

        [Test]
        public void Analyse_SingleTypeOnWhitelist_SingleTypeFails() 
        {
            var sut = new TypeLoader(new[] { "ArchRuleExample.dll" });
            var types = sut.GetTypes();

            var engine = CreateWithWhiteListSut(new []{"ArchRuleExample.Infrastructure.InfraModule1.UI.InfraModuleUIClass1"});
            var feedback = new FeedbackCollector();
            var result = engine.Analyse(types, feedback);

            ResultCollectorSimplePrinter printer = new ResultCollectorSimplePrinter();

            feedback.Explore(printer);

            TestContext.WriteLine(printer.GetFeedback());
         
            Assert.That(result, Is.False);
        }

        [Test]
        public void Analyse_OnlyCorrectTypes_Success() 
        {
            var sut = new TypeLoader(new[] { "ArchRuleExample.dll" });
            var types = sut.GetTypes();

            var engine = CreateWithWhiteListSut(new []{"ArchRuleExample.Core.CoreModule1.Data.Module1DataClass1"});
            var feedback = new FeedbackCollector();
            var result = engine.Analyse(types, feedback);
            ResultCollectorSimplePrinter printer = new ResultCollectorSimplePrinter();

            feedback.Explore(printer);

            Assert.That(printer.GetFeedback(), Is.Empty);

            Assert.That(result, Is.True);
        }

    	
        private RuleEngine CreateWithWhiteListSut(IEnumerable<string> whiteList)
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

            return new RuleEngine(typeRules, dependencyRules);
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