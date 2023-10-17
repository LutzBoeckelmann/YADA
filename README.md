[![Maintainability](https://api.codeclimate.com/v1/badges/113627dff37e739515de/maintainability)](https://codeclimate.com/github/LutzBoeckelmann/YADA/maintainability)
[![CodeFactor](https://www.codefactor.io/repository/github/lutzboeckelmann/yada/badge)](https://www.codefactor.io/repository/github/lutzboeckelmann/yada)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=LutzBoeckelmann_YADA&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=LutzBoeckelmann_YADA)

# YADA 

**YADA - Yet Another Dependency Analyzer**

The general purpose of YADA is to observe dependencies between classes in C# projects and
provide a simple way to implement automated tests to ensure that any dependency follows the architectural rules.

YADA was created with some simple design rules in mind, which will be explained in the next examples.

## First simple example

In my opinion everybody who writes architectural tests needs a full and simple access to any dependency information which is retrievable from the code. Hence the first design goal is to provide the simplest possible interface providing any information needed to create an analyzer.

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

Any think needed to analyze the code is placed in the namespace *YADA.Analyzer*. Maybe the most imported interface in *YADA* is *ITypeDependency*. It represents a simple C# type and delivers next to the full qualified name a set of dependencies. A dependency is represented by *IDependency*. It delivers the type of the dependency as *ITypeDependency* and some information about the location where the dependency was found.
The ITypeDescription for any type in a set of assembly can simply fetched with the TypeLoader instance by using the method *GetTypes()*.

See the following snippet:

```csharp
    // creating the type loader and load all types from the example dll
    var typeLoader = new TypeLoader(new[] { @"./YADA.Example.dll"});

    // fetch all types 
    var typeDescriptions = typeLoader.GetTypes();

    // iterate over all type for the analyze 
    foreach (var typeDescription in typeDescriptions)
    {
        // access the full qualified name of the current type
        Console.WriteLine($"Type: {typeDescription.FullName}");
        Console.WriteLine("  Dependencies:");

        //iterate over all dependencies of the current type
        foreach (var typeDependency in typeDescription.Dependencies)
        {
            // access the full qualified name of the dependencies type
            Console.WriteLine($"    - {typeDependency.Type.FullName}");
        }
    }
```
As you can see with under 10 lines of code you can generate some insights about your classes and its dependencies. The snipped was taken from AnalyzerTests/FirstSimpleExample.cs.

Executing the code produces the following output:
```bash
...
Type: Example.Example1
  Dependencies:
    - Example.Dependency1
    - System.Object
Type: Example.Dependency1
  Dependencies:
    - System.Object
Type: Example.Dependency2
  Dependencies:
    - System.Object
...
```

### How it works

Internally *YADA* uses the great [*Mono.Cecil*](https://www.mono-project.com/docs/tools+libraries/libraries/Mono.Cecil/) library to analyze the given assemblies. Each type found within the assemblies will be analyzed by the *TypeAnalyse*.

Dependencies will be searched at the following ways:

* base class
* implemented interfaces
* members and properties
* parameters and return values 
* casts and created objects within the methods
  
Any found dependency will be added to the ITypeDescription together with the IDependencyContext containing information about the occurrence. Any needed information will be retrieved during this step and stored in the result set.

If not any type should be analyzed it is possible to ignore them directly during the type loading. This can be done with the **YADA.Analyzer.IgnoreType** Attribute.
This has to be added above one method in the call stack which creates the typeloader. No only the type will be removed from the result set also
any dependency to this type in other types are ignored.
In the example above ```[IgnoreType("**.*Dependency*")]``` will remove any class containing **Dependency** in the name or as part of a namespace.

### Testability as first class citizen

Any complex logic needs tests. And creating tests for input data fetched from assemblies is not the right way. So any interface in YADA uses only plain c# types, which provides a simple way to test your application with Fakes. In fact there are simple fake implementations for ITypeDescription and IDependency available.

### Customizing output

If you create an automated test to ensure your architectural rules for dependencies, a red test is not enough to know exactly what happens. It is nice to know that you have introduced a violation, but to fix the issue you need also to know where. To archive this any time *YADA* founds a reference to a type, the current context will be preserved.
If class A depends on class B, a dependency will be generated. At any point where a reference in class A to class B was found, context information about the occurrence will be added to the dependency.
The *IDependencyContext* self is a nearly empty class, but it can be visited by IDependencyContextVisitor. If visited the context calls the correct method at the visitor and provides case depending information as parameter. An specialized implementation of *IDependencyContext* uses the provided information to generate an output fitting the current situation.

## Automate architectural rules

With the Core mechanisms of *YADA* described above it is possible to create own automated tests. A way to do this is provided with the rule engine, which can be found under. [*YADA.DependencyRuleEngine*](./src/DependencyRuleEngine/Readme.md).

##  Filtering the results

Not in all circumstances any rule may be fulfilled. Sometimes you add an new rule and the code base is still not adapted, sometimes someone has made
a mistake. However you need a possibility to commit code which breaks the test but only with an exclusion for the current code base. Therefor a filter mechanism was added and the possibility to create a baseline file for the current code base.
See [*FilteringResults*](./src/DependencyRuleEngine/ResultFiltering.md)
