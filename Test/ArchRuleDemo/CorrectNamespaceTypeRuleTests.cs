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

            var result = sut.Apply(new ArchRuleExampleType("", null,null,null, true), new Mock<IFeedbackSet>().Object);

            Assert.That(result, Is.EqualTo(DependencyRuleResult.Approve));
        }

        public void Apply_ValidType_NoFeedback() 
        {
            var feedback = new Mock<IFeedbackSet>();
            feedback.Setup(s => s.AddFeedback(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));
            feedback.Setup(s => s.AddViolatedRuleFeedback(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Mock.Of<IViolatedRuleFeedback>());
            var sut = new CorrectNamespaceTypeRule();

            var result = sut.Apply(new ArchRuleExampleType("", null,null,null, true), feedback.Object);

            feedback.Verify(s => s.AddFeedback(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            feedback.Verify(s => s.AddViolatedRuleFeedback(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }
        
        [Test]
        public void Apply_InValidType_ExpectedFeedback() 
        {
            var violationFeedback = new Mock<IViolatedRuleFeedback>();
            var feedback = new Mock<IFeedbackSet>();
  
            feedback.Setup(s => s.AddViolatedRuleFeedback(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(violationFeedback.Object);
          
            var sut = new CorrectNamespaceTypeRule();

            var result = sut.Apply(new ArchRuleExampleType("",new ArchRuleDomainLayer(""),null,new ArchRuleTechnicalLayer(""), false), feedback.Object);

            feedback.Verify(s => s.AddViolatedRuleFeedback(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            violationFeedback.Verify(s => s.Add(It.IsAny<string>()), Times.Exactly(2));
            violationFeedback.Verify(s => s.Dispose(), Times.Once);
        }

        [Test]
        public void Apply_InValidType_Reject() 
        {
            var violationFeedback = new Mock<IViolatedRuleFeedback>();
            var feedback = new Mock<IFeedbackSet>();
  
            feedback.Setup(s => s.AddViolatedRuleFeedback(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(violationFeedback.Object);
          
            var sut = new CorrectNamespaceTypeRule();

            var result = sut.Apply(new ArchRuleExampleType("",new ArchRuleDomainLayer(""),null,new ArchRuleTechnicalLayer(""), false), feedback.Object);

            Assert.That(result, Is.EqualTo(DependencyRuleResult.Reject));
        }
    }
}
