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
            var type = new ArchRuleExampleType(null, null, null, true);
            var dependencyType = new ArchRuleExampleType(null,null,null, false);

            var sut = new CrossComponentAccessOnlyOnSameTechnicalLayerDependencyRule();

            var result = sut.Apply(type, new ArchRuleExampleDependency(dependencyType));

            Assert.That(result, Is.EqualTo(DependencyRuleResult.Ignore));
        }

        [Test]
        public void Apply_DependencyInSameComponent_Approve()
        {
            var type = new ArchRuleExampleType(null,new ArchRuleModule("Module"),null, true);
            var dependencyType = new ArchRuleExampleType(null, new ArchRuleModule("Module"), null, true);

            var sut = new CrossComponentAccessOnlyOnSameTechnicalLayerDependencyRule();

            var result = sut.Apply(type, new ArchRuleExampleDependency(dependencyType));

            Assert.That(result, Is.EqualTo(DependencyRuleResult.Approve));

        }

        [Test]
        public void Apply_DependencyDifferentModulesSameLayer_Approve()
        {
            var type = new ArchRuleExampleType(null, new ArchRuleModule("Module"), new ArchRuleTechnicalLayer(ArchRuleTechnicalLayer.BusinessLogic), true);
            var dependencyType = new ArchRuleExampleType(null, new ArchRuleModule("OtherModule"), new ArchRuleTechnicalLayer(ArchRuleTechnicalLayer.BusinessLogic), true);

            var sut = new CrossComponentAccessOnlyOnSameTechnicalLayerDependencyRule();

            var result = sut.Apply(type, new ArchRuleExampleDependency(dependencyType));

            Assert.That(result, Is.EqualTo(DependencyRuleResult.Approve));

        }

         [Test]
        public void Apply_DependencyDifferentModulesDifferentLayer_Approve()
        {
            var type = new ArchRuleExampleType(null,new ArchRuleModule("Module"),new ArchRuleTechnicalLayer(ArchRuleTechnicalLayer.BusinessLogic), true);
            var dependencyType = new ArchRuleExampleType(null, new ArchRuleModule("OtherModule"), new ArchRuleTechnicalLayer(ArchRuleTechnicalLayer.Data), true);

            var sut = new CrossComponentAccessOnlyOnSameTechnicalLayerDependencyRule();

            var result = sut.Apply(type, new ArchRuleExampleDependency(dependencyType));

            Assert.That(result, Is.EqualTo(DependencyRuleResult.Reject));

        }
    }
}