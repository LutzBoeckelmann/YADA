# The rule engine

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
```

There are two types of rules supported by the engine.

**TypeRules:** implementing the interface *YADA.Core.DependencyRuleEngine.Rules.ITypeRule<T>*

```csharp
    public interface ITypeRule<T>
    {
        DependencyRuleResult Apply(T type, IFeedbackCollector feedback);
    }
```

Type rules check the type self. These kind of rules may be used to reject any type which does not obeys the architectural rules for namespaces and type names at all. Also a type rule may skip types which should not be analysed. In any case for a dependency analyses at least one rule must accept the type.

**DependencyRules:** implementing the interface *YADA.Core.DependencyRuleEngine.Rules.IDependencyRule<T, K>*

```csharp
    public interface IDependencyRule<T, K>
    {
        DependencyRuleResult Apply(T type, K dependency, IFeedbackCollector feedback);
    }
```

A dependency rule checks any dependency of a type. A dependency may be approved, rejected, skipped or ignored. Skipped suppresses further analyses of the dependency and ignore simple has no further effect.

The result of any rule is **DependencyRuleResult**. It provides four different result types:

| return type | consequences |
|-------------|--------------|
|  Approve    | the rule approves the type or dependency. Overall at least one rule must approve |
|  Ignore     | The rule is not applicable, no influences on the over all result
|  Reject     | The dependency or type is rejected. One rejecting rule is quit enough to reject the dependency or type overall|
|  Skip       | The current type or dependency should be not further processed. No further rule will be applied. Used to implement whitelists.|
  
### Feedback collector

Like mentioned before without good feedback the best tests are worthless.
To support this the FeedbackCollector was introduced. The **IFeedbackCollector**
and the associated interfaces resides in the namespace
**YADA.DependencyRuleEngine.Feedback**.
As root element of any feedback an implementation of **IFeedbackCollector**
must be injected to the rule engine. The analyze procedure follows always the
same steps, this is reflected in the feedback interfaces too.
For any specific situation a specific feedback interface is available.
Adding feedback returns the next needed specific feedback interface.
Simpler formulated the interfaces are nested in a way to form a fluent interface.

A good feedback should not only tell what was violated also where the violation
can be found. The example strings above can be enhanced with the
**IDependencyContextVisitor\<T>**. 

### Testability

Also the requirement for testability is reflected in the way how the RuleEngine works. Any rule you define is coded in a separate class. And this class is indeed standalone testable.
Any interface used by YADA is simple enough to be mocked easily. Never the less this would lead to a lot of boiler plate code. To overcome this **ITypeRule\<T>** and **IDependencyRule<T, K>** do not use **ITypeDescription** and **IDependency** directly, but behind the generic parameter **T** and **K**. So any rule can be tested with domain specific input. To use the arbitrary typed rules in the rule engine the adapter classes **BaseDependencyRule<T, K>** and **BaseTypeRule<T,K>** should be used. The mapping in the adapter is done by an implementation of **IDependencyRuleInputMapper<T, K>**.

### ArchRuleDemo

An full fletched example is available under *ArchRuleDemo*. It contains specific architectural rules, which are formulated against an architectural model. A set of tests uses this rules and the rule engine to analyse the example dll *ArchRuleExample*. The example is documented under [ArchRuleDemo.md](../../ArchRuleDemo/ArchRuleDemo.md).