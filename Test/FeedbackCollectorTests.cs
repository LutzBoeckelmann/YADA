// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt


using System.Text;
using YADA.Core.DependencyRuleEngine.Feedback;
using NUnit.Framework;

namespace YADA.Test
{
    [TestFixture]
    public class TypeFeedbackTests
    {
         [Test]
        public void ViolatesRule_Rule_NotNull( ) 
        {
            TypeFeedback sut = new TypeFeedback();
            var result = sut.ViolatesRule("Rule");
            Assert.NotNull(result);
        }

            [Test]
        public void ViolatesRule_TwoTimesSameType_ReturnsSameValue() 
        {
            TypeFeedback sut = new TypeFeedback();
            var result = sut.ViolatesRule("Rule");
            var secondCallResult = sut.ViolatesRule("Rule");
        
            Assert.That(result, Is.EqualTo(secondCallResult));
        }
        
        [Test]
        public void Print_ViolatesRuleCalledForSeveralRules_ReturnsEachType() 
        {
            TypeFeedback sut = new TypeFeedback();
            sut.ViolatesRule("Rule");
            sut.ViolatesRule("Rule");
            sut.ViolatesRule("OtherRule");
            
            var printer = new ResultCollectorSimplePrinter();

            sut.Explore(printer);

            Assert.That(printer.GetFeedback(), Does.Contain("Rule").And.Contain("OtherRule"));
        }
    }

    [TestFixture]
    public class FeedbackCollectorTests
    {
        [Test]
        public void AddFeedbackForType_Type_ReturnsValue() 
        {
            FeedbackCollector sut = new FeedbackCollector();
            var result = sut.AddFeedbackForType("Type");
            Assert.NotNull(result);
        }
       
        [Test]
        public void AddFeedbackForType_TwoTimesSameType_ReturnsSameValue() 
        {
            FeedbackCollector sut = new FeedbackCollector();
            var result = sut.AddFeedbackForType("Type");
            var secondCallResult = sut.AddFeedbackForType("Type");
            Assert.That(result, Is.EqualTo(secondCallResult));
        }
        
        [Test]
        public void Print_AddFeedbackForTypeSeveralTypes_ReturnsEachType() 
        {
            FeedbackCollector sut = new FeedbackCollector();
            sut.AddFeedbackForType("Type");
            sut.AddFeedbackForType("Type");
            sut.AddFeedbackForType("OtherType");
            var printer = new ResultCollectorSimplePrinter();

            sut.Explore(printer);

            Assert.That(printer.GetFeedback(), Does.Contain("Type").And.Contain("OtherType"));
        }
    }
}
