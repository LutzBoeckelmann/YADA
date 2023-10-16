// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using NUnit.Framework;
using YADA.DependencyRuleEngine;
using ArchRuleDemo.ArchRuleExampleDependencyRuleEngine;
using ArchRuleDemo.ArchitecturalModel;
using ArchRuleDemo.ArchitecturalRules;

namespace ArchRuleDemo.ArchRuleTests
{
    [TestFixture]
    public class OnlyAccessTypesOnOwnOrLowerDomainLayerDependencyRuleTests
    {
        [Test]
        public void Apply_DependencyWithoutValidType_Ignore()
        {
            var type = new ArchRuleExampleType("", null, null, null, true);

            var dependencyInvalidType = new ArchRuleExampleType("", null, null, null, false);

            var sut = new OnlyAccessTypesOnOwnOrLowerDomainLayerDependencyRule();

            var result = sut.Apply(type, new ArchRuleExampleDependency(dependencyInvalidType), new TestFeedbackCollector());

            Assert.That(result, Is.EqualTo(DependencyRuleResult.Ignored));

        }

        [Test]
        public void Apply_ValidDependencyInSameLevel_Approve()
        {
            var type = new ArchRuleExampleType("", new ArchRuleDomainLayer(ArchRuleDomainLayer.Core), null, null, true);

            var otherValidType = new ArchRuleExampleType("", new ArchRuleDomainLayer("Core"), null, null, true);

            var sut = new OnlyAccessTypesOnOwnOrLowerDomainLayerDependencyRule();

            var result = sut.Apply(type, new ArchRuleExampleDependency(otherValidType), new TestFeedbackCollector());

            Assert.That(result, Is.EqualTo(DependencyRuleResult.Approve));
        }

        [Test]
        public void Apply_ValidDependencyInSameLevel_NoFeedback()
        {
            var type = new ArchRuleExampleType("", new ArchRuleDomainLayer(ArchRuleDomainLayer.Core), null, null, true);

            var otherValidType = new ArchRuleExampleType("", new ArchRuleDomainLayer("Core"), null, null, true);

            var sut = new OnlyAccessTypesOnOwnOrLowerDomainLayerDependencyRule();
            var feedback = new TestFeedbackCollector();
            var result = sut.Apply(type, new ArchRuleExampleDependency(otherValidType), feedback);

            Assert.That(feedback.AddFeedbackForTypeCalls, Is.Empty);
        }

        [Test]
        public void Apply_ValidDependencyOnLowerLevel_Approve()
        {
            var type = new ArchRuleExampleType("", new ArchRuleDomainLayer(ArchRuleDomainLayer.Extensions), null, null, true);
            var otherValidType = new ArchRuleExampleType("", new ArchRuleDomainLayer(ArchRuleDomainLayer.Infrastructure), null, null, true);

            var sut = new OnlyAccessTypesOnOwnOrLowerDomainLayerDependencyRule();

            var result = sut.Apply(type, new ArchRuleExampleDependency(otherValidType), new TestFeedbackCollector());

            Assert.That(result, Is.EqualTo(DependencyRuleResult.Approve));
        }

        [Test]
        public void Apply_ValidDependencyOnLowerLevel_NoFeedback()
        {
            var type = new ArchRuleExampleType("", new ArchRuleDomainLayer(ArchRuleDomainLayer.Extensions), null, null, true);
            var otherValidType = new ArchRuleExampleType("", new ArchRuleDomainLayer(ArchRuleDomainLayer.Infrastructure), null, null, true);

            var sut = new OnlyAccessTypesOnOwnOrLowerDomainLayerDependencyRule();
            var feedback = new TestFeedbackCollector();

            sut.Apply(type, new ArchRuleExampleDependency(otherValidType), feedback);

            Assert.That(feedback.AddFeedbackForTypeCalls, Is.Empty);
        }

        [Test]
        public void Apply_ValidDependencyOnLowerLevel_Reject()
        {
            var type = new ArchRuleExampleType("ArchRuleDomainLayer.Infrastructure", new ArchRuleDomainLayer(ArchRuleDomainLayer.Infrastructure), null, null, true);
            var inaccessibleType = new ArchRuleExampleType("ArchRuleDomainLayer.Extensions", new ArchRuleDomainLayer(ArchRuleDomainLayer.Extensions), null, null, true);

            var sut = new OnlyAccessTypesOnOwnOrLowerDomainLayerDependencyRule();

            var result = sut.Apply(type, new ArchRuleExampleDependency(inaccessibleType), new TestFeedbackCollector());

            Assert.That(result, Is.EqualTo(DependencyRuleResult.Reject));
        }

        [Test]
        public void Apply_ValidDependencyOnLowerLevel_ExpectedFeedback()
        {
            var type = new ArchRuleExampleType("ArchRuleDomainLayer.Infrastructure", new ArchRuleDomainLayer(ArchRuleDomainLayer.Infrastructure), null, null, true);
            var inaccessibleType = new ArchRuleExampleType("ArchRuleDomainLayer.Extensions", new ArchRuleDomainLayer(ArchRuleDomainLayer.Extensions), null, null, true);

            var sut = new OnlyAccessTypesOnOwnOrLowerDomainLayerDependencyRule();
            var feedback = new TestFeedbackCollector();

            sut.Apply(type, new ArchRuleExampleDependency(inaccessibleType), feedback);

            Assert.That(feedback.AddFeedbackForTypeCalls, Has.Exactly(1).EqualTo("ArchRuleDomainLayer.Infrastructure"));
            Assert.That(feedback.ViolatesRuleCalls, Has.Exactly(1).EqualTo(nameof(OnlyAccessTypesOnOwnOrLowerDomainLayerDependencyRule)));
            Assert.That(feedback.ForbiddenDependencyCalls, Has.Exactly(1).EqualTo("ArchRuleDomainLayer.Extensions"));
            Assert.That(feedback.AddInfoCalls, Is.Empty);
            Assert.That(feedback.AtCalls, Is.Empty);
        }
    }
}