[![Maintainability](https://api.codeclimate.com/v1/badges/113627dff37e739515de/maintainability)](https://codeclimate.com/github/LutzBoeckelmann/YADA/maintainability)

<a href="https://scan.coverity.com/projects/lutzboeckelmann-yada">
  <img alt="Coverity Scan Build Status"
       src="https://scan.coverity.com/projects/23841/badge.svg"/>
</a>
# YADA 

**YADA - Yet Another Dependency Analyser**

The general purpose of YADA is to observe dependencies between classes in C# projects and
provide a simple way to implement automated tests to ensure that any dependency follows the architectural rules.

YADA was created with some simple design rules in mind, which will be explained in the next examples.

## First simple example

In my opinion everybody who writes architectural tests needs a full and simple access to any dependency information which is retrievable from the code. Hence the first design goal is to provide the simplest possible interface providing any information needed to create an analyser.

```plantuml

class TypeLoader {
    IEnumerable<string> AssemblyLocations
    IEnumerable<ITypeDescription> GetTypes()
}
interface ITypeDependency {
    string AssemblyName
    string FullQualifiedTypeName
}
interface IDependency {
    ITypeDescription Type
    IEnumerable<IDependencyContext> Context
}

ITypeDependency *- IDependency
TypeLoader ..> ITypeDependency : Creates 

```

Any think needed to analyse the code is placed in the namespace *YADA.Core.Analyser*. Maybe the most imported interface in *YADA* is *ITypeDependency*. It represents a simple C# type and delivers next to the full qualified name a set of dependencies. A dependency is represented by *IDependency*. It delivers the type of the dependency as *ITypeDependency* and some information about the location where the dependency was found.
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
...
Full name: Example.StaticProvider2
Dependencies of 
    - System.Int32
    - System.Object
Full name: Example.ClassWithMethodUsingStaticReferences
Dependencies of 
    - System.Object
    - Example.StaticProvider
    - Example.IDependencyInterface
...
```

### How it works

Internally *YADA* uses the great [*Mono.Cecil*](https://www.mono-project.com/docs/tools+libraries/libraries/Mono.Cecil/) library to analyse the given assemblies. Each type found within the assemblies will be analysed by the *TypeAnalyse*.

Dependencies will be searched at the following ways:

* base class
* implemented interfaces
* members and properties
* parameters and return values 
* casts and created objects within the methods
  
Any found dependency will be added to the ITypeDescription together with the IDependencyContext containing information about the occurrence. Any needed information will be retrieved during this step and stored in the result set.

### Testability as first class citizen

Any complex logic needs tests. And creating tests for input data fetched from assemblies is not the right way. So any interface in YADA uses only plain c# types, which provides a simple way to test your application with Fakes. In fact there are simple fake implementations for ITypeDescription and IDependency available.

### Customizing output

If you create an automated test to ensure your architectural rules for dependencies, a red test is not enough to know exactly what happens. It is nice to know that you have introduced a violation, but to fix the issue you need also to know where. To archive this any time *YADA* founds a reference to a type, the current context will be preserved.
If class A depends on class B, a dependency will be generated. At any point where a reference in class A to class B was found, context information about the occurrence will be added to the dependency.
The *IDependencyContext* self is a nearly empty class, but it can be visited by IDependencyContextVisitor. If visited the context calls the correct method at the visitor and provides case depending information as parameter. An specialized implementation of *IDependencyContext* uses the provided information to generate an output fitting the current situation.

## Automate architectural rules

With the Core mechanisms of *YADA* described above it is possible to create own automated tests. A way to do this is provided with the rule engine, which can be found under . [*YADA.DependencyRuleEngine*](./core/DependencyRuleEngine/Readme.md).

