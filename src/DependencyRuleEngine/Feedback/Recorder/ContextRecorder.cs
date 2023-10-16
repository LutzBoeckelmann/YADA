// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using YADA.Analyzer;

namespace YADA.DependencyRuleEngine.Feedback.Recorder
{
    public class ContextRecorder : IDependencyContextVisitor<string>
    {
        public string BaseClassDefinition(string name)
        {
            return $"{nameof(BaseClassDefinition)}_{name}";
        }

        public string FieldDefinition(string fieldName)
        {
            return $"{nameof(FieldDefinition)}_{fieldName}";
        }

        public string MethodBodyAccessedFieldType(string methodName, string fieldName)
        {
            return $"{nameof(MethodBodyAccessedFieldType)}_{methodName}_{fieldName}";
        }

        public string MethodBodyCalledMethodParameter(string methodName, string calledMethodFullName)
        {
            return $"{nameof(MethodBodyCalledMethodParameter)}_{methodName}_{calledMethodFullName}";
        }

        public string MethodBodyCalledMethodReturnType(string methodName, string calledMethodFullName)
        {
            return $"{nameof(MethodBodyCalledMethodReturnType)}_{methodName}_{calledMethodFullName}";
        }

        public string MethodBodyCallMethodAtType(string methodName, string calledMethodFullName)
        {
            return $"{nameof(MethodBodyCallMethodAtType)}_{methodName}_{calledMethodFullName}";
        }

        public string MethodBodyReferencedType(string methodName)
        {
            return $"{nameof(MethodBodyReferencedType)}_{methodName}";
        }

        public string MethodDefinitionLocalVariable(string methodName)
        {
            return $"{nameof(MethodDefinitionLocalVariable)}_{methodName}";
        }

        public string MethodDefinitionParameter(string methodName)
        {
            return $"{nameof(MethodDefinitionParameter)}_{methodName}";
        }

        public string MethodDefinitionReturnType(string methodName)
        {
            return $"{nameof(MethodDefinitionReturnType)}_{methodName}";
        }
    }
}