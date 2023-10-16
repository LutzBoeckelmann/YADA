// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt


using System;
using System.Linq;
using Moq;
using NUnit.Framework;
using YADA.Analyzer;
using YADA.DependencyRuleEngine.Feedback.Recorder.Recordings;

namespace YADA.DependencyRuleEngine.Test.Feedback.Recorder.Recordings
{
    [TestFixture]
    public class DependencyRecordingTests
    {
        [Test]
        public void DependencyName_FromConstructorArgument()
        {
            var sut = new DependencyRecording("Dependency", null);
            Assert.That(sut.DependencyName, Is.EqualTo("Dependency"));
        }

        [Test]
        public void ReadMode_DeliversValue_FromParent()
        {
            var parentMock = new Mock<IRecording>();
            parentMock.Setup(g => g.ReadMode).Returns(true);
            var sut = new DependencyRecording("Dependency", parentMock.Object);

            Assert.That(sut.ReadMode, Is.True);
        }

        [Test]
        public void Close_Not_Supported() 
        {
            var parentMock = new Mock<IRecording>();

            var sut = new DependencyRecording("Dependency", parentMock.Object);
            Assert.Throws<NotSupportedException>(()=>sut.Closed(Mock.Of<IRecording>()));
        }

        [Test]
        public void Context_RecordedContext_ContextsContainsSerializedContext()
        {
            var parentMock = new Mock<IRecording>();

            var sut = new DependencyRecording("Dependency", parentMock.Object);

            var context = new RecordedContext("Context");
            _ = sut.Context(context);

            Assert.That(sut.Contexts.First().Context, Is.EqualTo("Context"));
        }


        [Test]
        public void Context_GenericContextWithStringVisitor_ContextsContainsVisitResult()
        {
            var visitor = Mock.Of<IDependencyContextVisitor<string>>();
            
            var parentMock = new Mock<IRecording>();
            parentMock.Setup(g => g.ContextVisitor).Returns(visitor);

            var sut = new DependencyRecording("Dependency", parentMock.Object);

            var context = new Mock<IDependencyContext>();
            context.Setup(g => g.Visit(visitor)).Returns("Context");
            
            _ = sut.Context(context.Object);

            Assert.That(sut.Contexts.First().Context, Is.EqualTo("Context"));
        }
    }
}
