// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using NUnit.Framework;
using YADA.Core.DependencyRuleEngine;

namespace YADA.Test
{
    [TestFixture]
    public class DependencyRuleResultAggregationTest
    {
        [Test]
        public void AggregatedResult_NothingAdded_Ignored()
        {
            DependencyRuleResultAggregation sut = new DependencyRuleResultAggregation();

            var result = sut.AggregatedResult();

            Assert.That(result, Is.EqualTo(DependencyRuleResult.Ignored));
        }

        [Test]
        public void AggregatedResult_OnlyIgnoredAdded_Ignored()
        {
            DependencyRuleResultAggregation sut = new DependencyRuleResultAggregation();

            sut.Add(DependencyRuleResult.Ignored);
            sut.Add(DependencyRuleResult.Ignored);
            sut.Add(DependencyRuleResult.Ignored);
            sut.Add(DependencyRuleResult.Ignored);

            var result = sut.AggregatedResult();

            Assert.That(result, Is.EqualTo(DependencyRuleResult.Ignored));
        }


        [Test]
        public void AggregatedResult_OneApprovedRestIgnoredAdded_Approved()
        {
            DependencyRuleResultAggregation sut = new DependencyRuleResultAggregation();

            sut.Add(DependencyRuleResult.Ignored);
            sut.Add(DependencyRuleResult.Ignored);
            sut.Add(DependencyRuleResult.Approve);
            sut.Add(DependencyRuleResult.Ignored);

            var result = sut.AggregatedResult();

            Assert.That(result, Is.EqualTo(DependencyRuleResult.Approve));
        }

        [Test]
        public void AggregatedResult_AllApproved_Approved()
        {
            DependencyRuleResultAggregation sut = new DependencyRuleResultAggregation();

            sut.Add(DependencyRuleResult.Approve);
            sut.Add(DependencyRuleResult.Approve);
            sut.Add(DependencyRuleResult.Approve);
            sut.Add(DependencyRuleResult.Approve);

            var result = sut.AggregatedResult();

            Assert.That(result, Is.EqualTo(DependencyRuleResult.Approve));
        }


        [Test]
        public void AggregatedResult_OneSkipped_Skip()
        {
            DependencyRuleResultAggregation sut = new DependencyRuleResultAggregation();

            sut.Add(DependencyRuleResult.Approve);
            sut.Add(DependencyRuleResult.Ignored);
            sut.Add(DependencyRuleResult.Approve);
            sut.Add(DependencyRuleResult.Skip);

            var result = sut.AggregatedResult();

            Assert.That(result, Is.EqualTo(DependencyRuleResult.Skip));
        }

        [Test]
        public void AggregatedResult_OneRejectedNoSkip_Reject()
        {
            DependencyRuleResultAggregation sut = new DependencyRuleResultAggregation();

            sut.Add(DependencyRuleResult.Approve);
            sut.Add(DependencyRuleResult.Ignored);
            sut.Add(DependencyRuleResult.Approve);
            sut.Add(DependencyRuleResult.Reject);

            var result = sut.AggregatedResult();

            Assert.That(result, Is.EqualTo(DependencyRuleResult.Reject));
        }

        [Test]
        public void AggregatedResult_OneRejectedOneSkip_Skipped()
        {
            DependencyRuleResultAggregation sut = new DependencyRuleResultAggregation();

            sut.Add(DependencyRuleResult.Reject);
            sut.Add(DependencyRuleResult.Ignored);
            sut.Add(DependencyRuleResult.Approve);
            sut.Add(DependencyRuleResult.Skip);

            var result = sut.AggregatedResult();

            Assert.That(result, Is.EqualTo(DependencyRuleResult.Skip));
        }
    }
}