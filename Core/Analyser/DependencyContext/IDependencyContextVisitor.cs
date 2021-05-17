// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

namespace YADA.Core.Analyser
{
    /// <summary>
    /// The visitor interface accepted by any DependencyContext implementation.
    /// Can be used to retrieve more information about the context of the dependency.
    /// </summary>
    public interface IDependencyContextVisitor
    {
        /// <summary>
        /// The dependency was introduced as field or property.
        /// </summary>
        /// <param name="fieldName">name of the field</param>
        void FieldDefinition(string fieldName);

        /// <summary>
        /// The dependency was introduced as Baseclass or implemented interface.
        /// </summary>
        /// <param name="name">Name of the base class or interface</param>
        void BaseClassDefinition(string name);
        
        /// <summary>
        /// The dependency was introduced as type of a methods parameter
        /// </summary> 
        /// <param name="methodName">name of the method</param>
        void MethodDefinitionParameter(string methodName);
        /// <summary>
        /// The dependency was introduced as type of a local variable within a method.
        /// </summary>
        /// <param name="methodName">name of the method</param>
        void MethodDefinitionLocalVariable(string methodName);
        
        /// <summary>
        /// The dependency was introduced because a defined method returns the dependency's type.
        /// </summary>
        /// <param name="methodName">name of the method</param>
        void MethodDefinitionReturnType(string methodName);
       
        /// <summary>
        /// The dependency was introduced by calling a method at the dependency's type.
        /// </summary>
        /// <param name="methodName">Name of the method where the dependency's method is called</param>
        /// <param name="calledMethodFullName">Name of the called method</param>
        void MethodBodyCallMethodAtType(string methodName, string calledMethodFullName);
        
        /// <summary>
        /// The dependency was introduced by calling a method which uses the dependency's type as return type.
        /// </summary>
        /// <param name="methodName">Name of the method where the is called happens</param>
        /// <param name="calledMethodFullName">Name of the called method</param>
        void MethodBodyCalledMethodReturnType(string methodName, string calledMethodFullName);
        
        /// <summary>
        /// The dependency was introduced by calling a method which uses the dependency's type as return type.
        /// </summary>
        /// <param name="methodName">Name of the method where the is called happens</param>
        /// <param name="calledMethodFullName">Name of the called method</param>
        void MethodBodyCalledMethodParameter(string methodName, string calledMethodFullName);
        
        /// <summary>
        /// The type of the dependency is used in the body of the method.
        /// </summary>
        /// <param name="methodName">Name of the method which access the type</param>
        void MethodBodyReferencedType(string methodName);
        
        /// <summary>
        /// The dependency was introduced because the method access a field of the dependency's type.
        /// Probably the dependency will be found twice. 
        /// </summary>
        /// <param name="methodName">Name of the method</param>
        /// <param name="fieldName">Name of the field</param>
        void MethodBodyAccessedFieldType(string methodName, string fieldName);
    }
    
    /// <summary>
    /// A generic visitor interface accepted by any DependencyContext implementation.
    /// Similar to the normal visitor interface but a value may be returned.
    /// Can be used to retrieve more information about the context of the dependency.
    /// </summary>
    public interface IDependencyContextVisitor<T>
    {
        /// <summary>
        /// The dependency was introduced as field or property.
        /// </summary>
        /// <param name="fieldName">name of the field</param>
        T FieldDefinition(string fieldName);

        /// <summary>
        /// The dependency was introduced as Baseclass or implemented interface.
        /// </summary>
        /// <param name="name">Name of the base class or interface</param>
        T BaseClassDefinition(string name);
        
        /// <summary>
        /// The dependency was introduced as type of a methods parameter
        /// </summary> 
        /// <param name="methodName">name of the method</param>
        T MethodDefinitionParameter(string methodName);
        /// <summary>
        /// The dependency was introduced as type of a local variable within a method.
        /// </summary>
        /// <param name="methodName">name of the method</param>
        T MethodDefinitionLocalVariable(string methodName);
        
        /// <summary>
        /// The dependency was introduced because a defined method returns the dependency's type.
        /// </summary>
        /// <param name="methodName">name of the method</param>
        T MethodDefinitionReturnType(string methodName);
       
        /// <summary>
        /// The dependency was introduced by calling a method at the dependency's type.
        /// </summary>
        /// <param name="methodName">Name of the method where the dependency's method is called</param>
        /// <param name="calledMethodFullName">Name of the called method</param>
        T MethodBodyCallMethodAtType(string methodName, string calledMethodFullName);
        
        /// <summary>
        /// The dependency was introduced by calling a method which uses the dependency's type as return type.
        /// </summary>
        /// <param name="methodName">Name of the method where the is called happens</param>
        /// <param name="calledMethodFullName">Name of the called method</param>
        T MethodBodyCalledMethodReturnType(string methodName, string calledMethodFullName);
        
        /// <summary>
        /// The dependency was introduced by calling a method which uses the dependency's type as return type.
        /// </summary>
        /// <param name="methodName">Name of the method where the is called happens</param>
        /// <param name="calledMethodFullName">Name of the called method</param>
        T MethodBodyCalledMethodParameter(string methodName, string calledMethodFullName);
        
        /// <summary>
        /// The type of the dependency is used in the body of the method.
        /// </summary>
        /// <param name="methodName">Name of the method which access the type</param>
        T MethodBodyReferencedType(string methodName);
        
        /// <summary>
        /// The dependency was introduced because the method access a field of the dependency's type.
        /// Probably the dependency will be found twice. 
        /// </summary>
        /// <param name="methodName">Name of the method</param>
        /// <param name="fieldName">Name of the field</param>
        T MethodBodyAccessedFieldType(string methodName, string fieldName);
    }

}
