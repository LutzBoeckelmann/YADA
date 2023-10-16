// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using NUnit.Framework;
using YADA.Analyzer;
using YADA.DependencyRuleEngine;
using YADA.DependencyRuleEngine.Rules;

namespace YADA.Test
{
    [TestFixture]
    public class WhitelistIgnoreTypeRuleTests
    {
        [Test]
        public void Apply_TypeNotOnWhiteList_Ignore()
        {
            var notWhiteListedType = new TypeDescriptionFake("notWhiteListedType");
            
            WhitelistIgnoreTypeRule sut = new WhitelistIgnoreTypeRule(new [] {"WhiteListedType"});
            
            var result = sut.Apply(notWhiteListedType, null);

            Assert.That(result, Is.EqualTo(DependencyRuleResult.Skip));
        }
        
        [Test]
        public void Apply_TypeOnWhiteList_Skip()
        {
            var whiteListedType = new TypeDescriptionFake("WhiteListedType");
            
            WhitelistIgnoreTypeRule sut = new WhitelistIgnoreTypeRule(new [] {"WhiteListedType"});
            
            var result = sut.Apply(whiteListedType, null);

            Assert.That(result, Is.EqualTo(DependencyRuleResult.Ignored));
        }


        
    }
}