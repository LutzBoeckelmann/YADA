// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt


using System.Text;
using NUnit.Framework;
using YADA.Core.DependencyRuleEngine.Impl;

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

            var resultCollector = new StringBuilder();
            sut.Print(resultCollector);
            Assert.That(resultCollector.ToString(), Contains.Substring("Rule").And.ContainsSubstring("OtherRule"));
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

            var result = sut.Print().ToString();
            Assert.That(result, Contains.Substring("Type").And.ContainsSubstring("OtherType"));
        }
    }
}
