// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt


using System;
using Moq;
using NUnit.Framework;

using YADA.DependencyRuleEngine.Feedback.Recorder.Recordings;

namespace YADA.DependencyRuleEngine.Test.Feedback.Recorder.Recordings
{
    [TestFixture]
    public class ContextRecordingTests
    {
        [Test]
        public void ContextProperty_Returns_ContextFromConstructorArg() 
        {
            var sut = new ContextRecording("Context", null);

            Assert.That(sut.Context, Is.EqualTo("Context"));
        }

        [Test]
        public void ReadMode_DeliversValue_FromParent()
        {
            var parentMock = new Mock<IRecording>();
            parentMock.Setup(x => x.ReadMode).Returns(true);
            var sut = new ContextRecording("Context", parentMock.Object);

            Assert.That(sut.ReadMode, Is.True);
        }

        [Test]
        public void Close_NotSupported()
        {
            var sut = new ContextRecording("Context", null);
            Assert.Throws<NotSupportedException>(()=>sut.Closed(Mock.Of<IRecording>()));
        }

        [Test]
        public void Dispose_DoesNot_Inform_Parent()
        {
            var parentMock = new Mock<IRecording>(MockBehavior.Strict);
            
            var sut = new ContextRecording("Context", parentMock.Object);

            sut.Dispose();
        }


    }
}
