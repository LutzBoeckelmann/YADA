// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using YADA.Core.Analyser;

namespace YADA.Core.DependencyRuleEngine.Feedback
{
    public class FeedbackRecorder : IFeedbackVisitor
    {
        private readonly FeedbackRoot m_Parent = new FeedbackRoot(false);
               
        public IDisposable Context(IDependencyContext context)
        {
            return m_Parent.Context(context);
        }

        public IDisposable ForbiddenDependency(string dependency)
        {
            return m_Parent.ForbiddenDependency(dependency);
        }

        public IDisposable Info(string msg)
        {
            return m_Parent.Info(msg);
        }

        public IDisposable Type(string type)
        {
            return m_Parent.Type(type);
        }

        public IDisposable ViolatedRule(string rule)
        {
            return m_Parent.ViolatedRule(rule);
        }

        public IList<string> GetResult()
        {
            var types = m_Parent.Types();

            return FeedbackPrinter.Print(types);
        }

        public void WriteFeedbackResults(string file)
        {
            var types = m_Parent.Types();
            types.Reverse();

            File.WriteAllLines(file, FeedbackPrinter.Print(types));
        }
    }
}