_[KA] add [shields](https://img.shields.io) to show license, version, code coverage ..._
# YADA 

**YADA - Yet Another Dependency Analyser**

The general purpose of YADA is to observe dependencies between classes in C# projects and
provide a simple way to implement automated tests to ensure that any dependency follows the architectural rules.

YADA was created with some simple design rules in mind, which will be explained in the next examples.
_[KA] Add valueadd (battlecard vs ArchUnit, NDepend, ...)_

## First simple example

In my opinion everybody who writes architectural tests needs a full and simple access to any dependency information which is retrievable from the code. Hence the first design goal is to provide the simplest possible interface providing any information needed to create an analyser.
_[KA] Add Diagram here_
Anything needed to analyse the code is placed in the namespace *YADA.Core.Analyser*. Maybe the most imported interface in *YADA* is *ITypeDependency*. It represents a simple C# type and delivers next to the full qualified name a set of dependencies. A dependency is represented by *IDependency*. It delivers the type of the dependency as *ITypeDependency* and some information about the location where the dependency was found.
The ITypeDescription for any type in a set of assembly can simply fetched with the TypeLoader instance by using the method *GetTypes()*.

See the following snippet:

```csharp
    // creating the type loader and load all types from the example dll
    var typeLoader = new TypeLoader(new[] { @"./YADA.Example.dll"});

    // fetch all types 
    var typeDescriptions = typeLoader.GetTypes();

    // iterate over all type for the analyse 
    foreach (var typeDescription in typeDescriptions)
    {
        // access the full qualified name of the current type
        Console.WriteLine($"Full name: {typeDescription.FullName}");
        Console.WriteLine("Dependencies of ");

        //iterate over all dependencies of the current type
        foreach (var typeDependency in typeDescription.Dependencies)
        {
            // access the full qualified name of the dependencies type
            Console.WriteLine($"    - {typeDependency.Type.FullName}");
        }
    }
```
As you can see with under 10 lines of code you can generate some insights about your classes and its dependencies. The snipped was taken fromTest/FirstSimpleExample.cs.

Executing the code produces the following output:

```bash
Full name: Example.StaticProvider2
Dependencies of 
    - System.Int32
    - System.Object
Full name: Example.ClassWithMethodUsingStaticReferences
Dependencies of 
    - System.Object
    - Example.StaticProvider
    - Example.IDependencyInterface
```

_[KA]Please describe how YADA internal work (the dependencies are collected?)_

### Customizing output

If you create an automated test to ensure your architectural rules for dependencies, a red test is not enough to know exactly what happens. It is nice to know that you have introduced a violation, but to fix the issue you need also to know where. Therefor any time YADA founds a reference to a type, the current context will be preserved.
If class A depends on class B, a dependency will be generated. At any point where a reference in class A to class B was found, context information about the occurrence will be added to the dependency.
The IDependencyContext self is a nearly empty class, but it can be visited by IDependencyContextVisitor or IDependencyContextVisitor<T>. If visited the context calls the correct method at the visitor and provides case depending information as parameter. The following snippet should give you a short overview which information are available.

_[KA] Why add full interface here but not **ITypeDescription** or **IDependency**_
```csharp
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
```

## Rule engine

The primary purpose of YADA is to provide a simple way to created automated architecture tests. This tests should ensure that any introduced dependency betweens classes fulfills all architectural requirement. Normally architectural requirements are formulated as a set of rules in plain text. A rule engine, where every rule is a single piece of code with an appropriate name and specific output, is the best way to codify this rule set.

The rule engine self is a general component which can be created and tested once. An implementation is available and can be fetched by instantiating the class YADA.Core.DependencyRuleEngine.DependencyRuleEngine. Your rules will be injected into the engine by parameter.

```csharp
var engine = new DependencyRuleEngine(
    new[] 
    {
        new TypeRule1(),
        new TypeRule2() 
    },
    new[]
    { 
        new DependencyRule1(),
        new DependencyRule2()
    }
);
```

There are two types of rules supported by the engine.

**TypeRules:** implementing the interface *YADA.Core.DependencyRuleEngine.Rules.ITypeRule<T>*

```csharp
public interface ITypeRule<T>
{
    DependencyRuleResult Apply(T type, IFeedbackCollector feedback);
}
```
_[KA] Would **Validate** or **Check** be a better name?_
Type rules check the type self. These kind of rules may be used to reject any type which does not obeys the architectural rules for namespaces and type names at all. Also a type rule may skip types which should not be analysed. In any case for a dependency analyses at least one rule must accept the type.

**DependencyRules:** implementing the interface *YADA.Core.DependencyRuleEngine.Rules.IDependencyRule<T, K>*

```csharp
public interface IDependencyRule<T, K>
{
    DependencyRuleResult Apply(T type, K dependency, IFeedbackCollector feedback);
}
```
_[KA] Would **Validate** or **Check** be a better name?_

A dependency rule checks any dependency of a type. A dependency may be approved, rejected, skipped or ignored. Skipped suppresses further analyses of the dependency and ignore simple has no further effect.

_[KA] please add table with consequences of 4 variants_
The result of any rule is **DependencyRuleResult**. It provides four different result types:

- Approve
- Ignore
- Reject
- Skip
  
### Feedback collector

Like mentioned before without good feedback the best tests are worthless. To support this the FeedbackCollector was introduced. The **IFeedbackCollector** and the associated interfaces resides in the namespace *YADA.DependencyRuleEngine.Feedback*. As root element of any feedback an implementation of IFeedbackCollector must be injected to the rule engine. The analyse procedure follows always the same steps, this is reflected in the feedback interfaces too. For any specific situation a specific feedback interface is available. Adding feedback returns the next needed specific feedback interface. Simpler formulated the interfaces are nested in a way to form a fluent interface. 
_[KA] only use the simpler description_
A code example may explain it better:

```csharp
feebackCollector.AddFeedbackForType("ExampleType")
                .ViolatesRule("SpecificLayerRule")
                .AddInfo("Some additional hints")
                .AddInfo("Some specific infos")
                .At("Member with Name m_SomeMember");
                        
```
_[KA] Feedback Collector is injected into Rule, alone with the Type, why does this need to be added explicitly again?_

A good feedback should not only tell what was violated also where the violation can be found, therefor the example strings above can be enhanced with the **IDependencyContextVisitor<T>**. 

### Testability

Any complex logic needs tests. And creating tests for input data fetched from assemblies is not the right way. So any interface in YADA uses only plain c# types, which provides a simple way to test your application with Fakes. In fact there are simple fake implementations for ITypeDescription and IDependency available.

Testability is realized in the way how the RuleEngine works. Any rule you define is coded in a separate class. And this class is indeed standalone testable.
Any interface used by YADA is simple enough to be mocked easily. Never the less this would lead to a lot of boiler plate code. To overcome this **ITypeRule<T>** and **IDependencyRule<T, K>** do not use **ITypeDefinition** and **IDependencyDefinition** directly, but behind the generic parameter *T* and *K*. So any rule can be tested with domain specific input. To use the arbitrary typed rules in the rule engine the adapter classes **BaseDependencyRule<T, K>** and  **BaseTypeRule<T,K>** should be used. The mapping in the adapter is done by an implementation of **IDependencyRuleInputMapper<T, K>**.


### ArchRuleDemo

An full fletched example is available under **ArchRuleDemo**. It contains specific architectural rules, which are formulated against an architectural model. A set of tests uses this rules and the rule engine to analyse the example dll **ArchRuleExample**.
_[KA] directly link the folder_
_[KA] a full fletched example is nice a reference, to have a realworld minimal example with a step-by-step /how-to implementation of rule, feedback, visitor would be the best_