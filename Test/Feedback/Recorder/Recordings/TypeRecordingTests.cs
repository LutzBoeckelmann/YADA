// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt


using System;
using Moq;
using NUnit.Framework;
using YADA.Core.DependencyRuleEngine.Feedback;

namespace YADA.Test.Feedback.Recorder.Recordings
{
    [TestFixture]
    public class TypeRecordingTests
    {
        [Test]
        public void TypeNameProperty_CreatedFromConstructor() 
        {
            TypeRecording sut = new TypeRecording("TypeName", null);
            Assert.That(sut.TypeName, Is.EqualTo("TypeName"));
            
        }

        [Test]
        public void Closed_NoActiveRule_Throws() 
        {
            TypeRecording sut = new TypeRecording("TypeName", null);
            Assert.Throws<NotSupportedException>(() => sut.Closed(Mock.Of<IRecording>()));
        }


        [Test]
        public void Closed_UnknownRule_Throws()
        {
            TypeRecording sut = new TypeRecording("TypeName", null);
            var rule = sut.ViolatedRule("Rule");

            Assert.Throws<NotSupportedException>(() => sut.Closed(Mock.Of<IRecording>()));
        }


        [Test]
        public void ReadMode_Returns_ParentReadMode()
        {
            var parentMock = new Mock<IRecording>();
            parentMock.Setup(g => g.ReadMode).Returns(true);
            TypeRecording sut = new TypeRecording("TypeName", parentMock.Object);
            Assert.That(sut.ReadMode, Is.EqualTo(true));
        }

        [Test]
        public void Dispose_CallsClose_AtParent()
        {
            var parentMock = new Mock<IRecording>();
            
            TypeRecording sut = new TypeRecording("TypeName", parentMock.Object);
            
            sut.Dispose();

            parentMock.Verify(g => g.Closed(sut));
        }

        [Test]
        public void ViolatesRule_SecondRuleNoReadMode_Throws()
        {
            var parentMock = new Mock<IRecording>();
            parentMock.Setup(g => g.ReadMode).Returns(false);
            TypeRecording sut = new TypeRecording("TypeName", parentMock.Object);
            sut.ViolatedRule("NotDisposedRule");
            Assert.Throws<NotSupportedException>(()=>sut.ViolatedRule("UnexpectedRule"));
        }
    }
}
