// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using NUnit.Framework;

using YADA.Core.DependencyRuleEngine;
using ArchRuleDemo.ArchRuleExampleDependencyRuleEngine;
using ArchRuleDemo.ArchitecturalModel;
using ArchRuleDemo.ArchitecturalRules;

namespace ArchRuleDemo.ArchRuleTests
{
    [TestFixture]
    public class CrossComponentAccessOnlyOnSameTechnicalLayerDependencyRuleTests
    {
        [Test]
        public void Apply_DependencyWithoutValidType_Ignore()
        {
            var type = new ArchRuleExampleType("", null, null, null, true);
            var dependencyType = new ArchRuleExampleType("", null, null, null, false);

            var sut = new CrossComponentAccessOnlyOnSameTechnicalLayerDependencyRule();

            var result = sut.Apply(type, new ArchRuleExampleDependency(dependencyType), new TestFeedbackCollector());

            Assert.That(result, Is.EqualTo(DependencyRuleResult.Ignored));
        }

        [Test]
        public void Apply_DependencyInSameComponent_Approve()
        {
            var type = new ArchRuleExampleType("", null, new ArchRuleModule("Module"), null, true);
            var dependencyType = new ArchRuleExampleType("", null, new ArchRuleModule("Module"), null, true);

            var sut = new CrossComponentAccessOnlyOnSameTechnicalLayerDependencyRule();

            var result = sut.Apply(type, new ArchRuleExampleDependency(dependencyType), new TestFeedbackCollector());

            Assert.That(result, Is.EqualTo(DependencyRuleResult.Approve));

        }

        [Test]
        public void Apply_DependencyDifferentModulesSameLayer_Approve()
        {
            var type = new ArchRuleExampleType("", null, new ArchRuleModule("Module"), new ArchRuleTechnicalLayer(ArchRuleTechnicalLayer.BusinessLogic), true);
            var dependencyType = new ArchRuleExampleType("", null, new ArchRuleModule("OtherModule"), new ArchRuleTechnicalLayer(ArchRuleTechnicalLayer.BusinessLogic), true);

            var sut = new CrossComponentAccessOnlyOnSameTechnicalLayerDependencyRule();

            var result = sut.Apply(type, new ArchRuleExampleDependency(dependencyType), new TestFeedbackCollector());

            Assert.That(result, Is.EqualTo(DependencyRuleResult.Approve));

        }

        [Test]
        public void Apply_DependencyInSameComponent_NoFeedback()
        {
            var type = new ArchRuleExampleType("", null, new ArchRuleModule("Module"), null, true);
            var dependencyType = new ArchRuleExampleType("", null, new ArchRuleModule("Module"), null, true);
            var feedback = new TestFeedbackCollector();
            var sut = new CrossComponentAccessOnlyOnSameTechnicalLayerDependencyRule();

            var result = sut.Apply(type, new ArchRuleExampleDependency(dependencyType), feedback);

            Assert.That(feedback.AddFeedbackForTypeCalls, Is.Empty);

        }

        [Test]
        public void Apply_DependencyDifferentModulesSameLayer_NoFeedback()
        {
            var type = new ArchRuleExampleType("", null, new ArchRuleModule("Module"), new ArchRuleTechnicalLayer(ArchRuleTechnicalLayer.BusinessLogic), true);
            var dependencyType = new ArchRuleExampleType("", null, new ArchRuleModule("OtherModule"), new ArchRuleTechnicalLayer(ArchRuleTechnicalLayer.BusinessLogic), true);
            var feedback = new TestFeedbackCollector();
            var sut = new CrossComponentAccessOnlyOnSameTechnicalLayerDependencyRule();

            sut.Apply(type, new ArchRuleExampleDependency(dependencyType), feedback);

            Assert.That(feedback.AddFeedbackForTypeCalls, Is.Empty);

        }

        [Test]
        public void Apply_DependencyDifferentModulesDifferentLayer_Reject()
        {
            var type = new ArchRuleExampleType("", null, new ArchRuleModule("Module"), new ArchRuleTechnicalLayer(ArchRuleTechnicalLayer.BusinessLogic), true);
            var dependencyType = new ArchRuleExampleType("", null, new ArchRuleModule("OtherModule"), new ArchRuleTechnicalLayer(ArchRuleTechnicalLayer.Data), true);

            var sut = new CrossComponentAccessOnlyOnSameTechnicalLayerDependencyRule();

            var result = sut.Apply(type, new ArchRuleExampleDependency(dependencyType), new TestFeedbackCollector());

            Assert.That(result, Is.EqualTo(DependencyRuleResult.Reject));
        }

        [Test]
        public void Apply_DependencyDifferentModulesDifferentLayer_ExpectedOutput()
        {
            var type = new ArchRuleExampleType("My.Module", null, new ArchRuleModule("Module"), new ArchRuleTechnicalLayer(ArchRuleTechnicalLayer.BusinessLogic), true);
            var dependencyType = new ArchRuleExampleType("OtherType.OtherModule", null, new ArchRuleModule("OtherModule"), new ArchRuleTechnicalLayer(ArchRuleTechnicalLayer.Data), true);

            var sut = new CrossComponentAccessOnlyOnSameTechnicalLayerDependencyRule();
            var feedback = new TestFeedbackCollector();
            sut.Apply(type, new ArchRuleExampleDependency(dependencyType), feedback);

            Assert.That(feedback.AddFeedbackForTypeCalls, Has.Exactly(1).EqualTo("My.Module"));
            Assert.That(feedback.ViolatesRuleCalls, Has.Exactly(1).EqualTo(nameof(CrossComponentAccessOnlyOnSameTechnicalLayerDependencyRule)));
            Assert.That(feedback.ForbiddenDependencyCalls, Has.Exactly(1).EqualTo("OtherType.OtherModule"));
            Assert.That(feedback.AddInfoCalls, Is.Empty);
            Assert.That(feedback.AtCalls, Is.Empty);



        }
    }
}