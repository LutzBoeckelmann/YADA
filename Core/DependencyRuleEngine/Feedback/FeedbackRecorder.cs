using System;
using System.Collections.Generic;
using YADA.Core.Analyser;

namespace YADA.Core.DependencyRuleEngine.Feedback
{   
    public class FeedbackRecorder : IFeedbackVisitor
    {
        private class Callback : IDisposable
        {
            private readonly Action m_Callback;

            public Callback(Action callback)
            {
                m_Callback = callback;
            }

            public void Dispose()
            {
                m_Callback();
            }
        }
        List<string> m_RawFeedback = new List<string>();

        private int m_IntentLevel;
        private string m_Intent = "";

        private void IncreaseIntent()
        {
            m_IntentLevel++;
            UpdateIntent();
        }

        private void DecreaseIntent()
        {
            m_IntentLevel--;
            UpdateIntent();
        }

        private void UpdateIntent() 
        {
            m_Intent = "";
            for (int i = 0; i < m_IntentLevel;i++) 
            {
                m_Intent += "  ";
            }
        }

        public IDisposable Context(IDependencyContext context)
        {
            IncreaseIntent();

            m_RawFeedback.Add($"{m_Intent}Context: {context}");

            return new Callback(DecreaseIntent);
        }

        public IDisposable ForbiddenDependency(string dependency)
        {
            IncreaseIntent();

            m_RawFeedback.Add($"{m_Intent}Dependency: {dependency}");

            return new Callback(DecreaseIntent);
        }

        public IDisposable Info(string msg)
        {
            IncreaseIntent();

            m_RawFeedback.Add($"{m_Intent}Info: {msg}");

            return new Callback(DecreaseIntent);
        }

        public IDisposable Type(string type)
        {
            IncreaseIntent();

            m_RawFeedback.Add($"{m_Intent}Type: {type}");

            return new Callback(DecreaseIntent);
        }

        public IDisposable ViolatedRule(string rule)
        {
            IncreaseIntent();

            m_RawFeedback.Add($"{m_Intent}Rule: {rule}");

            return new Callback(DecreaseIntent);
        }

        public IList<string> GetResult() { return m_RawFeedback; }
    }

    public class DependencyContextRecorder : IDependencyContextVisitor<string>
    {
        public string BaseClassDefinition(string name)
        {
            return nameof(BaseClassDefinition);
        }

        public string FieldDefinition(string fieldName)
        {
            return nameof(FieldDefinition);
        }

        public string MethodBodyAccessedFieldType(string methodName, string fieldName)
        {
            return nameof(MethodBodyAccessedFieldType);
        }

        public string MethodBodyCalledMethodParameter(string methodName, string calledMethodFullName)
        {
            return nameof(MethodBodyCalledMethodParameter);
        }

        public string MethodBodyCalledMethodReturnType(string methodName, string calledMethodFullName)
        {
            return nameof(MethodBodyCalledMethodReturnType);
        }

        public string MethodBodyCallMethodAtType(string methodName, string calledMethodFullName)
        {
            return nameof(MethodBodyCallMethodAtType);
        }

        public string MethodBodyReferencedType(string methodName)
        {
            return nameof(MethodBodyReferencedType);
        }

        public string MethodDefinitionLocalVariable(string methodName)
        {
            return nameof(MethodDefinitionLocalVariable);
        }

        public string MethodDefinitionParameter(string methodName)
        {
            return nameof(MethodDefinitionParameter);
        }

        public string MethodDefinitionReturnType(string methodName)
        {
            return nameof(MethodDefinitionReturnType);
        }
    }
}