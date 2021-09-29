To demonstrate the usage of YADA and a the possibility its RuleEngine provides, the ArchRuleDemo was created. The demo contains two parts at first the project ArchRuleExample. This project has self has no technical content, it only contains a set of classes, which should be analysed. The interesting part is the demo is contained in ArchRuleDemo project. It contains the codified rule of the example, the needed infrastructure and the tests for the rule and the system tests for the ArchRuleExample self.

At first a brief definition of the rules any type in ArchRuleExample should fulfill. The example is structured in layers. Any class must be in exact one domain layer as well as exactly one technical layer. Additionally any piece of code must belong to one module. 
This structure is expressed in the namespaces of any ArchRuleExample class.
To ensure the layering and the architecture any namespace must follow a specific form. 

*ArchRuleExample.DomainLayer.Module.TechnicalLayer.NotArchitecturalRelevant.ClassName*

* Any namespace starts with the prefix ArchRuleExample
* Domain is structured in Infrastructure, Core and Extensions 
* Technical the system is structured in Data, BusinessLogic and UI
* Module is the Name of the Module
* The suffix of the namespace is not relevant for the architecture and will be ignored. Here a subsystem may structure its internals  

To have some kind of a nearly real live example how YADA and a RuleEngine may be used to ensure architectural rules in tests, the structure above was codified in tests.

At first the namespace of any class in the ArchRuleExample.dll will be checked if it fits into the defined scheme. A non fitting class breaks the tests and will be ignored for further analyses. This is simple needed because any further test needs to parse the namespace.

The following rules checks if the found dependencies between any layering 

The following picture shows the structure of the example assembly.

```plantuml
package Core {
class "ArchRuleExample.Core.CoreModule1.Data.Module1DataClass1" as md1
}
package Infrastructure {
class "ArchRuleExample.Infrastructure.InfraModule1.UI.InfraModuleUIClass1" as inmUI1
class "ArchRuleExample.Infrastructure.InfraModule1.Data.InfraModuleDataClass1" as inmDa1    
}
class "ArchRuleExample.ClassWithoutCorrectNamespace"
note top
A class without a valid namespace.
Will be check only by the type rules.
end note 
md1 <- inmUI1 : illegal
inmDa1 -u-> md1 : illegal
```

The ArchRuleDemo assembly contains the code and the test to analyse the ArchRuleExample and is structured in the following parts.

* Architectural Models
    This contains the classes to describe the current architecture in code
* Architectural Rules
    This contains an implements the rules defined above
* ArchRuleTests
    This contains the tests for the rules self and a system tests which analyses the example
* DependencyRuleEngine
    This contains an example how to specialize the DependencyRuleEngine for this specific example