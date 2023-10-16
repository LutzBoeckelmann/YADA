// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

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
        string IDependencyContextVisitor<string>.BaseClassDefinition(string name)
        {
           return $"Derives from class {name}";
        }

        string IDependencyContextVisitor<string>.FieldDefinition(string name)
        {
            return $"Defines field {name}";
        }

        string IDependencyContextVisitor<string>.MethodBodyAccessedFieldType(string name, string m_FieldName)
        {
            return $"Method {name} access field {m_FieldName}";
        }

        string IDependencyContextVisitor<string>.MethodBodyCalledMethodParameter(string name, string m_CalledMethodFullName)
        {
            return $"Method {name} accessed type is parameter at calls method {m_CalledMethodFullName}";
        }

        string IDependencyContextVisitor<string>.MethodBodyCalledMethodReturnType(string name, string m_CalledMethodFullName)
        {
            return $"Method {name} accessed type is return type of called method {m_CalledMethodFullName}";
        }

        string IDependencyContextVisitor<string>.MethodBodyCallMethodAtType(string name, string m_CalledMethodFullName)
        {
            return $"Method {name} accessed type by calling method {m_CalledMethodFullName}";
        }

        string IDependencyContextVisitor<string>.MethodBodyReferencedType(string name)
        {
            return $"Method {name} references the accessed type";
        }

         string IDependencyContextVisitor<string>.MethodDefinitionLocalVariable(string name)
        {
            return $"Method {name} accessed type is local defined variable";
        }

        string IDependencyContextVisitor<string>.MethodDefinitionParameter(string name)
        {
            return $"Method {name} accessed type as parameter";
        }

         string IDependencyContextVisitor<string>.MethodDefinitionReturnType(string name)
        {
            return $"Method {name} returns accessed type";
        }
    }
}

