// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System;
using System.Collections.Generic;
using NUnit.Framework;
using YADA.Core.Analyser;
using YADA.Core.DependencyRuleEngine.Feedback;

namespace YADA.Test
{
    [TestFixture]
    public class FeedbackFilterTests
    {
        private class TestNext : IFeedbackVisitor
        {
            public List<string> SeenCalls = new List<string>();
            private class Disposable : IDisposable
            {
                private readonly Action m_DisposeAction;

                public Disposable(Action disposeAction)
                {
                    m_DisposeAction = disposeAction;
                }

                public void Dispose()
                {
                    m_DisposeAction();
                }
            }
            public IDisposable Context(IDependencyContext context)
            {
                var msg = ((RecordedContext)context).SerializedContext;
                SeenCalls.Add(msg);
                return new Disposable(() => SeenCalls.Add($"{msg} disposed"));
            }

            public IDisposable ForbiddenDependency(string dependency)
            {
                SeenCalls.Add(dependency);
                return new Disposable(() => SeenCalls.Add($"{dependency} disposed"));
            }

            public IDisposable Info(string msg)
            {
                SeenCalls.Add(msg);
                return new Disposable(() => SeenCalls.Add($"{msg} disposed"));
            }

            public IDisposable Type(string type)
            {
                SeenCalls.Add(type);
                return new Disposable(() => SeenCalls.Add($"{type} disposed"));
            }

            public IDisposable ViolatedRule(string rule)
            {
                SeenCalls.Add(rule);
                return new Disposable(() => SeenCalls.Add($"{rule} disposed"));
            }
        }

        [Test]
        public void KnowType_NoFeedback()
        {
            var tester = new TestNext();
            var types = new List<TypeRecording>() { new TypeRecording("KnownType", null) };

            var sut = new FeedbackFilter(types, tester);
            var disposable = sut.Type("KnownType");
            disposable.Dispose();

            Assert.That(tester.SeenCalls, Is.Empty);
        }

        [Test]
        public void KnowRuleViolationRuleViolation_AtKnownType_NoFeedback()
        {
            var tester = new TestNext();
            var type = new TypeRecording("KnownType", null);
            type.ViolatedRule("KnownRule").Dispose();

            var sut = new FeedbackFilter(new [] {type}, tester);
            using (sut.Type("KnownType"))
            {
                sut.ViolatedRule("KnownRule").Dispose();
            }
            Assert.That(tester.SeenCalls, Is.Empty);
        }

        [Test]
        public void ViolationNewRule_At_KnownType_Feedback_ForTypeAndNewRule()
        {
            var tester = new TestNext();
            var type = new TypeRecording("KnownType", null);
            type.ViolatedRule("KnownRule").Dispose();

            var sut = new FeedbackFilter(new[] { type }, tester);
            using (sut.Type("KnownType"))
            {
                sut.ViolatedRule("KnownRule").Dispose();
                sut.ViolatedRule("UnKnownRule").Dispose();
            }
            Assert.That(tester.SeenCalls.Count, Is.EqualTo(4));
        }

        [Test]
        public void KnownDependency_NoFeedback()
        {
            var tester = new TestNext();
            var type = new TypeRecording("KnownType", new FeedbackRoot(true));
            
            using (type.ViolatedRule("KnownRule"))
            {
                type.ForbiddenDependency("Dependency").Dispose();
            }

            type.Dispose();

            var sut = new FeedbackFilter(new[] { type }, tester);
            using (sut.Type("KnownType"))
            {
                using (sut.ViolatedRule("KnownRule"))
                {
                    sut.ForbiddenDependency("Dependency").Dispose();
                }
            }
            Assert.That(tester.SeenCalls.Count, Is.EqualTo(0));
        }

        [Test]
        public void NewDependencyAddedToKnownTypeAndRule_FeedbackForTypeRuleAndNewDependency()
        {
            var tester = new TestNext();
            var type = new TypeRecording("KnownType", new FeedbackRoot(true));

            using (type.ViolatedRule("KnownRule"))
            {
                type.ForbiddenDependency("Dependency").Dispose();
            }

            type.Dispose();

            var sut = new FeedbackFilter(new[] { type }, tester);
            using (sut.Type("KnownType"))
            {
                using (sut.ViolatedRule("KnownRule"))
                {
                    sut.ForbiddenDependency("Dependency").Dispose();
                    sut.ForbiddenDependency("Unknown").Dispose();
                }
            }
            Assert.That(tester.SeenCalls.Count, Is.EqualTo(6));
        }

        [Test]
        public void KnownInfo_NoFeedback()
        {
            var tester = new TestNext();
            var type = new TypeRecording("KnownType", new FeedbackRoot(true));

            using (type.ViolatedRule("KnownRule"))
            {
                type.ForbiddenDependency("Dependency").Dispose();
                type.Info("KnownInfo").Dispose();
            }

            type.Dispose();

            var sut = new FeedbackFilter(new[] { type }, tester);
            using (sut.Type("KnownType"))
            {
                using (sut.ViolatedRule("KnownRule"))
                {
                    sut.ForbiddenDependency("Dependency").Dispose();
                    sut.Info("KnownInfo");
                }
            }
            Assert.That(tester.SeenCalls.Count, Is.EqualTo(0));
        }

        [Test]
        public void NewInfos_Will_Not_Generate_Feedback_At_Known_Types()
        {
            var tester = new TestNext();
            var type = new TypeRecording("KnownType", new FeedbackRoot(true));

            using (type.ViolatedRule("KnownRule"))
            {
                type.ForbiddenDependency("Dependency").Dispose();
                type.Info("KnownInfo").Dispose();
            }

            type.Dispose();

            var sut = new FeedbackFilter(new[] { type }, tester);
            using (sut.Type("KnownType"))
            {
                using (sut.ViolatedRule("KnownRule"))
                {
                    sut.ForbiddenDependency("Dependency").Dispose();
                    sut.Info("KnownInfo").Dispose(); ;
                    sut.Info("Unknown").Dispose(); ;
                }
            }
            Assert.That(tester.SeenCalls.Count, Is.EqualTo(0));
        }

        [Test]
        public void UnknownDependency_Feedback_Also_For_Corresponding_Known_Infos()
        {
            var tester = new TestNext();
            var type = new TypeRecording("KnownType", new FeedbackRoot(true));

            using (type.ViolatedRule("KnownRule"))
            {
                type.ForbiddenDependency("Dependency").Dispose();
                type.Info("KnownInfo").Dispose();
            }

            type.Dispose();

            var sut = new FeedbackFilter(new[] { type }, tester);
            using (sut.Type("KnownType"))
            {
                using (sut.ViolatedRule("UnknownRule"))
                {
                    sut.ForbiddenDependency("UnknownDependency").Dispose();
                    sut.Info("KnownInfo").Dispose();
                }
            }
            Assert.That(tester.SeenCalls.Count, Is.EqualTo(8));
        }

        [Test]
        public void KnownContext_NoFeedback()
        {
            var tester = new TestNext();
            var type = new TypeRecording("KnownType", new FeedbackRoot(true));

            using (type.ViolatedRule("KnownRule"))
            {
                using (type.ForbiddenDependency("Dependency"))
                {
                    type.Context(new RecordedContext("KnownContext"));
                }
                    type.Info("KnownInfo").Dispose();
                
            }

            type.Dispose();

            var sut = new FeedbackFilter(new[] { type }, tester);
            using (sut.Type("KnownType"))
            {
                using (sut.ViolatedRule("KnownRule"))
                {
                    using (sut.ForbiddenDependency("Dependency"))
                    {
                        sut.Context(new RecordedContext("KnownContext"));
                    }
                    
                    sut.Info("KnownInfo").Dispose();
                }
            }

            Assert.That(tester.SeenCalls.Count, Is.EqualTo(0));
        }

        [Test]
        public void UnKnownContext_Feedback()
        {
            
            var type = new TypeRecording("KnownType", new FeedbackRoot(true));

            using (type.ViolatedRule("KnownRule"))
            {
                using (type.ForbiddenDependency("Dependency"))
                {
                    type.Context(new RecordedContext("KnownContext"));
                }
                type.Info("KnownInfo").Dispose();

            }

            type.Dispose();
            
            var tester = new TestNext();
            var sut = new FeedbackFilter(new[] { type }, tester);
            using (sut.Type("KnownType"))
            {
                using (sut.ViolatedRule("KnownRule"))
                {
                    using (sut.ForbiddenDependency("Dependency"))
                    {
                        sut.Context(new RecordedContext("KnownContext")).Dispose();
                        sut.Context(new RecordedContext("UnknownContext")).Dispose();
                    }

                    sut.Info("KnownInfo").Dispose();
                }
            }

            Assert.That(tester.SeenCalls.Count, Is.EqualTo(8));
        }
    }
}
