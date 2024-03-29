// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using NUnit.Framework;
using YADA.Analyzer;
using YADA.DependencyRuleEngine;
using YADA.DependencyRuleEngine.Rules;

namespace YADA.DependencyRuleEngine.Test
{
    [TestFixture]
    public class BlacklistIgnoreTypeRuleTests
    {
        [Test]
        public void Apply_TypeOnBlacklist_Skip()
        {
            var typeToIgnore = new TypeDescriptionFake("TypeToSkip");
            
            BlacklistIgnoreTypeRule sut = new BlacklistIgnoreTypeRule(new [] {typeToIgnore.FullName});
            
            var result = sut.Apply(typeToIgnore, null);

            Assert.That(result, Is.EqualTo(DependencyRuleResult.Skip));
        }
        
        [Test]
        public void Apply_TypeNotOnBlacklist_Ignore()
        {
            var typeToIgnore = new TypeDescriptionFake("TypeNotToSkip");
            
            BlacklistIgnoreTypeRule sut = new BlacklistIgnoreTypeRule(new [] {"TypeToSkip"});
            
            var result = sut.Apply(typeToIgnore, null);

            Assert.That(result, Is.EqualTo(DependencyRuleResult.Ignored));
        }



    }
}