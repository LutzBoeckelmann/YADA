// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Linq;
using NUnit.Framework;
using YADA.Core;

namespace YADA.Test
{
    [TestFixture]
    public class SimpleStringCollectionFeedbackSetTests
    {
        [Test]
        public void Messages_AfterAddFeedback_ContainsExpectedValues() 
        {
            SimpleStringCollectionFeedbackSet sut = new SimpleStringCollectionFeedbackSet();
            sut.AddFeedback("Rule", "Type", "Message");
            Assert.That(sut.Messages, Has.Exactly(1).Items);
            Assert.That(sut.Messages.First(), Contains.Substring("Rule").And.Contains("Type").And.Contains("Message"));
        }

        
        [Test]
        public void Messages_AddViolatedRuleFeedback_ContainsExpectedValues() 
        {
            SimpleStringCollectionFeedbackSet sut = new SimpleStringCollectionFeedbackSet();
            using (var d= sut.AddViolatedRuleFeedback("Rule", "Type", "Message")) {

                d.Add("nextMessage");
            }

            var result = sut.Messages.ToArray();
            Assert.That(result, Has.Exactly(2).Items);
            Assert.That(result[0], Contains.Substring("Rule").And.Contains("Type").And.Contains("Message"));
            Assert.That(result[1], Contains.Substring("nextMessage"));
        }

        [Test]
        public void Messages_AddSeveralViolations_CorrectOrder() 
        {
            SimpleStringCollectionFeedbackSet sut = new SimpleStringCollectionFeedbackSet();
            var d= sut.AddViolatedRuleFeedback("Rule", "Type", "Message");
            sut.AddFeedback("Other", "Other2", "Other3");

            d.Add("nextMessage");

            d.Dispose();
            var result = sut.Messages.ToArray();
            Assert.That(result, Has.Exactly(3).Items);
            Assert.That(result[0], Contains.Substring("Rule").And.Contains("Type").And.Contains("Message"));
            Assert.That(result[1], Contains.Substring("nextMessage"));
            Assert.That(result[2], Contains.Substring("Other"));
        }
    }
}