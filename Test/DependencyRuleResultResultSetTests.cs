using NUnit.Framework;
using YADA.Core;
using YADA.Core.DependencyRuleEngine.Impl;
namespace YADA.Test
{
    [TestFixture]
    public class DependencyRuleResultResultSetTests
    {
        [Test]
        public void Approved_NoApproveResultAdded_False() 
        {
            DependencyRuleResultSet sut = new DependencyRuleResultSet();

            sut.Add(DependencyRuleResult.Ignore);
            sut.Add(DependencyRuleResult.Ignore);
            sut.Add(DependencyRuleResult.Ignore);
            sut.Add(DependencyRuleResult.Ignore);

            Assert.That(sut.Approved, Is.False);
        }
        
        [Test]
        public void Approved_OneApproveResultAddedNoReject_True() 
        {
            DependencyRuleResultSet sut = new DependencyRuleResultSet();

            sut.Add(DependencyRuleResult.Ignore);
            sut.Add(DependencyRuleResult.Ignore);
            sut.Add(DependencyRuleResult.Approve);
            sut.Add(DependencyRuleResult.Ignore);

            Assert.That(sut.Approved, Is.True);
        }

        [Test]
        public void Approved_SeveralApprovedButOneRejects_False() 
        {
            DependencyRuleResultSet sut = new DependencyRuleResultSet();

            sut.Add(DependencyRuleResult.Ignore);
            sut.Add(DependencyRuleResult.Approve);
            sut.Add(DependencyRuleResult.Ignore);
            sut.Add(DependencyRuleResult.Reject);

            Assert.That(sut.Approved, Is.False);
        }

        
        [Test]
        public void Approved_NothingAdded_False() 
        {
            DependencyRuleResultSet sut = new DependencyRuleResultSet();

        
            Assert.That(sut.Approved, Is.False);
        }
   
        [Test]
        public void Ignored_NothingAdded_False() 
        {
            DependencyRuleResultSet sut = new DependencyRuleResultSet();
        
            Assert.That(sut.Ignore, Is.False);
        }
        
        [Test]
        public void Ignored_NotASingledIgnoredResultAdded_False() 
        {
            DependencyRuleResultSet sut = new DependencyRuleResultSet();
        
            sut.Add(DependencyRuleResult.Reject);
            sut.Add(DependencyRuleResult.Approve);
            sut.Add(DependencyRuleResult.Approve);
            sut.Add(DependencyRuleResult.Reject);


            Assert.That(sut.Ignore, Is.False);
        }

           [Test]
        public void Ignored_AtLeastOneIgnoredResultAdded_True() 
        {
            DependencyRuleResultSet sut = new DependencyRuleResultSet();
        
            sut.Add(DependencyRuleResult.Reject);
            sut.Add(DependencyRuleResult.Approve);
            sut.Add(DependencyRuleResult.Ignore);
            sut.Add(DependencyRuleResult.Reject);


            Assert.That(sut.Ignore, Is.True);
        }
    }
}