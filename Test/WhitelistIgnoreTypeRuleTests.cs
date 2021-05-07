using NUnit.Framework;
using YADA.Core.Analyser;
using YADA.Core.DependencyRuleEngine;
using YADA.Core.DependencyRuleEngine.Rules;

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