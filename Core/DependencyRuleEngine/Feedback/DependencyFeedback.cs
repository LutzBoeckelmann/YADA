// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Collections.Generic;
using System.Text;
using YADA.Core.Analyser;

namespace YADA.Core.DependencyRuleEngine.Feedback
{
    public class SimplePrinterDependencyContextVisitor : IDependencyContextVisitor
    {
        private string m_Result = "";
        public string Map(IDependencyContext context) 
        {
            m_Result = string.Empty;
            context.Visit(this);
            return m_Result;
        }

        void IDependencyContextVisitor.BaseClassDefinition(string name)
        {
            m_Result = $"Derives from class {name}";
        }

        void IDependencyContextVisitor.FieldDefinition(string name)
        {
            m_Result = $"Defines field {name}";
        }

        void IDependencyContextVisitor.MethodBodyAccessedFieldType(string name, string m_FieldName)
        {
            m_Result = $"Method {name} access field {m_FieldName}";
        }

        void IDependencyContextVisitor.MethodBodyCalledMethodParameter(string name, string m_CalledMethodFullName)
        {
            m_Result = $"Method {name} accessed type is pararameter at calls method {m_CalledMethodFullName}";
        }

        void IDependencyContextVisitor.MethodBodyCalledMethodReturnType(string name, string m_CalledMethodFullName)
        {
            m_Result = $"Method {name} accessed type is return type of called method {m_CalledMethodFullName}";
        }

        void IDependencyContextVisitor.MethodBodyCallMethodAtType(string name, string m_CalledMethodFullName)
        {
            m_Result = $"Method {name} accessed type by calling method {m_CalledMethodFullName}";
        }

        void IDependencyContextVisitor.MethodBodyReferencedType(string name)
        {
            m_Result = $"Method {name} references the accessed type";
        }

        void IDependencyContextVisitor.MethodDefinitionLocalVariable(string name)
        {
            m_Result = $"Method {name} accessed type is local defined variable";
        }

        void IDependencyContextVisitor.MethodDefinitionParameter(string name)
        {
            m_Result = $"Method {name} accessed type as parameter";
        }

        void IDependencyContextVisitor.MethodDefinitionReturnType(string name)
        {
            m_Result = $"Method {name} returns accessed type";
        }
    }

    public class SimplePrinterGenericDependencyContextVisitor : IDependencyContextVisitor<string>
    {
        public string Map(IDependencyContext context) 
        {
                       
            return context.Visit<string>(this);
        }

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
            return $"Method {name} accessed type is pararameter at calls method {m_CalledMethodFullName}";
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

    internal class DependencyFeedback : IDependencyFeedback
    {
        private readonly List<string> m_Contexted = new List<string>();
        public IDependencyFeedback At(string context)
        {
            m_Contexted.Add(context);
            return this;
        }

        internal void Print(StringBuilder result)
        {
            foreach (var context in m_Contexted)
            {
                
                result.AppendLine($"    At: {context}");
            }
        }
    }
}

