// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using YADA.Core.Analyser;
using YADA.Core.DependencyRuleEngine;
using YADA.Core.DependencyRuleEngine.Rules;
using YADA.Core.DependencyRuleEngine.Feedback;
using Moq;

namespace YADA.Test
{
    [TestFixture]
    public class RuleEngineTests
    {

        public class DependencyRule : IDependencyRule<ITypeDescription, IDependency>
        {
            private readonly Func<ITypeDescription, IDependency, DependencyRuleResult> m_Function;

            public DependencyRule(Func<ITypeDescription, IDependency, DependencyRuleResult> function)
            {
                m_Function = function;
            }

            public DependencyRuleResult Apply(ITypeDescription type, IDependency dependency, IFeedbackCollector feedback)
            {
                return m_Function(type, dependency);
            }
        }

        public class TypeRule : ITypeRule<ITypeDescription>
        {
            private readonly Func<ITypeDescription, DependencyRuleResult> m_Function;

            public TypeRule(Func<ITypeDescription, DependencyRuleResult> function)
            {
                m_Function = function;
            }

            public DependencyRuleResult Apply(ITypeDescription type, IFeedbackCollector feedback)
            {
                return m_Function(type);
            }
        }

        [Test]
        public void Constructor_CorrectInput_Success()
        {
            var sut = new DependencyRuleEngine(new[] { new TypeRule(_ => DependencyRuleResult.Approve) }, new[] { new DependencyRule((s, d) => DependencyRuleResult.Approve) });

            Assert.NotNull(sut);
        }

        [Test]
        public void Analyse_AnyFilterDeliversTrue_Success()
        {
            var input = new List<ITypeDescription>() { new TypeDescriptionFake("Type1"), new TypeDescriptionFake("Type2") };

            var sut = new DependencyRuleEngine(new[] { new TypeRule(_ => DependencyRuleResult.Approve) }, new[] { new DependencyRule((s, d) => DependencyRuleResult.Approve) });

            var result = sut.Analyse(input, new FeedbackCollector());

            Assert.That(result, Is.True);
        }

        [Test]
        public void Analyse_ValidTypesWithoutDependency_Success()
        {
            var input = new List<ITypeDescription>() { new TypeDescriptionFake("Type1") };

            var sut = new DependencyRuleEngine(new[] { new TypeRule(_ => DependencyRuleResult.Approve) }, new IDependencyRule<ITypeDescription, IDependency>[0] { });

            var result = sut.Analyse(input, new FeedbackCollector());

            Assert.That(result, Is.True);
        }

        [Test]
        public void Analyse_SetOfTypes_TypeFilterCalledForAnyType()
        {
            var input = new List<ITypeDescription>() { new TypeDescriptionFake("Type1"), new TypeDescriptionFake("Type2") };

            var list = new List<ITypeDescription>();

            var sut = new DependencyRuleEngine(new[] { new TypeRule(t => { list.Add(t); return DependencyRuleResult.Approve; }) }, new[] { new DependencyRule((s, d) => DependencyRuleResult.Approve) });

            var result = sut.Analyse(input, new FeedbackCollector());

            Assert.That(list, Is.EquivalentTo(input));
        }

        [Test]
        public void Analyse_SetOfTypesWithoutDependency_DependencyFiltersNotCalled()
        {
            var input = new List<ITypeDescription>() { new TypeDescriptionFake("Type1"), new TypeDescriptionFake("Type2") };

            bool dependencyFilterCall = false;

            var sut = new DependencyRuleEngine(
                new[] { new TypeRule(_ => DependencyRuleResult.Approve) },
                new[] { new DependencyRule((s, d) => { dependencyFilterCall = true; return DependencyRuleResult.Approve; }) });

            var result = sut.Analyse(input, new FeedbackCollector());

            Assert.That(dependencyFilterCall, Is.False);
        }

        [Test]
        public void Analyse_SetOfTypesOneWithExactlyDependency_DependencyFiltersCalled()
        {
            var input = new List<TypeDescriptionFake>() { new TypeDescriptionFake("Type1"), new TypeDescriptionFake("Type2") };
            input[0].Add(input[1]);

            var calls = new List<Tuple<ITypeDescription, IDependency>>();
            var sut = new DependencyRuleEngine(new[] { new TypeRule(_ => DependencyRuleResult.Approve) }, new[] { new DependencyRule((s, d) => { calls.Add(new Tuple<ITypeDescription, IDependency>(s, d)); return DependencyRuleResult.Approve; }) });

            var result = sut.Analyse(input, new FeedbackCollector());

            Assert.That(calls, Is.EquivalentTo(new[] { new Tuple<ITypeDescription, IDependency>(input[0], input[0].Dependencies.First()) }));
        }

        [Test]
        public void Analyse_FirstTypeRuleSkipsType_NoOtherTypeRuleCalledForThisType() 
        {
            var input = new List<TypeDescriptionFake>() { new TypeDescriptionFake("Type1"), new TypeDescriptionFake("Type2") };

            var sut = new DependencyRuleEngine(new[] { new TypeRule(_ => DependencyRuleResult.Skip), new TypeRule(_ => throw new NotImplementedException()) }, new[] { new DependencyRule((s, d) => { throw new NotImplementedException(); }) });

            Assert.DoesNotThrow(()=> sut.Analyse(input, new FeedbackCollector()));
        }
        
        [Test]
        public void Analyse_SkippedType_DoesNotFail() 
        {
            var input = new List<TypeDescriptionFake>() { new TypeDescriptionFake("Type1"), new TypeDescriptionFake("Type2") };

            
            var sut = new DependencyRuleEngine(new[] { new TypeRule(_ => DependencyRuleResult.Skip), new TypeRule(_ => throw new NotImplementedException()) }, new[] { new DependencyRule((s, d) => { throw new NotImplementedException(); }) });

            var result = sut.Analyse(input, new FeedbackCollector());

            Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        public void Analyse_RejectType_Failed()
        {
            var input = new List<TypeDescriptionFake>() { new TypeDescriptionFake("Type1"), new TypeDescriptionFake("Type2") };

            var sut = new DependencyRuleEngine(new[] { new TypeRule(_ => DependencyRuleResult.Reject) }, new[] { new DependencyRule((s,v) =>  DependencyRuleResult.Ignored )});
            var result = sut.Analyse(input, new FeedbackCollector());

            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public void Analyse_RejectType_NoOtherTypeRuleExecutedForThisType()
        {
            var input = new List<TypeDescriptionFake>() { new TypeDescriptionFake("Type1"), new TypeDescriptionFake("Type2") };

            var typeRejectingRule = new Mock<ITypeRule<ITypeDescription>>();
            typeRejectingRule.Setup(t => t.Apply(input[0], It.IsAny<IFeedbackCollector>())).Returns(DependencyRuleResult.Reject);
            typeRejectingRule.Setup(t => t.Apply(input[1], It.IsAny<IFeedbackCollector>())).Returns(DependencyRuleResult.Approve);

            var additionalTypeRule = new Mock<ITypeRule<ITypeDescription>>();

            var sut = new DependencyRuleEngine(new[] { typeRejectingRule.Object, additionalTypeRule.Object }, new[] { Mock.Of<IDependencyRule<ITypeDescription, IDependency>>() });
            var result = sut.Analyse(input, new FeedbackCollector());

            additionalTypeRule.Verify(c => c.Apply(input[0], It.IsAny<IFeedbackCollector>()), Times.Never);
        }

        [Test]
        public void Analyse_OneRejectOneApprovedType_ApplyCalledAtAllRulesForApprovedType()
        {
            var type1 = new TypeDescriptionBuilder("Type1").DependsOn("Dependency1").Create();
            var type2 = new TypeDescriptionBuilder("Type2").DependsOn("Dependency2").Create();
            var input = new List<ITypeDescription>() { type1, type2 };

            var typeRejectingRule = new Mock<ITypeRule<ITypeDescription>>();
            typeRejectingRule.Setup(t => t.Apply(input[0], It.IsAny<IFeedbackCollector>())).Returns(DependencyRuleResult.Reject);
            typeRejectingRule.Setup(t => t.Apply(input[1], It.IsAny<IFeedbackCollector>())).Returns(DependencyRuleResult.Approve);

            var additionalTypeRule = new Mock<ITypeRule<ITypeDescription>>();
            var dependencyRule = new Mock<IDependencyRule<ITypeDescription, IDependency>>();

            var sut = new DependencyRuleEngine(new[] { typeRejectingRule.Object, additionalTypeRule.Object }, new[] {dependencyRule.Object});
            var result = sut.Analyse(input, new FeedbackCollector());

            additionalTypeRule.Verify(c => c.Apply(input[1], It.IsAny<IFeedbackCollector>()), Times.Once);
            dependencyRule.Verify(c => c.Apply(input[1], input[1].Dependencies.First(), It.IsAny<IFeedbackCollector>()), Times.Once);
            dependencyRule.Verify(c => c.Apply(input[0], input[0].Dependencies.First(), It.IsAny<IFeedbackCollector>()), Times.Never);
        }

        [Test]
        public void Analyse_RejectType_NoOtherDependencyRuleExecutedForThisType()
        {
            var type1 = new TypeDescriptionBuilder("Type1").DependsOn("DependencyType").Create();

            var input = new List<ITypeDescription>() { type1, new TypeDescriptionFake("Type2") };

            var sut = new DependencyRuleEngine(new[] { new TypeRule(_ => DependencyRuleResult.Reject) }, new[] { new DependencyRule((s,v) =>  throw new NotImplementedException() )});
            var result = sut.Analyse(input, new FeedbackCollector());

            Assert.That(result, Is.EqualTo(false));
        }

        public class TypeDescriptionBuilder 
        {
            private string m_Fullname;

            private List<TypeDescriptionFake> m_Dependencies;

            public TypeDescriptionBuilder(string fullname)
            {
                m_Fullname = fullname;
            }

            public TypeDescriptionBuilder() 
            {
                m_Fullname = "SomeType";
            }

            public TypeDescriptionBuilder FullName(string fullName) 
            {
                m_Fullname = fullName;
                return this;
            }

            public TypeDescriptionBuilder DependsOn(string dependencyTypeFullName) 
            {
                if(m_Dependencies == null) 
                {
                    m_Dependencies = new List<TypeDescriptionFake>();
                }

                m_Dependencies.Add(new TypeDescriptionFake(dependencyTypeFullName));

                return this;
            }

            public ITypeDescription Create()
            {
                var result =  new TypeDescriptionFake(m_Fullname);

                if(m_Dependencies!=null) 
                {
                    foreach (var dependencies in m_Dependencies)
                    {
                        result.Add(dependencies);
                    }
                }
                
                return result;
            }
       }
    }
}