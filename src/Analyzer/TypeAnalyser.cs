// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System;
using System.Collections.Generic;
using Mono.Cecil;

namespace YADA.Analyzer
{
    /// <summary>
    /// Responsible for retrieving all dependencies of the given type.
    /// </summary>
    internal class TypeAnalyser
    {
        private readonly ITypeFilter m_TypeFilter;
        public TypeAnalyser(ITypeFilter typeFilter = null)
        {
            m_TypeFilter = typeFilter;
        }

        /// <summary>
        /// Analyses the given type. 
        /// </summary>
        /// <param name="typeDefinition">The type to analyze as TypeDefinition</param>
        /// <returns>The type a TypeDescription</returns>
        internal TypeDescription AnalyseType(TypeDefinition typeDefinition)
        {
            if(m_TypeFilter != null && m_TypeFilter.IgnoreType(typeDefinition))
            {
                throw new ArgumentException(nameof(typeDefinition), "Do not analyze ignored types");
            }

            var result = new TypeDescription(typeDefinition.FullName, typeDefinition.Module.Assembly.FullName);

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
                    AnalyseMethodBody(result, method);
                }
            }
        }

        private void AnalyseMethodBody(TypeDescription result, MethodDefinition method)
        {
            foreach (var localVariable in method.Body.Variables)
            {
                AddDependency(result, localVariable.VariableType, new MethodDefinitionLocalVariableContext(method.Name));
            }

            foreach (Mono.Cecil.Cil.Instruction line in method.Body.Instructions)
            {
                AnalyseMethodInstruction(result, method, line);
            }
        }

        private void AnalyseMethodInstruction(TypeDescription result, MethodDefinition method, Mono.Cecil.Cil.Instruction line) 
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
                AnalyseMethodReference(result, method, methodReference);
            }
        }

        private void AnalyseMethodReference(TypeDescription result, MethodDefinition method, MethodReference methodReference)
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

        private void AddDependency(TypeDescription currentType, TypeReference dependency, IDependencyContext context)
        {
            if (m_TypeFilter != null && m_TypeFilter.IgnoreTypeAsDependency(dependency.Resolve()))
            {
                return;
            }

            var dependencies = GetDependency(dependency);
                            
            foreach (var type in dependencies)
            {
                var d = type.Module.Assembly.FullName;
                var dependencyDescription = new TypeDescription(type.FullName, d);
                currentType.AddDependency(dependencyDescription, context);
            }
        }

        private IEnumerable<TypeReference> GetDependency(TypeReference dependency)
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

        private IEnumerable<TypeReference> AnalyseGeneric(TypeReference dependency)
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
