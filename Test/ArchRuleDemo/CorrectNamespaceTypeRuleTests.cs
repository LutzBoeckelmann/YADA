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
            var sut = new CorrectNamespaceTypeRule();

            var result = sut.Apply(new ArchRuleExampleType(null,null,null, true));

            Assert.That(result, Is.EqualTo(DependencyRuleResult.Approve));
        }
        
        [Test]
        public void Apply_InValidType_Reject() 
        {

            var sut = new CorrectNamespaceTypeRule();

            var result = sut.Apply(new ArchRuleExampleType(null,null,null, false));

            Assert.That(result, Is.EqualTo(DependencyRuleResult.Reject));
        }
    }
}