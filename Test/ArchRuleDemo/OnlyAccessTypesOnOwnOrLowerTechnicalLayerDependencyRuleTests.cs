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
            var type = MoqProvider.GetTypeDescriptionMoq("ValidType");

            var invalidType = MoqProvider.GetTypeDescriptionMoq("InValidType");

            var dependency = MoqProvider.GetDependencyMoq(invalidType);

            var moq = new Mock<IArchRuleExampleTypeRepository>();

            moq.Setup(m => m.GetTypeRepresentation("ValidType")).Returns(new ArchRuleExampleTypes(null,null,null, true));
            moq.Setup(m => m.GetTypeRepresentation("InValidType")).Returns(new ArchRuleExampleTypes(null,null,null, false));

            var sut = new OnlyAccessTypesOnOwnOrLowerTechnicalLayerDependencyRule(moq.Object);

            var result = sut.Apply(type, dependency);

            Assert.That(result, Is.EqualTo(DependencyRuleResult.Ignore));

        }

        [Test]
        public void Apply_ValidDependencyInSameLevel_Approve() 
        {
             var type = MoqProvider.GetTypeDescriptionMoq("ValidType");

            var otherValidType = MoqProvider.GetTypeDescriptionMoq("OtherValidType");

            var dependency = MoqProvider.GetDependencyMoq(otherValidType);

            var moq = new Mock<IArchRuleExampleTypeRepository>();

            moq.Setup(m => m.GetTypeRepresentation("ValidType")).Returns(new ArchRuleExampleTypes(null,null,new ArchRuleTechnicalLayer(ArchRuleTechnicalLayer.BusinessLogic), true));
            moq.Setup(m => m.GetTypeRepresentation("OtherValidType")).Returns(new ArchRuleExampleTypes(null,null,new ArchRuleTechnicalLayer(ArchRuleTechnicalLayer.BusinessLogic), true));

            var sut = new OnlyAccessTypesOnOwnOrLowerTechnicalLayerDependencyRule(moq.Object);

            var result = sut.Apply(type, dependency);

            Assert.That(result, Is.EqualTo(DependencyRuleResult.Approve));
        }

        [Test]
        public void Apply_ValidDependencyOnLowerLevel_Approve() 
        {
             var type = MoqProvider.GetTypeDescriptionMoq("ValidType");

            var otherValidType = MoqProvider.GetTypeDescriptionMoq("OtherValidType");

            var dependency = MoqProvider.GetDependencyMoq(otherValidType);

            var moq = new Mock<IArchRuleExampleTypeRepository>();

            moq.Setup(m => m.GetTypeRepresentation("ValidType")).Returns(new ArchRuleExampleTypes(null,null,new ArchRuleTechnicalLayer(ArchRuleTechnicalLayer.UI), true));
            moq.Setup(m => m.GetTypeRepresentation("OtherValidType")).Returns(new ArchRuleExampleTypes(null,null,new ArchRuleTechnicalLayer(ArchRuleTechnicalLayer.Data), true));

            var sut = new OnlyAccessTypesOnOwnOrLowerTechnicalLayerDependencyRule(moq.Object);

            var result = sut.Apply(type, dependency);

            Assert.That(result, Is.EqualTo(DependencyRuleResult.Approve));
        }
    }
}