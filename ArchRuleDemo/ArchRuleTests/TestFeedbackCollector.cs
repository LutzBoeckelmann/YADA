// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Collections.Generic;
using YADA.Analyzer;
using YADA.DependencyRuleEngine.Feedback;


namespace ArchRuleDemo.ArchRuleTests
{
    public class TestFeedbackCollector : IFeedbackCollector, ITypeFeedback, IRuleFeedback, IDependencyFeedback
    {
        public TestFeedbackCollector()
        {
        }

        public List<string> AddFeedbackForTypeCalls { get; } = new List<string>();
        public List<string> ViolatesRuleCalls { get; } = new List<string>();
        public List<string> ForbiddenDependencyCalls { get; } = new List<string>();


        public List<string> AddInfoCalls { get; } = new List<string>();
        public List<string> AtCalls { get; } = new List<string>();



        public ITypeFeedback AddFeedbackForType(string type)
        {
            AddFeedbackForTypeCalls.Add(type);
            return this;
        }

        public IRuleFeedback AddInfo(string name)
        {
            AddInfoCalls.Add(name);
            return this;
        }

        public IRuleFeedback ViolatesRule(string nameOfRule)
        {
            ViolatesRuleCalls.Add(nameOfRule);
            return this;
        }
        public IDependencyFeedback ForbiddenDependency(string dependency)
        {
            ForbiddenDependencyCalls.Add(dependency);
            return this;
        }

        public IDependencyFeedback At(IDependencyContext context)
        {
            AtCalls.Add(context.ToString());
            return this;
        }

        public void Explore(IFeedbackVisitor visitor)
        {
            
        }
    }
}
