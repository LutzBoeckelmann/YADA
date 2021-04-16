// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

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
            var type = MoqProvider.GetTypeDescriptionMoq("ValidType");

            var moq = new Mock<IArchRuleExampleTypeRepository>();

            moq.Setup(m => m.GetTypeRepresentation("ValidType")).Returns(new ArchRuleExampleTypes(null,null,null, true));

            var sut = new CorrectNamespaceTypeRule(moq.Object);

            var result = sut.Apply(type);

            Assert.That(result, Is.EqualTo(DependencyRuleResult.Approve));
        }
        
        [Test]
        public void Apply_InValidType_Reject() 
        {
             
            var invalidType = MoqProvider.GetTypeDescriptionMoq("InValidType");

            var moq = new Mock<IArchRuleExampleTypeRepository>();

            moq.Setup(m => m.GetTypeRepresentation("InValidType")).Returns(new ArchRuleExampleTypes(null,null,null, false));


            var sut = new CorrectNamespaceTypeRule(moq.Object);

            var result = sut.Apply(invalidType);

            Assert.That(result, Is.EqualTo(DependencyRuleResult.Reject));
        }
    }
}