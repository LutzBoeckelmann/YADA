// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Collections.Generic;
using System.Text;
using Mono.Cecil;


namespace YADA.Core.Analyser
{
    /// <summary>
    /// The base idea of IDependencyConext is to provide information about  
    /// where the current type is coupled to the dependency.
    /// </summary>
    internal abstract class BaseContext : IDependencyContext 
    {
        public string Name { get; }

        protected BaseContext(string name) { 
            Name = name;
            AdditionalInfomation = new Dictionary<string, string>();
        }

        public Dictionary<string, string> AdditionalInfomation { get; }

        protected string PrintAdditionalInformation() 
        {
            StringBuilder result = new StringBuilder();
            foreach(var pair in AdditionalInfomation)
             {
                result.Append($"{pair.Key} - {pair.Value}");

            }

            return result.ToString();
        }
    }

    internal class FieldContext : BaseContext
    {
        public FieldContext(string fieldName) : base(fieldName) {        }

        public override string ToString()
        {
            return $"FieldContext {Name}";
        }
    }

    internal class InheritesContext : BaseContext
    {
        

        public InheritesContext(string dependencyName) : base(dependencyName)  { }

         public override string ToString()
        {
            return $"InheritesContext {Name}";
        }
    }

    internal enum MethodDependencyUsageType
    {
        Parameter,
        ReturnType,
        LocalVariable
    }

    internal class MethodDefinitionContext : BaseContext
    {
        public MethodDependencyUsageType m_UsageType;

        public MethodDefinitionContext(string methodName, MethodDependencyUsageType dependencyAccessType): base(methodName)  
        {
            m_UsageType = dependencyAccessType;
        }

         public override string ToString()
        {
            return $"MethodDefinitionContext {Name} as {m_UsageType}";
        }
    }

    internal enum MethodDependencyAccessType 
    {
        CallToMethodDefintion,
        AccessFields,

        CalledMethodReturnValue,

        CalledMethodParameter,

        TypeAccess
    }

    internal class MethodBodyContext : BaseContext
    {
       
        public MethodDependencyAccessType m_UsageType;

        public MethodBodyContext(string methodName, MethodDependencyAccessType dependencyAccessType): base(methodName) 
        {
            m_UsageType = dependencyAccessType;
        }

        public override string ToString()
        {
            return $"MethodBodyContext {Name} as {m_UsageType} ({PrintAdditionalInformation()})";
        }
    }

    /// <summary>
    /// Responsible for retriving all dependencies of the given type.
    /// </summary>
    public class TypeAnalyser
    {
        internal static TypeDescription AnalyseType(TypeDefinition typeDefinition)
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


        private static void AnalyseMethod(TypeDescription result, TypeDefinition typeDefinition)
        {


            // method 
            foreach (var method in typeDefinition.Methods)
            {
                foreach (var parameter in method.Parameters)
                {
                    AddDependency(result, parameter.ParameterType, new MethodDefinitionContext(method.Name, MethodDependencyUsageType.Parameter));
                }

                var returnValue = method.MethodReturnType.ReturnType;

                if (returnValue.FullName != "System.Void")
                {
                    AddDependency(result, returnValue,  new MethodDefinitionContext(method.Name, MethodDependencyUsageType.ReturnType) );
                }

                if (method.HasBody)
                {
                    //Console.WriteLine($"--- {method.FullName} ---");
                    foreach (var localVariable in method.Body.Variables)
                    {
                        AddDependency(result, localVariable.VariableType, new MethodDefinitionContext(method.Name, MethodDependencyUsageType.LocalVariable));
                    }

                    foreach (var line in method.Body.Instructions)
                    {
                        //Console.WriteLine($"{line} ({line.Operand?.GetType().FullName})");

                        if (line.OpCode == Mono.Cecil.Cil.OpCodes.Newobj)
                        {
                            MethodDefinition definition = line.Operand as MethodDefinition;

                            //  AddDependency(result, definition.DeclaringType, repository);
                        }

                        if (line.Operand is MethodDefinition methodDefinition)
                        {
                            var context = new MethodBodyContext(method.Name, MethodDependencyAccessType.CallToMethodDefintion);
                            AddDependency(result, methodDefinition.DeclaringType, context );
                            //Console.WriteLine($"!! {methodDefinition.DeclaringType.FullName}");
                        }

                        if (line.Operand is FieldDefinition fieldDefinition)
                        {
                            // this may not be relevant for the dependency if its a local field, but may be used for deeper informations
                            var context = new MethodBodyContext(method.Name, MethodDependencyAccessType.AccessFields);
                            AddDependency(result, fieldDefinition.FieldType, context);

                            //Console.WriteLine($"!! {fieldDefinition.FieldType.FullName}");
                        }
                        if (line.Operand is TypeDefinition typeDefinition1)
                        {
                            var context = new MethodBodyContext(method.Name, MethodDependencyAccessType.TypeAccess);
                            AddDependency(result, typeDefinition1, context);
                            //Console.WriteLine($"!! {typeDefinition1.FullName}");
                        }
                        if (line.Operand is MethodReference methodReference)
                        {

                            if (methodReference.ReturnType.FullName != "System.Void")
                            {
                                var context = new MethodBodyContext(method.Name, MethodDependencyAccessType.CalledMethodReturnValue);
                                context.AdditionalInfomation.Add("CalledMethod", methodReference.FullName);
                                AddDependency(result, methodReference.ReturnType, context);
                            }

                            foreach (var t in methodReference.Parameters)
                            {
                                var context = new MethodBodyContext(method.Name, MethodDependencyAccessType.CalledMethodParameter);
                                context.AdditionalInfomation.Add("CalledMethod", methodReference.FullName);
                                AddDependency(result, t.ParameterType, context);
                                //Console.WriteLine("References to Parameter " + t.ParameterType.FullName);
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
