// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Linq;
using Moq;
using NUnit.Framework;
using YADA.Core;

namespace YADA.Test
{
    [TestFixture]
    public class OnlyAccessTypesOnOwnOrLowerDomainLayerDependencyRuleTests 
    {
        [Test]
        public void Apply_DependencyWithoutValidType_Ignore() 
        {
            var type = new ArchRuleExampleType("", null,null,null, true);

            var dependencyInvalidType = new ArchRuleExampleType("", null,null,null, false);

            var sut = new OnlyAccessTypesOnOwnOrLowerDomainLayerDependencyRule();

            var result = sut.Apply(type, new ArchRuleExampleDependency(dependencyInvalidType), new SimpleStringCollectionFeedbackSet());

            Assert.That(result, Is.EqualTo(DependencyRuleResult.Ignore));

        }

        [Test]
        public void Apply_ValidDependencyInSameLevel_Approve() 
        {
            var type = new ArchRuleExampleType("",new ArchRuleDomainLayer(ArchRuleDomainLayer.Core),null,null, true);

            var otherValidType = new ArchRuleExampleType("",new ArchRuleDomainLayer("Core"),null,null, true);
            
            var sut = new OnlyAccessTypesOnOwnOrLowerDomainLayerDependencyRule();

            var result = sut.Apply(type, new ArchRuleExampleDependency(otherValidType), new SimpleStringCollectionFeedbackSet());

            Assert.That(result, Is.EqualTo(DependencyRuleResult.Approve));
        }

      [Test]
        public void Apply_ValidDependencyInSameLevel_NoFeedback() 
        {
            var type = new ArchRuleExampleType("",new ArchRuleDomainLayer(ArchRuleDomainLayer.Core),null,null, true);

            var otherValidType = new ArchRuleExampleType("",new ArchRuleDomainLayer("Core"),null,null, true);
            
            var sut = new OnlyAccessTypesOnOwnOrLowerDomainLayerDependencyRule();
            var feedback = new SimpleStringCollectionFeedbackSet();
            var result = sut.Apply(type, new ArchRuleExampleDependency(otherValidType), feedback);

            Assert.That(feedback.Messages, Is.Empty);
        }

        [Test]
        public void Apply_ValidDependencyOnLowerLevel_Approve() 
        {
            var type = new ArchRuleExampleType("",new ArchRuleDomainLayer(ArchRuleDomainLayer.Extentions), null, null, true);
            var otherValidType = new ArchRuleExampleType("",new ArchRuleDomainLayer(ArchRuleDomainLayer.Infrastructure),null,null, true);

            var sut = new OnlyAccessTypesOnOwnOrLowerDomainLayerDependencyRule();

            var result = sut.Apply(type, new ArchRuleExampleDependency(otherValidType), new SimpleStringCollectionFeedbackSet());

            Assert.That(result, Is.EqualTo(DependencyRuleResult.Approve));
        }

        [Test]
        public void Apply_ValidDependencyOnLowerLevel_NoFeedback() 
        {
            var type = new ArchRuleExampleType("",new ArchRuleDomainLayer(ArchRuleDomainLayer.Extentions), null, null, true);
            var otherValidType = new ArchRuleExampleType("",new ArchRuleDomainLayer(ArchRuleDomainLayer.Infrastructure),null,null, true);

            var sut = new OnlyAccessTypesOnOwnOrLowerDomainLayerDependencyRule();
            var feedback = new SimpleStringCollectionFeedbackSet();

            sut.Apply(type, new ArchRuleExampleDependency(otherValidType), feedback);

            Assert.That(feedback.Messages, Is.Empty);
        }

        [Test]
        public void Apply_ValidDependencyOnLowerLevel_Reject() 
        {
            var type = new ArchRuleExampleType("ArchRuleDomainLayer.Infrastructure",new ArchRuleDomainLayer(ArchRuleDomainLayer.Infrastructure), null, null, true);
            var inaccessibleType = new ArchRuleExampleType("ArchRuleDomainLayer.Extentions",new ArchRuleDomainLayer(ArchRuleDomainLayer.Extentions),null,null, true);

            var sut = new OnlyAccessTypesOnOwnOrLowerDomainLayerDependencyRule();

            var result = sut.Apply(type, new ArchRuleExampleDependency(inaccessibleType), new SimpleStringCollectionFeedbackSet());

            Assert.That(result, Is.EqualTo(DependencyRuleResult.Reject));
        }

        [Test]
        public void Apply_ValidDependencyOnLowerLevel_ExpectedFeedback() 
        {
            var type = new ArchRuleExampleType("ArchRuleDomainLayer.Infrastructure",new ArchRuleDomainLayer(ArchRuleDomainLayer.Infrastructure), null, null, true);
            var inaccessibleType = new ArchRuleExampleType("ArchRuleDomainLayer.Extentions",new ArchRuleDomainLayer(ArchRuleDomainLayer.Extentions),null,null, true);

            var sut = new OnlyAccessTypesOnOwnOrLowerDomainLayerDependencyRule();
            var feedback = new SimpleStringCollectionFeedbackSet();

            sut.Apply(type, new ArchRuleExampleDependency(inaccessibleType), feedback);
            
            Assert.That(feedback.Messages, Has.Exactly(1).Items);
            Assert.That(feedback.Messages.First(), Contains.Substring("OnlyAccessTypesOnOwnOrLowerDomainLayerDependencyRule").And.Contains("ArchRuleDomainLayer.Infrastructure").And.Contains("ArchRuleDomainLayer.Extentions"));
        }
    }
}