// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using NUnit.Framework;
using ArchRuleDemo.ArchitecturalModel;
using ArchRuleDemo.ArchitecturalRules;
using YADA.Core.DependencyRuleEngine.Rules;
using YADA.Core.DependencyRuleEngine.Feedback;
using ArchRuleDemo.ArchRuleExampleDependencyRuleEngine;
using YADA.Core.Analyser;
using YADA.Core.DependencyRuleEngine;
using System.Collections.Generic;

namespace ArchRuleDemo.ArchRuleTests
{
    [TestFixture]
    public class ArchRuleExample_SystemTests
    {
        [Test]
        public void FeedbackRecorder_FeedbackReader_Roundtrip()
        {
            var sut = new TypeLoader(new[] { @"./ArchRuleExample.dll" });
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

            var engine = new DependencyRuleEngine(typeRules, dependencyRules);
            
         
            var feedback = new FeedbackCollector();
            
            engine.Analyse(types, feedback);

            
            var feedbackRecorder = new FeedbackRecorder();
            
            feedback.Explore(feedbackRecorder);
            feedbackRecorder.WriteFeedbackResults(@".\out.txt");
                       
            FeedbackReader reader = new FeedbackReader();

            reader.ReadRecording(@".\out.txt");

            var visitorResult = feedbackRecorder.GetResult();
            var readerResult = reader.GetResult();
            
            Assert.That(visitorResult, Is.EquivalentTo(readerResult));
        }

        [Test]
        public void FeedbackFilter_NoChangesSinceLastBaseline_NoFeedback()
        {
            var sut = new TypeLoader(new[] { @"./ArchRuleExample.dll" });
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

            var engine = new DependencyRuleEngine(typeRules, dependencyRules);
                     
            var feedback = new FeedbackCollector();
            
            engine.Analyse(types, feedback);

            ResultCollectorSimplePrinter printer = new ResultCollectorSimplePrinter();
            
            var filter = new FeedbackFilter(@".\CompleteBaselineArchRuleDemo.txt", printer);
            
            feedback.Explore(filter);
                
            TestContext.WriteLine(printer.GetFeedback());
            var printerFeedBack = printer.GetFeedback();
            Assert.That(printerFeedBack , Is.Empty);
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
         
            Assert.That(result, Is.False);
        }

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