// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using NUnit.Framework;

using YADA.DependencyRuleEngine.Feedback.Recorder;

namespace YADA.DependencyRuleEngine.Test.Feedback.Recorder
{
    [TestFixture]
    public class ContextRecorderTests
    {
        private ContextRecorder m_Sut;

        [SetUp]
        public void SetUp()
        {
            m_Sut = new ContextRecorder();
        }

        [Test]
        public void BaseClassDefinition() 
        {
            var input = "BaseClass";
            var result = m_Sut.BaseClassDefinition();

            Assert.That(result, Is.EqualTo($"{nameof(ContextRecorder.BaseClassDefinition)}"));
        }

        [Test]
        public void FieldDefinition()
        {
            var input = "FieldName";

            var result = m_Sut.FieldDefinition(input);

            Assert.That(result, Is.EqualTo($"{nameof(ContextRecorder.FieldDefinition)}_{input}"));
        }

        [Test]
        public void MethodBodyAccessedFieldType()
        {
            var inputMethod = "inputMethod";
            var inputFieldName = "inputFieldName";
            
            var result = m_Sut.MethodBodyAccessedFieldType(inputMethod, inputFieldName);

            Assert.That(result, Is.EqualTo($"{nameof(ContextRecorder.MethodBodyAccessedFieldType)}_{inputMethod}_{inputFieldName}"));
        }

        [Test]
        public void MethodBodyCalledMethodParameter() 
        {
            var inputMethod = "inputMethod";
            var inputFieldName = "inputFieldName";

            var result = m_Sut.MethodBodyCalledMethodParameter(inputMethod, inputFieldName);

            Assert.That(result, Is.EqualTo($"{nameof(ContextRecorder.MethodBodyCalledMethodParameter)}_{inputMethod}_{inputFieldName}"));
        }

        [Test]
        public void MethodBodyCalledMethodReturnType() 
        {
            var inputMethod = "inputMethod";
            var inputFieldName = "inputFieldName";

            var result = m_Sut.MethodBodyCalledMethodReturnType(inputMethod, inputFieldName);

            Assert.That(result, Is.EqualTo($"{nameof(ContextRecorder.MethodBodyCalledMethodReturnType)}_{inputMethod}_{inputFieldName}"));
        }

        [Test]
        public void MethodBodyCallMethodAtType()
        {
            var inputMethod = "inputMethod";
            var inputFieldName = "inputFieldName";

            var result = m_Sut.MethodBodyCallMethodAtType(inputMethod, inputFieldName);

            Assert.That(result, Is.EqualTo($"{nameof(ContextRecorder.MethodBodyCallMethodAtType)}_{inputMethod}_{inputFieldName}"));
        }

        [Test]
        public void MethodBodyReferencedType()
        {
            var inputMethod = "inputMethod";

            var result = m_Sut.MethodBodyReferencedType(inputMethod);

            Assert.That(result, Is.EqualTo($"{nameof(ContextRecorder.MethodBodyReferencedType)}_{inputMethod}"));
        }

        [Test]
        public void MethodDefinitionLocalVariable()
        {
            var inputMethod = "inputMethod";

            var result = m_Sut.MethodDefinitionLocalVariable(inputMethod);

            Assert.That(result, Is.EqualTo($"{nameof(ContextRecorder.MethodDefinitionLocalVariable)}_{inputMethod}"));
        }

        [Test]
        public void MethodDefinitionParameter()
        {
            var inputMethod = "inputMethod";

            var result = m_Sut.MethodDefinitionParameter(inputMethod);

            Assert.That(result, Is.EqualTo($"{nameof(ContextRecorder.MethodDefinitionParameter)}_{inputMethod}"));
        }

        [Test]
        public void MethodDefinitionReturnType()
        {
            var inputMethod = "inputMethod";

            var result = m_Sut.MethodDefinitionReturnType(inputMethod);

            Assert.That(result, Is.EqualTo($"{nameof(ContextRecorder.MethodDefinitionReturnType)}_{inputMethod}"));
        }

    }
}
