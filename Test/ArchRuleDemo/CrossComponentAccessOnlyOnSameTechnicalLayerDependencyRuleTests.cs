// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using Moq;
using NUnit.Framework;
using YADA.Core;

namespace YADA.Test
{
    [TestFixture]
    public class CrossComponentAccessOnlyOnSameTechnicalLayerDependencyRuleTests 
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

            var sut = new CrossComponentAccessOnlyOnSameTechnicalLayerDependencyRule(moq.Object);

            var result = sut.Apply(type, dependency);

            Assert.That(result, Is.EqualTo(DependencyRuleResult.Ignore));

       
        }

        [Test]
        public void Apply_DependencyInSameComponent_Approve()
        {
            var type = MoqProvider.GetTypeDescriptionMoq("ValidType");

            var otherType = MoqProvider.GetTypeDescriptionMoq("OtherType");

            var dependency = MoqProvider.GetDependencyMoq(otherType);

            var moq = new Mock<IArchRuleExampleTypeRepository>();

            moq.Setup(m => m.GetTypeRepresentation(type.FullName)).Returns(new ArchRuleExampleTypes(null,new ArchRuleModule("Module"),null, true));
            moq.Setup(m => m.GetTypeRepresentation(otherType.FullName)).Returns(new ArchRuleExampleTypes(null,new ArchRuleModule("Module"),null, true));

            var sut = new CrossComponentAccessOnlyOnSameTechnicalLayerDependencyRule(moq.Object);

            var result = sut.Apply(type, dependency);

            Assert.That(result, Is.EqualTo(DependencyRuleResult.Approve));

        }

        [Test]
        public void Apply_DependencyDifferentModulesSameLayer_Approve()
        {
            var type = MoqProvider.GetTypeDescriptionMoq("ValidType");

            var otherType = MoqProvider.GetTypeDescriptionMoq("OtherType");

            var dependency = MoqProvider.GetDependencyMoq(otherType);

            var moq = new Mock<IArchRuleExampleTypeRepository>();

            moq.Setup(m => m.GetTypeRepresentation(type.FullName)).Returns(new ArchRuleExampleTypes(null,new ArchRuleModule("Module"),new ArchRuleTechnicalLayer(ArchRuleTechnicalLayer.BusinessLogic), true));
            moq.Setup(m => m.GetTypeRepresentation(otherType.FullName)).Returns(new ArchRuleExampleTypes(null,new ArchRuleModule("OtherModule"),new ArchRuleTechnicalLayer(ArchRuleTechnicalLayer.BusinessLogic), true));

            var sut = new CrossComponentAccessOnlyOnSameTechnicalLayerDependencyRule(moq.Object);

            var result = sut.Apply(type, dependency);

            Assert.That(result, Is.EqualTo(DependencyRuleResult.Approve));

        }

         [Test]
        public void Apply_DependencyDifferentModulesDifferentLayer_Approve()
        {
            var type = MoqProvider.GetTypeDescriptionMoq("ValidType");

            var otherType = MoqProvider.GetTypeDescriptionMoq("OtherType");

            var dependency = MoqProvider.GetDependencyMoq(otherType);

            var moq = new Mock<IArchRuleExampleTypeRepository>();

            moq.Setup(m => m.GetTypeRepresentation(type.FullName)).Returns(new ArchRuleExampleTypes(null,new ArchRuleModule("Module"),new ArchRuleTechnicalLayer(ArchRuleTechnicalLayer.BusinessLogic), true));
            moq.Setup(m => m.GetTypeRepresentation(otherType.FullName)).Returns(new ArchRuleExampleTypes(null,new ArchRuleModule("OtherModule"),new ArchRuleTechnicalLayer(ArchRuleTechnicalLayer.Data), true));

            var sut = new CrossComponentAccessOnlyOnSameTechnicalLayerDependencyRule(moq.Object);

            var result = sut.Apply(type, dependency);

            Assert.That(result, Is.EqualTo(DependencyRuleResult.Reject));

        }
    }
}