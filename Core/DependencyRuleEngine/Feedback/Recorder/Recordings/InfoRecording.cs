// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System;
using YADA.Core.Analyser;

namespace YADA.Core.DependencyRuleEngine.Feedback
{
    internal class InfoRecording : IRecording
    {
        private readonly IRecording m_Parent;
        
        public InfoRecording(string msg, IRecording parent) 
        {
            Info = msg;
            m_Parent = parent;
        }
        public bool ReadMode => m_Parent.ReadMode;
        public string Info { get; set; }

        public IDependencyContextVisitor<string> ContextVisitor => throw new NotImplementedException();

        public void Closed(IRecording current)
        {
            throw new NotImplementedException();
        }

        public IDisposable Context(IDependencyContext context)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            // do not inform violated rule
            //m_Parent.Closed(this);
        }

        public IDisposable ForbiddenDependency(string dependency)
        {
            throw new NotImplementedException();
        }

        public IDisposable Type(string type)
        {
            throw new NotImplementedException();
        }

        public IDisposable ViolatedRule(string rule)
        {
            throw new NotImplementedException();
        }

        IDisposable IFeedbackVisitor.Info(string msg)
        {
            throw new NotImplementedException();
        }
    }
}