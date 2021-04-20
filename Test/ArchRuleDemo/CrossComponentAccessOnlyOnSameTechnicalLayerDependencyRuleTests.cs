// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Linq;
using Moq;
using NUnit.Framework;
using YADA.Core;

namespace YADA.Test
{
    [TestFixture]
    public class CrossComponentAccessOnlyOnSameTechnicalLayerDependencyRuleTests 
    {
        [Test]
        public void Apply_DependencyWithoutValidType_Ignore()
        {
            var type = new ArchRuleExampleType("",null, null, null, true);
            var dependencyType = new ArchRuleExampleType("",null,null,null, false);

            var sut = new CrossComponentAccessOnlyOnSameTechnicalLayerDependencyRule();

            var result = sut.Apply(type, new ArchRuleExampleDependency(dependencyType), new SimpleStringCollectionFeedbackSet());

            Assert.That(result, Is.EqualTo(DependencyRuleResult.Ignore));
        }

        [Test]
        public void Apply_DependencyInSameComponent_Approve()
        {
            var type = new ArchRuleExampleType("",null,new ArchRuleModule("Module"),null, true);
            var dependencyType = new ArchRuleExampleType("",null, new ArchRuleModule("Module"), null, true);

            var sut = new CrossComponentAccessOnlyOnSameTechnicalLayerDependencyRule();

            var result = sut.Apply(type, new ArchRuleExampleDependency(dependencyType), new SimpleStringCollectionFeedbackSet());

            Assert.That(result, Is.EqualTo(DependencyRuleResult.Approve));

        }

        [Test]
        public void Apply_DependencyDifferentModulesSameLayer_Approve()
        {
            var type = new ArchRuleExampleType("",null, new ArchRuleModule("Module"), new ArchRuleTechnicalLayer(ArchRuleTechnicalLayer.BusinessLogic), true);
            var dependencyType = new ArchRuleExampleType("",null, new ArchRuleModule("OtherModule"), new ArchRuleTechnicalLayer(ArchRuleTechnicalLayer.BusinessLogic), true);

            var sut = new CrossComponentAccessOnlyOnSameTechnicalLayerDependencyRule();
  
            var result = sut.Apply(type, new ArchRuleExampleDependency(dependencyType), new SimpleStringCollectionFeedbackSet());

            Assert.That(result, Is.EqualTo(DependencyRuleResult.Approve));

        }

       [Test]
        public void Apply_DependencyInSameComponent_NoFeedback()
        {
            var type = new ArchRuleExampleType("",null,new ArchRuleModule("Module"),null, true);
            var dependencyType = new ArchRuleExampleType("",null, new ArchRuleModule("Module"), null, true);
            var feedback = new SimpleStringCollectionFeedbackSet();
            var sut = new CrossComponentAccessOnlyOnSameTechnicalLayerDependencyRule();

            var result = sut.Apply(type, new ArchRuleExampleDependency(dependencyType), feedback);

            Assert.That(feedback.Messages, Is.Empty);

        }

        [Test]
        public void Apply_DependencyDifferentModulesSameLayer_NoFeedback()
        {
            var type = new ArchRuleExampleType("",null, new ArchRuleModule("Module"), new ArchRuleTechnicalLayer(ArchRuleTechnicalLayer.BusinessLogic), true);
            var dependencyType = new ArchRuleExampleType("",null, new ArchRuleModule("OtherModule"), new ArchRuleTechnicalLayer(ArchRuleTechnicalLayer.BusinessLogic), true);
            var feedback = new SimpleStringCollectionFeedbackSet();
            var sut = new CrossComponentAccessOnlyOnSameTechnicalLayerDependencyRule();
  
            var result = sut.Apply(type, new ArchRuleExampleDependency(dependencyType), feedback);

            Assert.That(feedback.Messages, Is.Empty);

        }
      
        [Test]
        public void Apply_DependencyDifferentModulesDifferentLayer_Reject()
        {
            var type = new ArchRuleExampleType("",null,new ArchRuleModule("Module"),new ArchRuleTechnicalLayer(ArchRuleTechnicalLayer.BusinessLogic), true);
            var dependencyType = new ArchRuleExampleType("",null, new ArchRuleModule("OtherModule"), new ArchRuleTechnicalLayer(ArchRuleTechnicalLayer.Data), true);

            var sut = new CrossComponentAccessOnlyOnSameTechnicalLayerDependencyRule();

            var result = sut.Apply(type, new ArchRuleExampleDependency(dependencyType), new SimpleStringCollectionFeedbackSet());

            Assert.That(result, Is.EqualTo(DependencyRuleResult.Reject));
        }

        [Test]
        public void Apply_DependencyDifferentModulesDifferentLayer_ExpectedOutput()
        {
            var type = new ArchRuleExampleType("My.Module",null,new ArchRuleModule("Module"),new ArchRuleTechnicalLayer(ArchRuleTechnicalLayer.BusinessLogic), true);
            var dependencyType = new ArchRuleExampleType("OtherType.OtherModule",null, new ArchRuleModule("OtherModule"), new ArchRuleTechnicalLayer(ArchRuleTechnicalLayer.Data), true);

            var sut = new CrossComponentAccessOnlyOnSameTechnicalLayerDependencyRule();
            var feeback = new SimpleStringCollectionFeedbackSet();
            sut.Apply(type, new ArchRuleExampleDependency(dependencyType), feeback);

            var result = feeback.Messages.ToArray();
            Assert.That(result, Has.Exactly(1).Items);
            var t = result[0];
            Assert.That(result[0], Contains.Substring("CrossComponentAccessOnlyOnSameTechnicalLayerDependencyRule").And.Contains("My.Module").And.Contains("OtherType.OtherModule"));
        }
    }
}