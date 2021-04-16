// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

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
            var type = MoqProvider.GetTypeDescriptionMoq("ValidType");

            var invalidType = MoqProvider.GetTypeDescriptionMoq("InValidType");

            var dependency = MoqProvider.GetDependencyMoq(invalidType);

            var moq = new Mock<IArchRuleExampleTypeRepository>();

            moq.Setup(m => m.GetTypeRepresentation("ValidType")).Returns(new ArchRuleExampleTypes(null,null,null, true));
            moq.Setup(m => m.GetTypeRepresentation("InValidType")).Returns(new ArchRuleExampleTypes(null,null,null, false));

            var sut = new OnlyAccessTypesOnOwnOrLowerDomainLayerDependencyRule(moq.Object);

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

            moq.Setup(m => m.GetTypeRepresentation("ValidType")).Returns(new ArchRuleExampleTypes(new ArchRuleDomainLayer(ArchRuleDomainLayer.Core),null,null, true));
            moq.Setup(m => m.GetTypeRepresentation("OtherValidType")).Returns(new ArchRuleExampleTypes(new ArchRuleDomainLayer("Core"),null,null, true));

            var sut = new OnlyAccessTypesOnOwnOrLowerDomainLayerDependencyRule(moq.Object);

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

            moq.Setup(m => m.GetTypeRepresentation("ValidType")).Returns(new ArchRuleExampleTypes(new ArchRuleDomainLayer(ArchRuleDomainLayer.Extentions),null,null, true));
            moq.Setup(m => m.GetTypeRepresentation("OtherValidType")).Returns(new ArchRuleExampleTypes(new ArchRuleDomainLayer(ArchRuleDomainLayer.Infrastructure),null,null, true));

            var sut = new OnlyAccessTypesOnOwnOrLowerDomainLayerDependencyRule(moq.Object);

            var result = sut.Apply(type, dependency);

            Assert.That(result, Is.EqualTo(DependencyRuleResult.Approve));
        }
    }
}