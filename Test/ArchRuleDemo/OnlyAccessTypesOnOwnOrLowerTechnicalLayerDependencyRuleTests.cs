// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

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
        public void Apply_ValidDependencyOnLowerLevel_Approve() 
        {
            var type = new ArchRuleExampleType("",null,null,new ArchRuleTechnicalLayer(ArchRuleTechnicalLayer.UI), true);

            var otherValidType = new ArchRuleExampleType("",null,null,new ArchRuleTechnicalLayer(ArchRuleTechnicalLayer.Data), true);

            var sut = new OnlyAccessTypesOnOwnOrLowerTechnicalLayerDependencyRule();

            var result = sut.Apply(type, new ArchRuleExampleDependency(otherValidType), new SimpleStringCollectionFeedbackSet());

            Assert.That(result, Is.EqualTo(DependencyRuleResult.Approve));
        }
    }
}