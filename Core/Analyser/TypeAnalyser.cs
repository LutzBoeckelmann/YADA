// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Collections.Generic;
using Mono.Cecil;

namespace YADA.Core.Analyser
{
    internal class FieldContext : IDependencyContext
    {
        private readonly string m_Name;
        public FieldContext(string fieldName) 
        {   
            m_Name = fieldName;
        }
        public void Visit(IDependencyContextVisitor visitor)
        {
            visitor.FieldDefinition(m_Name);
        }

        public T Visit<T>(IDependencyContextVisitor<T> visitor) 
        {
            return visitor.FieldDefinition(m_Name);
        }
    }

    internal class InheritesContext : IDependencyContext
    {
        private readonly string m_Name;

        public InheritesContext(string dependencyName) 
        {   
            m_Name = dependencyName;
        }
        public  void Visit(IDependencyContextVisitor visitor)
        {
            visitor.BaseClassDefinition(m_Name);
        }

        public T Visit<T>(IDependencyContextVisitor<T> visitor) 
        {
            return visitor.BaseClassDefinition(m_Name);
        }
    }

    internal class MethodDefinitionParameterContext : IDependencyContext
    {
        private readonly string m_Name;

        public MethodDefinitionParameterContext(string name) 
        {
            m_Name = name;
        }

        public void Visit(IDependencyContextVisitor visitor)
        {
            visitor.MethodDefinitionParameter(m_Name);
        }

        public T Visit<T>(IDependencyContextVisitor<T> visitor) 
        {
            return visitor.MethodDefinitionParameter(m_Name);
        }
    }

    internal class MethodDefinitionReturnTypeContext : IDependencyContext
    {
        private readonly string m_Name;
        public MethodDefinitionReturnTypeContext(string name)   
        {
            m_Name = name;
        }

        public void Visit(IDependencyContextVisitor visitor)
        {
            visitor.MethodDefinitionReturnType(m_Name);
        }

        public T Visit<T>(IDependencyContextVisitor<T> visitor) 
        {
            return visitor.MethodDefinitionReturnType(m_Name);
        }
    }

    internal class MethodDefinitionLocalVariableContext : IDependencyContext
    {
        private readonly string m_Name;
        public MethodDefinitionLocalVariableContext(string name)   
        {
            m_Name = name;
        }
        
        public void Visit(IDependencyContextVisitor visitor)
        {
            visitor.MethodDefinitionLocalVariable(m_Name);
        }

        public T Visit<T>(IDependencyContextVisitor<T> visitor) 
        {
            return visitor.MethodDefinitionLocalVariable(m_Name);
        }
    }

    internal class MethodBodyFieldReferenceContext : IDependencyContext 
    {
        private readonly string m_MethodName;
        private readonly string m_FieldName;
        
        public MethodBodyFieldReferenceContext(string methodName, string fieldName)
        {
            m_MethodName = methodName;
            m_FieldName = fieldName;
        }

        public void Visit(IDependencyContextVisitor visitor)
        {
            visitor.MethodBodyAccessedFieldType(m_MethodName, m_FieldName);
        }

        public T Visit<T>(IDependencyContextVisitor<T> visitor) 
        {
            return visitor.MethodBodyAccessedFieldType(m_MethodName, m_FieldName);
        }
    }

    internal class MethodBodyTypeReferenceContext : IDependencyContext
    {
        private readonly string m_MethodName;
        public MethodBodyTypeReferenceContext(string name) {
            m_MethodName = name;
        }

        public  void Visit(IDependencyContextVisitor visitor)
        {
            visitor.MethodBodyReferencedType(m_MethodName);
        }

        public T Visit<T>(IDependencyContextVisitor<T> visitor) 
        {
            return visitor.MethodBodyReferencedType(m_MethodName);
        }
    }

    internal class MethodBodyCallMethodAtType : IDependencyContext
    {
        private readonly string m_MethodName;

        private readonly string m_CalledMethodFullName;

        public MethodBodyCallMethodAtType(string name, string calledMethodFullName)
        {
            m_MethodName = name;
            m_CalledMethodFullName = calledMethodFullName;
        }

        public  void Visit(IDependencyContextVisitor visitor)
        {
            visitor.MethodBodyCallMethodAtType(m_MethodName, m_CalledMethodFullName);
        }

        public T Visit<T>(IDependencyContextVisitor<T> visitor) 
        {
            return visitor.MethodBodyCallMethodAtType(m_MethodName, m_CalledMethodFullName);
        }
    }

    internal class MethodBodyCalledMethodParameterContext : IDependencyContext
    {
        private readonly string m_MethodName;
        private readonly string m_CalledMethodFullName;

        public MethodBodyCalledMethodParameterContext(string name, string calledMethodFullName)
        {
            m_MethodName = name;
            m_CalledMethodFullName = calledMethodFullName;
        }

        public void Visit(IDependencyContextVisitor visitor)
        {
            visitor.MethodBodyCalledMethodParameter(m_MethodName, m_CalledMethodFullName);
        }

        public T Visit<T>(IDependencyContextVisitor<T> visitor) 
        {
            return visitor.MethodBodyCalledMethodParameter(m_MethodName, m_CalledMethodFullName);
        }
    }

    internal class MethodBodyCalledMethodReturnTypeContext : IDependencyContext
    {
        private readonly string m_MethodName;
        private readonly string m_CalledMethodFullName;

        public MethodBodyCalledMethodReturnTypeContext(string name, string calledMethodFullName) 
        {
            m_MethodName = name;
            m_CalledMethodFullName = calledMethodFullName;
        }

        public void Visit(IDependencyContextVisitor visitor)
        {
            visitor.MethodBodyCalledMethodReturnType(m_MethodName, m_CalledMethodFullName);
        }

        public T Visit<T>(IDependencyContextVisitor<T> visitor) 
        {
            return visitor.MethodBodyCalledMethodReturnType(m_MethodName, m_CalledMethodFullName);
        }
    }

    /// <summary>
    /// Responsible for retriving all dependencies of the given type.
    /// </summary>
    public class TypeAnalyser
    {
        internal TypeDescription AnalyseType(TypeDefinition typeDefinition)
        {
            var result = new TypeDescription(typeDefinition.FullName);

            //Check fields
            foreach (var field in typeDefinition.Fields)
            {
                AddDependency(result, field.FieldType, new FieldContext(field.Name));
            }

            // interfaces
            foreach (var implementedInterface in typeDefinition.Interfaces)
            {
                AddDependency(result, implementedInterface.InterfaceType, new InheritesContext(""));
            }

            // base class 
            if (typeDefinition.BaseType != null)
            {
                AddDependency(result, typeDefinition.BaseType, new InheritesContext(""));
            }

            AnalyseMethod(result, typeDefinition);

            return result;
        }

        private void AnalyseMethod(TypeDescription result, TypeDefinition typeDefinition)
        {
            // method 
            foreach (var method in typeDefinition.Methods)
            {
                foreach (var parameter in method.Parameters)
                {
                    AddDependency(result, parameter.ParameterType, new MethodDefinitionParameterContext(method.Name));
                }

                var returnValue = method.MethodReturnType.ReturnType;

                if (returnValue.FullName != "System.Void")
                {
                    AddDependency(result, returnValue,  new MethodDefinitionReturnTypeContext(method.Name));
                }

                if (method.HasBody)
                {
                    foreach (var localVariable in method.Body.Variables)
                    {
                        AddDependency(result, localVariable.VariableType, new MethodDefinitionLocalVariableContext(method.Name));
                    }

                    foreach (var line in method.Body.Instructions)
                    {
                        if (line.Operand is FieldDefinition fieldDefinition)
                        {
                            // this may not be relevant for the dependency if its a local field, but may be used for deeper informations
                            var context = new MethodBodyFieldReferenceContext(method.Name, fieldDefinition.FullName);
                            AddDependency(result, fieldDefinition.FieldType, context);
                        }

                        if (line.Operand is TypeDefinition accessTypeDefinition)
                        {
                            var context = new MethodBodyTypeReferenceContext(method.Name);
                            AddDependency(result, accessTypeDefinition, context);
                        }

                        if (line.Operand is MethodDefinition methodDefinition)
                        {
                            var context = new MethodBodyCallMethodAtType(method.Name, methodDefinition.FullName);
                            AddDependency(result, methodDefinition.DeclaringType, context);
                        }
                        
                        if (line.Operand is MethodReference methodReference)
                        {

                            if (methodReference.ReturnType.FullName != "System.Void")
                            {
                                var context = new MethodBodyCalledMethodReturnTypeContext(method.Name, methodReference.FullName);
                                AddDependency(result, methodReference.ReturnType, context);
                            }

                            foreach (var t in methodReference.Parameters)
                            {
                                var context = new MethodBodyCalledMethodParameterContext(method.Name, methodReference.FullName);
                                AddDependency(result, t.ParameterType, context);
                            }
                        }
                    }
                }

            }
        }
        private static void AddDependency(TypeDescription currentType, TypeReference dependency, IDependencyContext context)
        {
            TypeDescription dependencyDescription;

            var dependencies = GetDependency(dependency);

            foreach (var type in dependencies)
            {
                dependencyDescription = new TypeDescription(type.FullName);
                currentType.AddDependency(dependencyDescription, context);
            }

        }

        private static IEnumerable<TypeReference> GetDependency(TypeReference dependency)
        {
            List<TypeReference> result = new List<TypeReference>();

            if (dependency.IsArray)
            {
                var arrayType = dependency as ArrayType;
                result.AddRange(GetDependency(arrayType.ElementType));
            }
            else if (dependency.IsGenericInstance)
            {
                var genericType = AnalyseGeneric(dependency);
                foreach (var type in genericType)
                {
                    result.AddRange(GetDependency(type));
                }
            }
            else
            {
                result.Add(dependency);
            }

            return result;
        }

        private static IEnumerable<TypeReference> AnalyseGeneric(TypeReference dependency)
        {
            List<TypeReference> result = new List<TypeReference>();

            if (dependency is GenericInstanceType generic)
            {
                result.Add(generic.ElementType);
                result.AddRange(generic.GenericArguments);
            }

            return result;

        }

    }
}
