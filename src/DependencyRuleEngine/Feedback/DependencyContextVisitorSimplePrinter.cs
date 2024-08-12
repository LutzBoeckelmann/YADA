// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Net.Http.Headers;
using YADA.Analyzer;

namespace YADA.DependencyRuleEngine.Feedback
{   
    /// <summary>
    /// A simple implementation of IDependencyContextVisitor<string>. Provides some common feedback
    /// for specific locations where dependencies where found. Primary for demonstration purposes,
    /// but fully usable if the messages fits for your use case.
    /// </summary>
    public class GenericDependencyContextVisitorSimplePrinter : IDependencyContextVisitor<string>
    {
        string IDependencyContextVisitor<string>.BaseClassDefinition()
        {
           return $"Derives from dependency";
        }

        string IDependencyContextVisitor<string>.FieldDefinition(string fieldName)
        {
            return $"Defines field {fieldName}";
        }

        string IDependencyContextVisitor<string>.MethodBodyAccessedFieldType(string methodName, string fieldName)
        {
            return $"Method {methodName} access field {fieldName}";
        }

        public string ClassAttributeContext()
        {
            return "Uses the dependency as class Attribute";
        }

        public string MethodAttributeContext(string methodName)
        {
            return $"The dependency is used as an attribute at the method {methodName}";
        }

        public string FieldAttribute(string fieldAttributeName)
        {
            return $"The dependency is used as an attribute at the field  or property {fieldAttributeName}";
        }

        string IDependencyContextVisitor<string>.MethodBodyCalledMethodParameter(string methodName, string calledMethodFullName)
        {
            return $"Method {methodName} accessed type is parameter at calls method {calledMethodFullName}";
        }

        string IDependencyContextVisitor<string>.MethodBodyCalledMethodReturnType(string methodName, string calledMethodFullName)
        {
            return $"Method {methodName} accessed type is return type of called method {calledMethodFullName}";
        }

        string IDependencyContextVisitor<string>.MethodBodyCallMethodAtType(string methodName, string calledMethodFullName)
        {
            return $"Method {methodName} accessed type by calling method {calledMethodFullName}";
        }

        string IDependencyContextVisitor<string>.MethodBodyReferencedType(string methodName)
        {
            return $"Method {methodName} references the accessed type";
        }

         string IDependencyContextVisitor<string>.MethodDefinitionLocalVariable(string methodName)
        {
            return $"Method {methodName} accessed type is local defined variable";
        }

        string IDependencyContextVisitor<string>.MethodDefinitionParameter(string methodName)
        {
            return $"Method {methodName} accessed type as parameter";
        }

         string IDependencyContextVisitor<string>.MethodDefinitionReturnType(string methodName)
        {
            return $"Method {methodName} returns accessed type";
        }
    }
}

