// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Collections.Generic;
using Mono.Cecil;

namespace YADA.Core
{

    public interface ITypeDescription 
    {
        
        string FullName { get; }

        IEnumerable<IDependency> Dependencies { get; }

    }

    public interface IDependency 
    {
        
        ITypeDescription Type { get; }
        int Occurrence { get; }
    }


    public class Loader
    {
        public static IEnumerable<TypeDescription> GetTypes(string location) 
        {
            var assembly = AssemblyDefinition.ReadAssembly(location);
        
            foreach (var module in assembly.Modules) 
            {
                foreach(var type in module.GetTypes()) 
                {
                    yield return AnalyseType(type);
                }
            }
        }
        
        internal static TypeDescription AnalyseType(TypeDefinition typeDefinition) 
        {
            var result = new TypeDescription( typeDefinition.FullName);

            //Check fields
            foreach (var field in typeDefinition.Fields) 
            {
                AddDependency(result, field.FieldType);
            }

            // interfaces
            foreach (var implementedInterface in typeDefinition.Interfaces)
            {
                AddDependency(result, implementedInterface.InterfaceType);
            }

            // base class 
            if (typeDefinition.BaseType != null)
            {
                AddDependency(result, typeDefinition.BaseType);
            }

            // method 
            foreach (var method in typeDefinition.Methods) 
            {
                foreach(var parameter in method.Parameters) 
                {
                    AddDependency(result, parameter.ParameterType);
                }

                var returnValue = method.MethodReturnType.ReturnType;

                if(returnValue.FullName != "System.Void") 
                {
                    AddDependency(result, returnValue);
                }

                if (method.HasBody)
                {
                    //Console.WriteLine($"--- {method.FullName} ---");
                    foreach (var localVariable in method.Body.Variables) 
                    {
                        AddDependency(result, localVariable.VariableType);
                    }

                    foreach(var line in method.Body.Instructions) 
                    {   
                        //Console.WriteLine($"{line} ({line.Operand?.GetType().FullName})");

                        if(line.OpCode == Mono.Cecil.Cil.OpCodes.Newobj) 
                        {
                            MethodDefinition definition = line.Operand as MethodDefinition;
                            
                          //  AddDependency(result, definition.DeclaringType, repository);
                        }

                        if(line.Operand is MethodDefinition methodDefinition)  
                        {
                            AddDependency(result, methodDefinition.DeclaringType);
                            //Console.WriteLine($"!! {methodDefinition.DeclaringType.FullName}");
                        }
                        
                        if(line.Operand is FieldDefinition fieldDefinition) 
                        {
                            AddDependency(result,fieldDefinition.FieldType);
                            
                            //Console.WriteLine($"!! {fieldDefinition.FieldType.FullName}");
                        }
                        if(line.Operand is TypeDefinition typeDefinition1) 
                        {
                             AddDependency(result,typeDefinition1);
                            //Console.WriteLine($"!! {typeDefinition1.FullName}");
                        }
                        if(line.Operand is MethodReference methodReference) 
                        {
                                        
                            if(methodReference.ReturnType.FullName != "System.Void") 
                            {
                                AddDependency(result, methodReference.ReturnType);
                            }

                            foreach(var t in methodReference.Parameters)
                            {
                                AddDependency(result,t.ParameterType);
                                //Console.WriteLine("References to Parameter " + t.ParameterType.FullName);
                            }

                        }
                    }
                }

            }

            return result;
        }

        private static void AddDependency(TypeDescription currentType, TypeReference dependency) 
        {
            TypeDescription dependencyDescription;

            var dependencies = GetDependency(dependency);
            
            foreach(var type in dependencies) 
            {
                dependencyDescription = new TypeDescription(type.FullName);
                currentType.AddDependency(dependencyDescription);
            }
            
        }

        private static IEnumerable<TypeReference> GetDependency( TypeReference dependency) 
        {
            List<TypeReference> result = new List<TypeReference>();
            
            if(dependency.IsArray) 
            {
                var arrayType = dependency as ArrayType;
                result.AddRange(GetDependency(arrayType.ElementType));
            }
            else if (dependency.IsGenericInstance) 
            {
                var genericType = GetGeneric(dependency);
                foreach(var type in genericType) 
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

        private static IEnumerable<TypeReference> GetGeneric( TypeReference dependency) 
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
