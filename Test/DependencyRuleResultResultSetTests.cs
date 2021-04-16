using NUnit.Framework;
using YADA.Core;

namespace YADA.Test
{
    [TestFixture]
    public class DependencyRuleResultResultSetTests
    {
        [Test]
        public void Approved_NoApproveResultAdded_False() 
        {
            DependencyRuleResultResultSet sut = new DependencyRuleResultResultSet();

            sut.Add(DependencyRuleResult.Ignore);
            sut.Add(DependencyRuleResult.Ignore);
            sut.Add(DependencyRuleResult.Ignore);
            sut.Add(DependencyRuleResult.Ignore);

            Assert.That(sut.Approved, Is.False);
        }
        
        [Test]
        public void Approved_OneApproveResultAddedNoReject_True() 
        {
            DependencyRuleResultResultSet sut = new DependencyRuleResultResultSet();

            sut.Add(DependencyRuleResult.Ignore);
            sut.Add(DependencyRuleResult.Ignore);
            sut.Add(DependencyRuleResult.Approve);
            sut.Add(DependencyRuleResult.Ignore);

            Assert.That(sut.Approved, Is.True);
        }

        [Test]
        public void Approved_SeveralApprovedButOneRejects_False() 
        {
            DependencyRuleResultResultSet sut = new DependencyRuleResultResultSet();

            sut.Add(DependencyRuleResult.Ignore);
            sut.Add(DependencyRuleResult.Approve);
            sut.Add(DependencyRuleResult.Ignore);
            sut.Add(DependencyRuleResult.Reject);

            Assert.That(sut.Approved, Is.False);
        }

        
        [Test]
        public void Approved_NothingAdded_False() 
        {
            DependencyRuleResultResultSet sut = new DependencyRuleResultResultSet();

        
            Assert.That(sut.Approved, Is.False);
        }
   
        [Test]
        public void Ignored_NothingAdded_False() 
        {
            DependencyRuleResultResultSet sut = new DependencyRuleResultResultSet();
        
            Assert.That(sut.Ignore, Is.False);
        }
        
        [Test]
        public void Ignored_NotASingledIgnoredResultAdded_False() 
        {
            DependencyRuleResultResultSet sut = new DependencyRuleResultResultSet();
        
            sut.Add(DependencyRuleResult.Reject);
            sut.Add(DependencyRuleResult.Approve);
            sut.Add(DependencyRuleResult.Approve);
            sut.Add(DependencyRuleResult.Reject);


            Assert.That(sut.Ignore, Is.False);
        }

           [Test]
        public void Ignored_AtLeastOneIgnoredResultAdded_True() 
        {
            DependencyRuleResultResultSet sut = new DependencyRuleResultResultSet();
        
            sut.Add(DependencyRuleResult.Reject);
            sut.Add(DependencyRuleResult.Approve);
            sut.Add(DependencyRuleResult.Ignore);
            sut.Add(DependencyRuleResult.Reject);


            Assert.That(sut.Ignore, Is.True);
        }
    }
}