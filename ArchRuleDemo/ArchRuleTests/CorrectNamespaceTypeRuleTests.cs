// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using Moq;
using NUnit.Framework;
using YADA.Core.DependencyRuleEngine;
using ArchRuleDemo.ArchitecturalModel;
using ArchRuleDemo.ArchitecturalRules;
using YADA.Core.DependencyRuleEngine.Feedback;

namespace ArchRuleDemo.ArchRuleTests
{

    [TestFixture]
    public class CorrectNamespaceTypeRuleTests
    {
        [Test]
        public void Apply_ValidType_Approve()
        {
            var sut = new CorrectNamespaceTypeRule();

            var result = sut.Apply(new ArchRuleExampleType("", null, null, null, true), new Mock<IFeedbackCollector>().Object);

            Assert.That(result, Is.EqualTo(DependencyRuleResult.Approve));
        }

        public void Apply_ValidType_NoFeedback()
        {
            var feedback = new TestFeedbackCollector();

            var sut = new CorrectNamespaceTypeRule();

            var result = sut.Apply(new ArchRuleExampleType("", null, null, null, true), feedback);

            Assert.That(feedback.AddFeedbackForTypeCalls, Is.Empty);
        }

        [Test]
        public void Apply_InValidType_ExpectedFeedback()
        {
            var feedback = new TestFeedbackCollector();

            var sut = new CorrectNamespaceTypeRule();

            var result = sut.Apply(new ArchRuleExampleType("Fullname", new ArchRuleDomainLayer(""), null, new ArchRuleTechnicalLayer(""), false), feedback);

            Assert.That(feedback.AddFeedbackForTypeCalls, Is.All.Contains("Fullname"));
            Assert.That(feedback.ViolatesRuleCalls, Is.All.Contains(nameof(CorrectNamespaceTypeRule)));

        }

        [Test]
        public void Apply_InValidType_Reject()
        {
            var feedback = new TestFeedbackCollector();

            var sut = new CorrectNamespaceTypeRule();

            var result = sut.Apply(new ArchRuleExampleType("", new ArchRuleDomainLayer(""), null, new ArchRuleTechnicalLayer(""), false), feedback);

            Assert.That(result, Is.EqualTo(DependencyRuleResult.Reject));
        }
    }

}
