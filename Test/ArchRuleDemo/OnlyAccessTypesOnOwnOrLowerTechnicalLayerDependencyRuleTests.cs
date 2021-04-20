// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Linq;
using Moq;
using NUnit.Framework;
using YADA.Core;

namespace YADA.Test
{
    [TestFixture]
    public class OnlyAccessTypesOnOwnOrLowerTechnicalLayerDependencyRuleTests 
    {
        [Test]
        public void Apply_DependencyWithoutValidType_Ignore() 
        {
            var type = new ArchRuleExampleType("",null,null,null, true);
            var invalidType = new ArchRuleExampleType("",null,null,null, false);

            var sut = new OnlyAccessTypesOnOwnOrLowerTechnicalLayerDependencyRule();

            var result = sut.Apply(type, new ArchRuleExampleDependency(invalidType), new SimpleStringCollectionFeedbackSet());

            Assert.That(result, Is.EqualTo(DependencyRuleResult.Ignore));
        }

        [Test]
        public void Apply_ValidDependencyInSameLevel_Approve() 
        {
            var type = new ArchRuleExampleType("",null,null,new ArchRuleTechnicalLayer(ArchRuleTechnicalLayer.BusinessLogic), true);
            var otherValidType = new ArchRuleExampleType("",null, null, new ArchRuleTechnicalLayer(ArchRuleTechnicalLayer.BusinessLogic), true);

            var sut = new OnlyAccessTypesOnOwnOrLowerTechnicalLayerDependencyRule();

            var result = sut.Apply(type, new ArchRuleExampleDependency(otherValidType), new SimpleStringCollectionFeedbackSet());

            Assert.That(result, Is.EqualTo(DependencyRuleResult.Approve));
        }

        [Test]
        public void Apply_ValidDependencyInSameLevel_NoFeedback() 
        {
            var type = new ArchRuleExampleType("",null,null,new ArchRuleTechnicalLayer(ArchRuleTechnicalLayer.BusinessLogic), true);
            var otherValidType = new ArchRuleExampleType("",null, null, new ArchRuleTechnicalLayer(ArchRuleTechnicalLayer.BusinessLogic), true);

            var sut = new OnlyAccessTypesOnOwnOrLowerTechnicalLayerDependencyRule();
            var feedback = new SimpleStringCollectionFeedbackSet();

            sut.Apply(type, new ArchRuleExampleDependency(otherValidType), feedback);

            Assert.That(feedback.Messages, Is.Empty);
        }

        [Test]
        public void Apply_ValidDependencyOnLowerLevel_Approve() 
        {
            var type = new ArchRuleExampleType("",null,null,new ArchRuleTechnicalLayer(ArchRuleTechnicalLayer.UI), true);

            var otherValidType = new ArchRuleExampleType("",null,null,new ArchRuleTechnicalLayer(ArchRuleTechnicalLayer.Data), true);

            var sut = new OnlyAccessTypesOnOwnOrLowerTechnicalLayerDependencyRule();

            var result = sut.Apply(type, new ArchRuleExampleDependency(otherValidType), new SimpleStringCollectionFeedbackSet());

            Assert.That(result, Is.EqualTo(DependencyRuleResult.Approve));
        }

        [Test]
        public void Apply_ValidDependencyOnLowerLevel_NoFeedback() 
        {
            var type = new ArchRuleExampleType("",null,null,new ArchRuleTechnicalLayer(ArchRuleTechnicalLayer.UI), true);

            var otherValidType = new ArchRuleExampleType("",null,null,new ArchRuleTechnicalLayer(ArchRuleTechnicalLayer.Data), true);

            var sut = new OnlyAccessTypesOnOwnOrLowerTechnicalLayerDependencyRule();
            var feedback = new SimpleStringCollectionFeedbackSet();

            sut.Apply(type, new ArchRuleExampleDependency(otherValidType), feedback);

            Assert.That(feedback.Messages, Is.Empty);
        }

        [Test]
        public void Apply_ValidDependencyOnLowerLevel_Reject() 
        {
            var type = new ArchRuleExampleType("",null,null,new ArchRuleTechnicalLayer(ArchRuleTechnicalLayer.Data), true);

            var inaccessibleType = new ArchRuleExampleType("",null,null,new ArchRuleTechnicalLayer(ArchRuleTechnicalLayer.UI), true);

            var sut = new OnlyAccessTypesOnOwnOrLowerTechnicalLayerDependencyRule();

            var result = sut.Apply(type, new ArchRuleExampleDependency(inaccessibleType), new SimpleStringCollectionFeedbackSet());

            Assert.That(result, Is.EqualTo(DependencyRuleResult.Reject));
        }

         [Test]
        public void Apply_ValidDependencyOnLowerLevel_ExpectedFeedback() 
        {
            var type = new ArchRuleExampleType(ArchRuleTechnicalLayer.Data,null,null,new ArchRuleTechnicalLayer(ArchRuleTechnicalLayer.Data), true);

            var inaccessibleType = new ArchRuleExampleType(ArchRuleTechnicalLayer.UI,null,null,new ArchRuleTechnicalLayer(ArchRuleTechnicalLayer.UI), true);

            var sut = new OnlyAccessTypesOnOwnOrLowerTechnicalLayerDependencyRule();
            var feedback = new SimpleStringCollectionFeedbackSet();
            var result = sut.Apply(type, new ArchRuleExampleDependency(inaccessibleType), feedback);
            Assert.That(feedback.Messages, Has.Exactly(1).Items);
            Assert.That(feedback.Messages.First(), Contains.Substring("OnlyAccessTypesOnOwnOrLowerTechnicalLayerDependencyRule").And.Contains(ArchRuleTechnicalLayer.Data).And.Contains(ArchRuleTechnicalLayer.UI));
        }
    }
}