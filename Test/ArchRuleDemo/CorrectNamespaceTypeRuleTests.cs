// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using YADA.Core;

namespace YADA.Test
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

            Assert.That(feedback.AddFeedbackForTypeCalls, Is.All.Contains("Fullname") );
            Assert.That(feedback.ViolatesRuleCalls, Is.All.Contains(nameof(CorrectNamespaceTypeRule) ));

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

    public class TestFeedbackCollector : IFeedbackCollector, ITypeFeedback, IRuleFeedback, IDependencyFeedback
    {
        public List<string> AddFeedbackForTypeCalls { get; } = new List<string>();
        public List<string> ViolatesRuleCalls { get; } = new List<string>();
        public List<string> ForbiddenDependencyCalls { get; } = new List<string>();


        public List<string> AddInfoCalls { get; } = new List<string>();
        public List<string> AtCalls { get; } = new List<string>();



        public ITypeFeedback AddFeedbackForType(string type)
        {
            AddFeedbackForTypeCalls.Add(type);
            return this;
        }

        public IRuleFeedback AddInfo(string name)
        {
            AddInfoCalls.Add(name);
            return this;
        }

        public IRuleFeedback ViolatesRule(string nameOfRule)
        {
            ViolatesRuleCalls.Add(nameOfRule);
            return this;
        }
        public IDependencyFeedback ForbiddenDependency(string dependency)
        {
            ForbiddenDependencyCalls.Add(dependency);
            return this;
        }

        public IDependencyFeedback At(string context)
        {
            AtCalls.Add(context);
            return this;
        }
    }

}
