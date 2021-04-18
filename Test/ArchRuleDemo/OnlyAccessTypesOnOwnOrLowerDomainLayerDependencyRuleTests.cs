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
            var type = new ArchRuleExampleType(null,null,null, true);

            var dependencyInvalidType = new ArchRuleExampleType(null,null,null, false);

            var sut = new OnlyAccessTypesOnOwnOrLowerDomainLayerDependencyRule();

            var result = sut.Apply(type, new ArchRuleExampleDependency(dependencyInvalidType));

            Assert.That(result, Is.EqualTo(DependencyRuleResult.Ignore));

        }

        [Test]
        public void Apply_ValidDependencyInSameLevel_Approve() 
        {
            var type = new ArchRuleExampleType(new ArchRuleDomainLayer(ArchRuleDomainLayer.Core),null,null, true);

            var otherValidType = new ArchRuleExampleType(new ArchRuleDomainLayer("Core"),null,null, true);
            
            var sut = new OnlyAccessTypesOnOwnOrLowerDomainLayerDependencyRule();

            var result = sut.Apply(type, new ArchRuleExampleDependency(otherValidType));

            Assert.That(result, Is.EqualTo(DependencyRuleResult.Approve));
        }

        [Test]
        public void Apply_ValidDependencyOnLowerLevel_Approve() 
        {
            var type = new ArchRuleExampleType(new ArchRuleDomainLayer(ArchRuleDomainLayer.Extentions), null, null, true);
            var otherValidType = new ArchRuleExampleType(new ArchRuleDomainLayer(ArchRuleDomainLayer.Infrastructure),null,null, true);

            var sut = new OnlyAccessTypesOnOwnOrLowerDomainLayerDependencyRule();

            var result = sut.Apply(type, new ArchRuleExampleDependency(otherValidType));

            Assert.That(result, Is.EqualTo(DependencyRuleResult.Approve));
        }
    }
}